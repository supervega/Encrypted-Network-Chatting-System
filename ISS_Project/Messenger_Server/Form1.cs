using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Messenger_Server
{
    public partial class Form1 : Form
    {
        Manager manager;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            manager.Dispose();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            manager = new Manager(LBLog);
        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void BtnStartServer_Click(object sender, EventArgs e)
        {
            if (BtnStartServer.Text == "Start")
            {
                manager.Start();
                BtnStartServer.Text = "Stop";
            }
            else
            {
                manager.Dispose();
                BtnStartServer.Text = "Start";
            }
        }

        private void BtnSaveLog_Click(object sender, EventArgs e)
        {
            SaveFileDialog SFD = new SaveFileDialog();
            if (SFD.ShowDialog() == DialogResult.OK)
            {
                string[] lines=new string[LBLog.Items.Count];
                for (int i = 0; i < lines.Length; i++)
			    {
                    lines[i]=LBLog.Items[i].ToString();
			    }
                File.WriteAllLines(SFD.FileName, lines);
            }
        }

        private void BtnNewClient_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("Messenger_Client.exe");
        }
    }
}
