using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using SharpPrivacy;
using SharpPrivacy.Cipher;
using SharpPrivacy.Cipher.Math;
using SharpPrivacy.OpenPGP;
using SharpPrivacy.OpenPGP.Messages;
using System.Security.Cryptography;

namespace Messenger_Server
{
    public class Manager
    {
        private TcpListener tcpListener;
        private Thread listenThread;

        public string SERVER_PRIVATE_KEY = "";
        public string SERVER_PUBLIC_KEY = "";

        List<Client> clients = new List<Client>();

        ListBox lb_Log;

        public Manager(ListBox lb)
        {
            lb_Log = lb;
            GenerateKeys();
        }

        public void Start()
        {
            clients = new List<Client>();
            this.tcpListener = new TcpListener(IPAddress.Any, 3000);
            this.listenThread = new Thread(new ThreadStart(ListenForClients));
            this.listenThread.Start();
        }

        public void ListenForClients()
        {
            this.tcpListener.Start();
            lb_Log.Invoke(new MethodInvoker(delegate { lb_Log.Items.Add("Listening Started.."); }));
            while (true)
            {
                //blocks until a client has connected to the server
                TcpClient client = this.tcpListener.AcceptTcpClient();
                lb_Log.Invoke(new MethodInvoker(delegate { lb_Log.Items.Add("A new client has been accepted ("+client.Client.RemoteEndPoint.ToString()+").."); }));
                Thread clientThread = new Thread(new ParameterizedThreadStart(ProcessNewConnection));
                clientThread.IsBackground = true;
                clientThread.Start(client);
            }
        }

        private void ProcessNewConnection(object ClientObj)
        {
            TcpClient tcpClient = (TcpClient)ClientObj;
            Client newClient = new Client(tcpClient, this);
            clients.Add(newClient);
            SendInitialInfo(newClient);
            lb_Log.Invoke(new MethodInvoker(delegate { lb_Log.Items.Add("Initial Info has been sent.."); }));
        }

