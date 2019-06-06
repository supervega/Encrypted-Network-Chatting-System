using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Messenger_Client
{
    public partial class Form1 : Form
    {
        Manager manager=null;

        public Form1()
        {
            InitializeComponent();
        }

        private void BtnConnect_Click(object sender, EventArgs e)
        {
            if (manager == null)
            {
                manager = new Manager(GB_Details, LBContacts, TXTConversation);
            }
            if (BtnConnect.Text == "Connect")
            {
                manager.CLIENT_NAME = TXT_Name.Text;
                if (manager.Connect())
                {
                    BtnConnect.Text = "Disconnect";
                    TXT_Name.Enabled = false;
                }
            }
            else
            {
                manager.Disconnect();
                try
                {
                    GB_Details.Invoke(new MethodInvoker(delegate
                    {
                        GB_Details.Enabled = false;
                    }));
                    TXT_Name.Invoke(new MethodInvoker(delegate
                    {
                        TXT_Name.Enabled = true;
                    }));
                    BtnConnect.Text = "Connect";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                Application.Exit();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            //MessageBox.Show(e.ExceptionObject.ToString());
        }

        private void BtnSendMsg_Click(object sender, EventArgs e)
        {
            if(LBContacts.SelectedIndex>-1)
            {
                TXTConversation.Text = TXTConversation.Text + manager.CLIENT_NAME + " Says : \r\n" + TXTMessage.Text + "\r\n\r\n";
                manager.SendMessage(LBContacts.Items[LBContacts.SelectedIndex].ToString(), TXTMessage.Text);
                TXTMessage.Text = "";
            }
            else
            {
                MessageBox.Show("Please select contact first.");
            }   
        }

        private void TXTMessage_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                BtnSendMsg_Click(sender, e);
        }
    }
}
