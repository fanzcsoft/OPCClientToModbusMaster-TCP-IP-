namespace OPC_To_Modbus_Server
{
    partial class SettingOPC
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
            this.Brown_OPC_Server = new System.Windows.Forms.Button();
            this.UpdateRate = new System.Windows.Forms.Label();
            this.DataLogger = new System.Windows.Forms.Label();
            this.Filename = new System.Windows.Forms.Label();
            this.textBoxUpdateRate = new System.Windows.Forms.TextBox();
            this.richTextBoxFilename = new System.Windows.Forms.RichTextBox();
            this.SaveSetting = new System.Windows.Forms.Button();
            this.CancelSetting = new System.Windows.Forms.Button();
            this.ms = new System.Windows.Forms.Label();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.Node = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Brown_OPC_Server
            // 
            this.Brown_OPC_Server.Location = new System.Drawing.Point(135, 15);
            this.Brown_OPC_Server.Name = "Brown_OPC_Server";
            this.Brown_OPC_Server.Size = new System.Drawing.Size(97, 34);
            this.Brown_OPC_Server.TabIndex = 0;
            this.Brown_OPC_Server.Text = "Brown OPC Server";
            this.Brown_OPC_Server.UseVisualStyleBackColor = true;
            this.Brown_OPC_Server.Click += new System.EventHandler(this.Brown_OPC_Server_Click);
            // 
            // UpdateRate
            // 
            this.UpdateRate.AutoSize = true;
            this.UpdateRate.Location = new System.Drawing.Point(9, 184);
            this.UpdateRate.Name = "UpdateRate";
            this.UpdateRate.Size = new System.Drawing.Size(65, 13);
            this.UpdateRate.TabIndex = 2;
            this.UpdateRate.Text = "UpdateRate";
            // 
            // DataLogger
            // 
            this.DataLogger.AutoSize = true;
            this.DataLogger.Location = new System.Drawing.Point(3, 0);
            this.DataLogger.Name = "DataLogger";
            this.DataLogger.Size = new System.Drawing.Size(63, 13);
            this.DataLogger.TabIndex = 3;
            this.DataLogger.Text = "DataLogger";
            // 
            // Filename
            // 
            this.Filename.AutoSize = true;
            this.Filename.Location = new System.Drawing.Point(3, 27);
            this.Filename.Name = "Filename";
            this.Filename.Size = new System.Drawing.Size(49, 13);
            this.Filename.TabIndex = 5;
            this.Filename.Text = "Filename";
            // 
            // textBoxUpdateRate
            // 
            this.textBoxUpdateRate.Location = new System.Drawing.Point(86, 183);
            this.textBoxUpdateRate.Name = "textBoxUpdateRate";
            this.textBoxUpdateRate.Size = new System.Drawing.Size(118, 20);
            this.textBoxUpdateRate.TabIndex = 6;
            // 
            // richTextBoxFilename
            // 
            this.richTextBoxFilename.Location = new System.Drawing.Point(58, 27);
            this.richTextBoxFilename.Name = "richTextBoxFilename";
            this.richTextBoxFilename.ReadOnly = true;
            this.richTextBoxFilename.Size = new System.Drawing.Size(162, 91);
            this.richTextBoxFilename.TabIndex = 8;
            this.richTextBoxFilename.Text = "";
            // 
            // SaveSetting
            // 
            this.SaveSetting.Location = new System.Drawing.Point(84, 344);
            this.SaveSetting.Name = "SaveSetting";
            this.SaveSetting.Size = new System.Drawing.Size(72, 33);
            this.SaveSetting.TabIndex = 9;
            this.SaveSetting.Text = "Save";
            this.SaveSetting.UseVisualStyleBackColor = true;
            this.SaveSetting.Click += new System.EventHandler(this.SaveSetting_Click);
            // 
            // CancelSetting
            // 
            this.CancelSetting.Location = new System.Drawing.Point(162, 344);
            this.CancelSetting.Name = "CancelSetting";
            this.CancelSetting.Size = new System.Drawing.Size(72, 33);
            this.CancelSetting.TabIndex = 10;
            this.CancelSetting.Text = "Cancel";
            this.CancelSetting.UseVisualStyleBackColor = true;
            this.CancelSetting.Click += new System.EventHandler(this.CancelSetting_Click);
            // 
            // ms
            // 
            this.ms.AutoSize = true;
            this.ms.Location = new System.Drawing.Point(208, 186);
            this.ms.Name = "ms";
            this.ms.Size = new System.Drawing.Size(24, 13);
            this.ms.TabIndex = 11;
            this.ms.Text = "sec";
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(12, 57);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(221, 108);
            this.listBox1.TabIndex = 13;
            // 
            // Node
            // 
            this.Node.Location = new System.Drawing.Point(12, 25);
            this.Node.Name = "Node";
            this.Node.Size = new System.Drawing.Size(111, 20);
            this.Node.TabIndex = 14;
            this.Node.Text = "localhost";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 13);
            this.label1.TabIndex = 15;
            this.label1.Text = "Node";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.Filename);
            this.panel1.Controls.Add(this.DataLogger);
            this.panel1.Controls.Add(this.richTextBoxFilename);
            this.panel1.Location = new System.Drawing.Point(2, 211);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(245, 127);
            this.panel1.TabIndex = 16;
            // 
            // SettingOPC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(249, 383);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Node);
            this.Controls.Add(this.CancelSetting);
            this.Controls.Add(this.SaveSetting);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.ms);
            this.Controls.Add(this.textBoxUpdateRate);
            this.Controls.Add(this.UpdateRate);
            this.Controls.Add(this.Brown_OPC_Server);
            this.Name = "SettingOPC";
            this.Text = "SettingOPC";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Brown_OPC_Server;
        private System.Windows.Forms.Label UpdateRate;
        private System.Windows.Forms.Label DataLogger;
        private System.Windows.Forms.Label Filename;
        private System.Windows.Forms.TextBox textBoxUpdateRate;
        private System.Windows.Forms.RichTextBox richTextBoxFilename;
        private System.Windows.Forms.Button SaveSetting;
        private System.Windows.Forms.Button CancelSetting;
        private System.Windows.Forms.Label ms;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.TextBox Node;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
    }
}