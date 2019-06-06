namespace Messenger_Client
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
            this.TXTConversation = new System.Windows.Forms.TextBox();
            this.TXTMessage = new System.Windows.Forms.TextBox();
            this.BtnSendMsg = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.BtnConnect = new System.Windows.Forms.Button();
            this.LBContacts = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.TXT_Name = new System.Windows.Forms.TextBox();
            this.GB_Details = new System.Windows.Forms.GroupBox();
            this.GB_Details.SuspendLayout();
            this.SuspendLayout();
            // 
            // TXTConversation
            // 
            this.TXTConversation.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TXTConversation.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TXTConversation.Location = new System.Drawing.Point(153, 39);
            this.TXTConversation.Multiline = true;
            this.TXTConversation.Name = "TXTConversation";
            this.TXTConversation.ReadOnly = true;
            this.TXTConversation.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TXTConversation.Size = new System.Drawing.Size(378, 231);
            this.TXTConversation.TabIndex = 0;
            // 
            // TXTMessage
            // 
            this.TXTMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TXTMessage.Location = new System.Drawing.Point(153, 273);
            this.TXTMessage.MaxLength = 100;
            this.TXTMessage.Name = "TXTMessage";
            this.TXTMessage.Size = new System.Drawing.Size(297, 26);
            this.TXTMessage.TabIndex = 1;
            this.TXTMessage.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TXTMessage_KeyUp);
            // 
            // BtnSendMsg
            // 
            this.BtnSendMsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnSendMsg.Location = new System.Drawing.Point(456, 273);
            this.BtnSendMsg.Name = "BtnSendMsg";
            this.BtnSendMsg.Size = new System.Drawing.Size(75, 27);
            this.BtnSendMsg.TabIndex = 2;
            this.BtnSendMsg.Text = "Send";
            this.BtnSendMsg.UseVisualStyleBackColor = true;
            this.BtnSendMsg.Click += new System.EventHandler(this.BtnSendMsg_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(149, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(124, 20);
            this.label1.TabIndex = 3;
            this.label1.Text = "Conversation :";
            // 
            // BtnConnect
            // 
            this.BtnConnect.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnConnect.Location = new System.Drawing.Point(310, 12);
            this.BtnConnect.Name = "BtnConnect";
            this.BtnConnect.Size = new System.Drawing.Size(97, 27);
            this.BtnConnect.TabIndex = 4;
            this.BtnConnect.Text = "Connect";
            this.BtnConnect.UseVisualStyleBackColor = true;
            this.BtnConnect.Click += new System.EventHandler(this.BtnConnect_Click);
            // 
            // LBContacts
            // 
            this.LBContacts.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LBContacts.FormattingEnabled = true;
            this.LBContacts.ItemHeight = 16;
            this.LBContacts.Location = new System.Drawing.Point(10, 39);
            this.LBContacts.Name = "LBContacts";
            this.LBContacts.Size = new System.Drawing.Size(137, 260);
            this.LBContacts.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(6, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(91, 20);
            this.label3.TabIndex = 7;
            this.label3.Text = "Contacts :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(157, 20);
            this.label2.TabIndex = 8;
            this.label2.Text = "Enter Your Name :";
            // 
            // TXT_Name
            // 
            this.TXT_Name.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TXT_Name.Location = new System.Drawing.Point(175, 14);
            this.TXT_Name.MaxLength = 9;
            this.TXT_Name.Name = "TXT_Name";
            this.TXT_Name.Size = new System.Drawing.Size(129, 22);
            this.TXT_Name.TabIndex = 9;
            // 
            // GB_Details
            // 
            this.GB_Details.Controls.Add(this.label3);
            this.GB_Details.Controls.Add(this.TXTConversation);
            this.GB_Details.Controls.Add(this.TXTMessage);
            this.GB_Details.Controls.Add(this.BtnSendMsg);
            this.GB_Details.Controls.Add(this.LBContacts);
            this.GB_Details.Controls.Add(this.label1);
            this.GB_Details.Enabled = false;
            this.GB_Details.Location = new System.Drawing.Point(12, 42);
            this.GB_Details.Name = "GB_Details";
            this.GB_Details.Size = new System.Drawing.Size(543, 316);
            this.GB_Details.TabIndex = 10;
            this.GB_Details.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(567, 357);
            this.Controls.Add(this.GB_Details);
            this.Controls.Add(this.TXT_Name);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.BtnConnect);
            this.MaximumSize = new System.Drawing.Size(583, 395);
            this.MinimumSize = new System.Drawing.Size(583, 395);
            this.Name = "Form1";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Messenger Client";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.GB_Details.ResumeLayout(false);
            this.GB_Details.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox TXTConversation;
        private System.Windows.Forms.TextBox TXTMessage;
        private System.Windows.Forms.Button BtnSendMsg;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button BtnConnect;
        private System.Windows.Forms.ListBox LBContacts;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TXT_Name;
        private System.Windows.Forms.GroupBox GB_Details;
    }
}

