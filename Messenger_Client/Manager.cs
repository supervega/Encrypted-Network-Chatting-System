using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using SharpPrivacy;
using SharpPrivacy.Cipher;
using SharpPrivacy.Cipher.Math;
using SharpPrivacy.OpenPGP;
using SharpPrivacy.OpenPGP.Messages;
using System.Security.Cryptography;

namespace Messenger_Client
{
    public class Manager
    {
        TcpClient client=null;
        NetworkStream clientStream;
        private bool is_connected = false;
        private bool is_disconnected = false;

        public string SERVER_PUBLIC_KEY = "";

        public string CLIENT_NAME = "";

        public string CLIENT_PRIVATE_KEY = "";
        public string CLIENT_PUBLIC_KEY = "";
        public string CLIENT_AES_KEY = "";

        GroupBox gb_info = null;
        ListBox lb_contacts = null;
        TextBox txt_conversation = null;

        Dictionary<string, string> contacts = new Dictionary<string, string>();

        SharpPrivacy.Cipher.RSA KeyGenerator = new SharpPrivacy.Cipher.RSA();
        BigInteger PublicKey = new BigInteger();
        BigInteger PrivateKey = new BigInteger();

        public Manager(GroupBox gb,ListBox lb,TextBox txt)
        {
            gb_info = gb;
            lb_contacts = lb;
            txt_conversation = txt;
            GenerateKeys();
        }

        #region Properties

        public bool Is_Connected
        {
            get
            {
                return is_connected;
            }
            set
            {
                is_connected = value;
                gb_info.Invoke(new MethodInvoker(delegate
                {
                    gb_info.Enabled = is_connected;
                }));
            }
        }

        #endregion

        #region Functions

        public bool Connect()
        {
            try
            {
                if (client == null || !client.Connected)
                {
                    client = new TcpClient();
                    IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 3000);
                    client.Connect(serverEndPoint);
                    clientStream = client.GetStream();
                    byte[] data = new byte[8000];
                    clientStream.BeginRead(data, 0, data.Length, new AsyncCallback(ReceiveCallback), data);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error connecting to server : " + ex.Message);
                return false;
            }
            return true;
        }

        public void Disconnect()
        {
            try
            {
                Message Msg = new Message();
                Msg.Source_Name = CLIENT_NAME;
                Msg.ExternalType = ExternalMsgTypes.Disconnect;
                Msg.Encrypt(CLIENT_PRIVATE_KEY, SERVER_PUBLIC_KEY);
                byte[] newdata = Msg.ToBinary();
                clientStream.BeginWrite(newdata, 0, newdata.Length, WriteCallBack, newdata);                
                is_disconnected = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
            
        private void ReceiveCallback(IAsyncResult ar)
        {
            byte[] buffer = (byte[])ar.AsyncState;
            clientStream.EndRead(ar);
            //Convert to object     
            Message newMsg = new Message();
            newMsg.ToObject(buffer);            
            switch (newMsg.ExternalType)
            {
                case ExternalMsgTypes.Info:
                    newMsg.Decrypt(CLIENT_PRIVATE_KEY, SERVER_PUBLIC_KEY);
                    if (contacts.ContainsKey(newMsg.Source_Name))
                        contacts.Remove(newMsg.Source_Name);
                    contacts.Add(newMsg.Source_Name,newMsg.Msg);
                    UpdateContactsList();
                    break;
                case ExternalMsgTypes.ServerPk:
                    SERVER_PUBLIC_KEY = newMsg.Msg;
                    //Registration
                    Message Msg = new Message();
                    Msg.Msg = CLIENT_PUBLIC_KEY;
                    Msg.Source_Name = CLIENT_NAME;
                    Msg.ExternalType = ExternalMsgTypes.ClientPk;
                    Msg.InternalType = InternalMsgTypes.Registration;
                    Msg.Msg = (int)Msg.InternalType + Msg.Source_Name.Length.ToString() + Msg.Source_Name + Msg.Msg;
                    byte[] newdata = Msg.ToBinary();
                    clientStream.BeginWrite(newdata, 0, newdata.Length, WriteCallBack, newdata);
                    break;
                case ExternalMsgTypes.Transfer:
                    bool Is_Decrypted = false;
                    if (!newMsg.Decrypt(CLIENT_PRIVATE_KEY, SERVER_PUBLIC_KEY))
                    {
                        foreach (KeyValuePair<string, string> item in contacts)
                        {
                            if (newMsg.Decrypt(CLIENT_PRIVATE_KEY, item.Value))
                            {
                                Is_Decrypted = true;
                                break;
                            }
                        }
                    }
                    else
                        Is_Decrypted = true;
                    if (!Is_Decrypted)
                    {
                        MessageBox.Show("Couldn't find sender.");
                        break;
                    }
                    switch (newMsg.InternalType)
                    {
                        case InternalMsgTypes.Disconnect:
                            contacts.Remove(newMsg.Source_Name);
                            UpdateContactsList();
                            break;
                        case InternalMsgTypes.InvalidName:
                            Is_Connected = false;
                            MessageBox.Show("Please choose different name.");
                            break;
                        case InternalMsgTypes.MessageTransfer:
                            txt_conversation.Invoke(new MethodInvoker(delegate { txt_conversation.Text=txt_conversation.Text+newMsg.Source_Name + " Says : \r\n" + newMsg.Msg + "\r\n\r\n"; }));
                            break;
                        case InternalMsgTypes.ValidName:
                            Is_Connected = true;                            
                            break;
                    }
                    break;
            }
            byte[] ndata = new byte[8000];
            clientStream.BeginRead(ndata, 0, ndata.Length, new AsyncCallback(ReceiveCallback), ndata);
        }

        private void WriteCallBack(IAsyncResult res)
        {
            byte[] data= (byte[])res.AsyncState;
            clientStream.EndWrite(res);
            if (is_disconnected)
            {
                clientStream.Close();
                if (client != null)
                    client.Close();                
            }           
        }

        public void SendMessage(string DestinationName,string message)
        {
            if (client.Connected)
            {
                Message Msg = new Message();
                Msg.ExternalType = ExternalMsgTypes.Transfer;
                Msg.InternalType = InternalMsgTypes.MessageTransfer;
                Msg.Msg = message;
                Msg.DestinationName = DestinationName;
                Msg.Source_Name = CLIENT_NAME;

                Msg.Encrypt(CLIENT_PRIVATE_KEY, contacts[DestinationName]);
                byte[] data = Msg.ToBinary();

                clientStream.BeginWrite(data, 0, data.Length, WriteCallBack, data);
            }
            else
                MessageBox.Show("Please connect first.");
        }

        private void GenerateKeys()
        {
            RSACryptoServiceProvider RSAProvider = new RSACryptoServiceProvider(1024);
            CLIENT_PRIVATE_KEY = RSAProvider.ToXmlString(true);
            CLIENT_PUBLIC_KEY = RSAProvider.ToXmlString(false);
        }

        private void UpdateContactsList()
        {
            lb_contacts.Invoke(new MethodInvoker(delegate { lb_contacts.Items.Clear(); }));
            foreach (var item in contacts)
            {
                string str = item.Key;
                if(str.Trim().Length>0)
                    lb_contacts.Invoke(new MethodInvoker(delegate { lb_contacts.Items.Add(str); }));
            }
            lb_contacts.Invoke(new MethodInvoker(delegate { lb_contacts.Update(); }));
        }       

        #endregion
    }
}