        public void ProcessNewMessage(Message msg,Client newClient)
        {                      
            switch (msg.ExternalType)
            {
                case ExternalMsgTypes.ClientPk:
                    lb_Log.Invoke(new MethodInvoker(delegate { lb_Log.Items.Add("Processing new message :"); }));  
                    lb_Log.Invoke(new MethodInvoker(delegate { lb_Log.Items.Add("  External Type : " + msg.ExternalType.ToString()); }));
                    lb_Log.Invoke(new MethodInvoker(delegate { lb_Log.Items.Add("  Internal Type : " + msg.InternalType.ToString()); }));
                    lb_Log.Invoke(new MethodInvoker(delegate { lb_Log.Items.Add("  Source Name : " + msg.Source_Name); }));
                    lb_Log.Invoke(new MethodInvoker(delegate { lb_Log.Items.Add("  Destination Name : " + msg.DestinationName); }));
                    lb_Log.Invoke(new MethodInvoker(delegate { lb_Log.Items.Add("  Message Content : " + msg.Msg); }));

                    if (msg.Msg.Length > 0)
                    {
                        msg.InternalType = (InternalMsgTypes)Convert.ToInt32(msg.Msg[0].ToString());
                        int source_Lenght = Convert.ToInt32(msg.Msg[1].ToString());
                        msg.Source_Name = msg.Msg.Substring(2, source_Lenght);
                        msg.Msg = msg.Msg.Substring(source_Lenght + 2, msg.Msg.Length - source_Lenght - 2);
                    }

                    if (msg.InternalType == InternalMsgTypes.Registration)
                    {
                        if (IsNameAvailable(msg.Source_Name))
                        {
                            newClient.Name = msg.Source_Name;
                            newClient.PublicKey = msg.Msg;

                            Message AcceptanceMsg = new Message();
                            AcceptanceMsg.ExternalType = ExternalMsgTypes.Transfer;
                            AcceptanceMsg.InternalType = InternalMsgTypes.ValidName;
                            AcceptanceMsg.Source_Name = "SERVER";
                            AcceptanceMsg.Encrypt(SERVER_PRIVATE_KEY, newClient.PublicKey);
                            newClient.Send(AcceptanceMsg);

                            for (int i = 0; i < clients.Count; i++)
                            {
                                Message PublishMsg = new Message();
                                PublishMsg.ExternalType = ExternalMsgTypes.Info;
                                PublishMsg.InternalType = InternalMsgTypes.MessageTransfer;
                                PublishMsg.Source_Name = newClient.Name;
                                PublishMsg.Msg = newClient.PublicKey;

                                PublishMsg.Encrypt(SERVER_PRIVATE_KEY, clients[i].PublicKey);
                                clients[i].Send(PublishMsg);
                            }

                            foreach (Client item in clients)
                            {
                                Message newContact = new Message();
                                newContact.ExternalType = ExternalMsgTypes.Info;
                                newContact.Source_Name = item.Name;
                                newContact.Msg = item.PublicKey;

                                newContact.Encrypt(SERVER_PRIVATE_KEY, newClient.PublicKey);
                                newClient.Send(newContact);
                            }
                        }
                        else
                        {
                            Message RejectMsg = new Message();
                            RejectMsg.ExternalType = ExternalMsgTypes.Transfer;
                            RejectMsg.InternalType = InternalMsgTypes.InvalidName;
                            RejectMsg.Source_Name = "SERVER";
                            RejectMsg.Encrypt(SERVER_PRIVATE_KEY, newClient.PublicKey);
                            newClient.Send(RejectMsg);
                        }                        
                    }
                    break;
                case ExternalMsgTypes.Disconnect:
                    msg.Decrypt(SERVER_PRIVATE_KEY, newClient.PublicKey);
                    lb_Log.Invoke(new MethodInvoker(delegate { lb_Log.Items.Add("Processing new message :"); }));  
                    lb_Log.Invoke(new MethodInvoker(delegate { lb_Log.Items.Add("  External Type : " + msg.ExternalType.ToString()); }));
                    lb_Log.Invoke(new MethodInvoker(delegate { lb_Log.Items.Add("  Internal Type : " + msg.InternalType.ToString()); }));
                    lb_Log.Invoke(new MethodInvoker(delegate { lb_Log.Items.Add("  Source Name : " + msg.Source_Name); }));
                    lb_Log.Invoke(new MethodInvoker(delegate { lb_Log.Items.Add("  Destination Name : " + msg.DestinationName); }));
                    lb_Log.Invoke(new MethodInvoker(delegate { lb_Log.Items.Add("  Message Content : " + msg.Msg); }));

                    string disconnectedClientName = msg.Source_Name;
                    for (int i = 0; i < clients.Count; i++)
                    {
                        if (clients[i].Name != disconnectedClientName)
                        {
                            Message disconnectMsg = new Message();
                            disconnectMsg.ExternalType = ExternalMsgTypes.Transfer;
                            disconnectMsg.InternalType = InternalMsgTypes.Disconnect;
                            disconnectMsg.Source_Name = disconnectedClientName;
                            disconnectMsg.Encrypt(SERVER_PRIVATE_KEY, clients[i].PublicKey);

                            clients[i].Send(disconnectMsg);
                        }
                        else
                        {
                            clients.Remove(clients[i]);
                            //clients[i].Dispose();                            
                            i--;
                        }
                    }
                    
                    break;
                case ExternalMsgTypes.Transfer:
                    lb_Log.Invoke(new MethodInvoker(delegate { lb_Log.Items.Add("Processing new message :"); }));  
                    lb_Log.Invoke(new MethodInvoker(delegate { lb_Log.Items.Add("  External Type : " + msg.ExternalType.ToString()); }));
                    lb_Log.Invoke(new MethodInvoker(delegate { lb_Log.Items.Add("  Destination Name : " + msg.DestinationName); }));
                    for (int i = 0; i < clients.Count; i++)
                    {
                        if (clients[i].Name == msg.DestinationName)
                        {
                            clients[i].Send(msg);
                            break;
                        }
                    }
                    break;
            }
        }

        private void SendInitialInfo(Client clt)
        {
            Message ServerPKMessage = new Message();
            ServerPKMessage.ExternalType = ExternalMsgTypes.ServerPk;
            ServerPKMessage.Msg = SERVER_PUBLIC_KEY;
            clt.Send(ServerPKMessage);           
        }

        /*
        private Client GetClientByName(string name)
        {
            for (int i = 0; i < clients.Count; i++)
            {
                if (clients[i].Name == name)
                    return clients[i];
            }
            return null;
        }
         */

        private bool IsNameAvailable(string name)
        {
            for (int i = 0; i < clients.Count; i++)
            {
                if (clients[i].Name == name)
                    return false;
            }
            return true;
        }

        public void Dispose()
        {
            for (int i = 0; i < clients.Count; i++)
            {
                clients[i].Dispose();
            }
            if (listenThread != null && listenThread.IsAlive)
                listenThread.Abort();
            tcpListener.Stop();
            lb_Log.Invoke(new MethodInvoker(delegate { lb_Log.Items.Add("Listening Stopped."); }));
        }

        private void GenerateKeys()
        {
            RSACryptoServiceProvider RSAProvider = new RSACryptoServiceProvider(1024);
            SERVER_PRIVATE_KEY = RSAProvider.ToXmlString(true);
            SERVER_PUBLIC_KEY = RSAProvider.ToXmlString(false);
        }
    }
}
