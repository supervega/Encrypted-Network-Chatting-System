using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace Messenger_Server
{
    public class Client
    {
        private string name="";
        private string publicKey="";

        Manager managerRef; 

        private TcpClient tcpsocket;
        private Thread receiveThread;

        public Client(TcpClient client,Manager manRef)
        {
            tcpsocket = client;
            managerRef = manRef;

            if (tcpsocket != null)
            {
                receiveThread = new Thread(new ThreadStart(Receive));
                receiveThread.IsBackground = true;
                receiveThread.Start();
            }
        }

        #region Properties

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }        
        
        public string PublicKey
        {
            get
            {
                return publicKey;
            }
            set
            {
                publicKey = value;
            }
        }

        #endregion

        #region Functions

        private void Receive()
        {
            NetworkStream clientStream = tcpsocket.GetStream();
            while (true)
            {
                if (clientStream.CanRead && clientStream.DataAvailable)
                {
                    byte[] buffer = new byte[8000];
                    clientStream.Read(buffer, 0, buffer.Length);
                    
                    Message newMsg = new Message();
                    newMsg.ToObject(buffer);
                    managerRef.ProcessNewMessage(newMsg,this);
                }
            }
        }

        public void Send(Message msg)
        {
            if (tcpsocket.Connected)
            {                
                NetworkStream clientStream = tcpsocket.GetStream();
                //Convert to binary :  
                byte[] data = msg.ToBinary();
                clientStream.Write(data, 0, data.Length);
            }
            else
                MessageBox.Show("Please connect first.");
        }

        public void Dispose()
        {
            try
            {
                if (receiveThread != null && receiveThread.IsAlive)
                    receiveThread.Abort();
            }
            catch (Exception ex)
            {
                ;
            }
        }

        #endregion
    }
}
