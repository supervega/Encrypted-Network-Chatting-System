namespace Messenger_Server
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.LBLog = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.BtnStartServer = new System.Windows.Forms.Button();
            this.BtnSaveLog = new System.Windows.Forms.Button();
            this.BtnNewClient = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // LBLog
            // 
            this.LBLog.FormattingEnabled = true;
            this.LBLog.Location = new System.Drawing.Point(12, 35);
            this.LBLog.Name = "LBLog";
            this.LBLog.Size = new System.Drawing.Size(347, 381);
            this.LBLog.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Log :";
            // 
            // BtnStartServer
            // 
            this.BtnStartServer.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnStartServer.Location = new System.Drawing.Point(377, 35);
            this.BtnStartServer.Name = "BtnStartServer";
            this.BtnStartServer.Size = new System.Drawing.Size(75, 27);
            this.BtnStartServer.TabIndex = 2;
            this.BtnStartServer.Text = "Start";
            this.BtnStartServer.UseVisualStyleBackColor = true;
            this.BtnStartServer.Click += new System.EventHandler(this.BtnStartServer_Click);
            // 
            // BtnSaveLog
            // 
            this.BtnSaveLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnSaveLog.Location = new System.Drawing.Point(284, 422);
            this.BtnSaveLog.Name = "BtnSaveLog";
            this.BtnSaveLog.Size = new System.Drawing.Size(75, 27);
            this.BtnSaveLog.TabIndex = 4;
            this.BtnSaveLog.Text = "Save Log";
            this.BtnSaveLog.UseVisualStyleBackColor = true;
            this.BtnSaveLog.Click += new System.EventHandler(this.BtnSaveLog_Click);
            // 
            // BtnNewClient
            // 
            this.BtnNewClient.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnNewClient.Location = new System.Drawing.Point(12, 422);
            this.BtnNewClient.Name = "BtnNewClient";
            this.BtnNewClient.Size = new System.Drawing.Size(162, 27);
            this.BtnNewClient.TabIndex = 5;
            this.BtnNewClient.Text = "Initiate New Client";
            this.BtnNewClient.UseVisualStyleBackColor = true;
            this.BtnNewClient.Click += new System.EventHandler(this.BtnNewClient_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(464, 454);
            this.Controls.Add(this.BtnNewClient);
            this.Controls.Add(this.BtnSaveLog);
            this.Controls.Add(this.BtnStartServer);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.LBLog);
            this.MaximumSize = new System.Drawing.Size(480, 492);
            this.MinimumSize = new System.Drawing.Size(480, 492);
            this.Name = "Form1";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Messenger Server";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox LBLog;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button BtnStartServer;
        private System.Windows.Forms.Button BtnSaveLog;
        private System.Windows.Forms.Button BtnNewClient;
    }
}

