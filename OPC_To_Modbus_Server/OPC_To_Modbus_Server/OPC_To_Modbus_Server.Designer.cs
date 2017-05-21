namespace OPC_To_Modbus_Server
{
    partial class OPC_To_ModBus_Server
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OPC_To_ModBus_Server));
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.RegNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OPC_Tag = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Modbus_Adr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TimeStamp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Quality = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TagType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.settingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.oPCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportCSVToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importCSVToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Add = new System.Windows.Forms.Button();
            this.OPC_Server_Name = new System.Windows.Forms.Label();
            this.ModBus_Port = new System.Windows.Forms.Label();
            this.textBoxServerName = new System.Windows.Forms.TextBox();
            this.textBoxModBus_Port = new System.Windows.Forms.TextBox();
            this.Modbus_Unit_ID = new System.Windows.Forms.Label();
            this.textBoxUnitID = new System.Windows.Forms.TextBox();
            this.Edit = new System.Windows.Forms.Button();
            this.Delete = new System.Windows.Forms.Button();
            this.Save = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.Server_Status = new System.Windows.Forms.Label();
            this.ServerStatusInTime = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.StopPort = new System.Windows.Forms.Button();
            this.StartPort = new System.Windows.Forms.Button();
            this.PortStatus = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.timer2ReconnectOPC = new System.Windows.Forms.Timer(this.components);
            this.timerModbus = new System.Windows.Forms.Timer(this.components);
            this.textBoxOPCTAG = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.RegNo,
            this.OPC_Tag,
            this.Modbus_Adr,
            this.Value,
            this.TimeStamp,
            this.Quality,
            this.TagType});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(835, 321);
            this.dataGridView1.TabIndex = 1;
            this.dataGridView1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dataGridView1_MouseClick);
            // 
            // RegNo
            // 
            this.RegNo.FillWeight = 50F;
            this.RegNo.HeaderText = "RegNo";
            this.RegNo.Name = "RegNo";
            this.RegNo.ReadOnly = true;
            this.RegNo.Width = 66;
            // 
            // OPC_Tag
            // 
            this.OPC_Tag.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.OPC_Tag.FillWeight = 200F;
            this.OPC_Tag.HeaderText = "OPC Tag.";
            this.OPC_Tag.Name = "OPC_Tag";
            this.OPC_Tag.ReadOnly = true;
            // 
            // Modbus_Adr
            // 
            this.Modbus_Adr.HeaderText = "Modbus Adr.";
            this.Modbus_Adr.Name = "Modbus_Adr";
            this.Modbus_Adr.ReadOnly = true;
            this.Modbus_Adr.Width = 92;
            // 
            // Value
            // 
            this.Value.FillWeight = 80F;
            this.Value.HeaderText = "Value";
            this.Value.Name = "Value";
            this.Value.ReadOnly = true;
            this.Value.Width = 80;
            // 
            // TimeStamp
            // 
            this.TimeStamp.FillWeight = 120F;
            this.TimeStamp.HeaderText = "TimeStamp";
            this.TimeStamp.Name = "TimeStamp";
            this.TimeStamp.ReadOnly = true;
            this.TimeStamp.Width = 120;
            // 
            // Quality
            // 
            this.Quality.HeaderText = "Quality";
            this.Quality.Name = "Quality";
            this.Quality.ReadOnly = true;
            // 
            // TagType
            // 
            this.TagType.FillWeight = 70F;
            this.TagType.HeaderText = "TagType";
            this.TagType.Name = "TagType";
            this.TagType.ReadOnly = true;
            this.TagType.Width = 70;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(835, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingToolStripMenuItem,
            this.exportCSVToolStripMenuItem,
            this.importCSVToolStripMenuItem,
            this.resetToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(47, 20);
            this.toolStripMenuItem1.Text = "Tools";
            // 
            // settingToolStripMenuItem
            // 
            this.settingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.oPCToolStripMenuItem});
            this.settingToolStripMenuItem.Name = "settingToolStripMenuItem";
            this.settingToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.settingToolStripMenuItem.Text = "Setting";
            // 
            // oPCToolStripMenuItem
            // 
            this.oPCToolStripMenuItem.Name = "oPCToolStripMenuItem";
            this.oPCToolStripMenuItem.Size = new System.Drawing.Size(98, 22);
            this.oPCToolStripMenuItem.Text = "OPC";
            this.oPCToolStripMenuItem.Click += new System.EventHandler(this.oPCToolStripMenuItem_Click);
            // 
            // exportCSVToolStripMenuItem
            // 
            this.exportCSVToolStripMenuItem.Name = "exportCSVToolStripMenuItem";
            this.exportCSVToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.exportCSVToolStripMenuItem.Text = "Export CSV";
            this.exportCSVToolStripMenuItem.Click += new System.EventHandler(this.exportCSVToolStripMenuItem_Click);
            // 
            // importCSVToolStripMenuItem
            // 
            this.importCSVToolStripMenuItem.Name = "importCSVToolStripMenuItem";
            this.importCSVToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.importCSVToolStripMenuItem.Text = "Import CSV";
            this.importCSVToolStripMenuItem.Click += new System.EventHandler(this.importCSVToolStripMenuItem_Click);
            // 
            // resetToolStripMenuItem
            // 
            this.resetToolStripMenuItem.Name = "resetToolStripMenuItem";
            this.resetToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.resetToolStripMenuItem.Text = "ReconnectOPC";
            this.resetToolStripMenuItem.Click += new System.EventHandler(this.resetToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // Add
            // 
            this.Add.Location = new System.Drawing.Point(438, 33);
            this.Add.Name = "Add";
            this.Add.Size = new System.Drawing.Size(83, 25);
            this.Add.TabIndex = 0;
            this.Add.Text = "Add";
            this.Add.UseVisualStyleBackColor = true;
            this.Add.Click += new System.EventHandler(this.Add_Click);
            // 
            // OPC_Server_Name
            // 
            this.OPC_Server_Name.AutoSize = true;
            this.OPC_Server_Name.Location = new System.Drawing.Point(3, 12);
            this.OPC_Server_Name.Name = "OPC_Server_Name";
            this.OPC_Server_Name.Size = new System.Drawing.Size(94, 13);
            this.OPC_Server_Name.TabIndex = 3;
            this.OPC_Server_Name.Text = "OPC Server Name";
            // 
            // ModBus_Port
            // 
            this.ModBus_Port.AutoSize = true;
            this.ModBus_Port.Location = new System.Drawing.Point(4, 39);
            this.ModBus_Port.Name = "ModBus_Port";
            this.ModBus_Port.Size = new System.Drawing.Size(67, 13);
            this.ModBus_Port.TabIndex = 4;
            this.ModBus_Port.Text = "Modbus Port";
            // 
            // textBoxServerName
            // 
            this.textBoxServerName.Location = new System.Drawing.Point(103, 9);
            this.textBoxServerName.Name = "textBoxServerName";
            this.textBoxServerName.ReadOnly = true;
            this.textBoxServerName.Size = new System.Drawing.Size(324, 20);
            this.textBoxServerName.TabIndex = 5;
            // 
            // textBoxModBus_Port
            // 
            this.textBoxModBus_Port.Location = new System.Drawing.Point(103, 36);
            this.textBoxModBus_Port.Name = "textBoxModBus_Port";
            this.textBoxModBus_Port.ReadOnly = true;
            this.textBoxModBus_Port.Size = new System.Drawing.Size(111, 20);
            this.textBoxModBus_Port.TabIndex = 6;
            // 
            // Modbus_Unit_ID
            // 
            this.Modbus_Unit_ID.AutoSize = true;
            this.Modbus_Unit_ID.Location = new System.Drawing.Point(223, 39);
            this.Modbus_Unit_ID.Name = "Modbus_Unit_ID";
            this.Modbus_Unit_ID.Size = new System.Drawing.Size(81, 13);
            this.Modbus_Unit_ID.TabIndex = 7;
            this.Modbus_Unit_ID.Text = "Modbus Unit ID";
            // 
            // textBoxUnitID
            // 
            this.textBoxUnitID.Location = new System.Drawing.Point(305, 36);
            this.textBoxUnitID.Name = "textBoxUnitID";
            this.textBoxUnitID.ReadOnly = true;
            this.textBoxUnitID.Size = new System.Drawing.Size(122, 20);
            this.textBoxUnitID.TabIndex = 8;
            // 
            // Edit
            // 
            this.Edit.Location = new System.Drawing.Point(527, 33);
            this.Edit.Name = "Edit";
            this.Edit.Size = new System.Drawing.Size(83, 25);
            this.Edit.TabIndex = 9;
            this.Edit.Text = "Edit";
            this.Edit.UseVisualStyleBackColor = true;
            this.Edit.Click += new System.EventHandler(this.Edit_Click);
            // 
            // Delete
            // 
            this.Delete.Location = new System.Drawing.Point(616, 33);
            this.Delete.Name = "Delete";
            this.Delete.Size = new System.Drawing.Size(83, 25);
            this.Delete.TabIndex = 10;
            this.Delete.Text = "Delete";
            this.Delete.UseVisualStyleBackColor = true;
            this.Delete.Click += new System.EventHandler(this.Delete_Click);
            // 
            // Save
            // 
            this.Save.Location = new System.Drawing.Point(381, 103);
            this.Save.Name = "Save";
            this.Save.Size = new System.Drawing.Size(83, 25);
            this.Save.TabIndex = 11;
            this.Save.Text = "Save";
            this.Save.UseVisualStyleBackColor = true;
            this.Save.Visible = false;
            this.Save.Click += new System.EventHandler(this.Save_Click);
            // 
            // Cancel
            // 
            this.Cancel.Location = new System.Drawing.Point(471, 103);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(83, 25);
            this.Cancel.TabIndex = 12;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Visible = false;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // Server_Status
            // 
            this.Server_Status.AutoSize = true;
            this.Server_Status.Location = new System.Drawing.Point(4, 64);
            this.Server_Status.Name = "Server_Status";
            this.Server_Status.Size = new System.Drawing.Size(96, 13);
            this.Server_Status.TabIndex = 13;
            this.Server_Status.Text = "OPC Server Status";
            // 
            // ServerStatusInTime
            // 
            this.ServerStatusInTime.AutoSize = true;
            this.ServerStatusInTime.Location = new System.Drawing.Point(101, 64);
            this.ServerStatusInTime.Name = "ServerStatusInTime";
            this.ServerStatusInTime.Size = new System.Drawing.Size(29, 13);
            this.ServerStatusInTime.TabIndex = 14;
            this.ServerStatusInTime.Text = "Stop";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.textBoxOPCTAG);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.StopPort);
            this.panel1.Controls.Add(this.StartPort);
            this.panel1.Controls.Add(this.PortStatus);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.ServerStatusInTime);
            this.panel1.Controls.Add(this.Cancel);
            this.panel1.Controls.Add(this.Server_Status);
            this.panel1.Controls.Add(this.Save);
            this.panel1.Controls.Add(this.ModBus_Port);
            this.panel1.Controls.Add(this.OPC_Server_Name);
            this.panel1.Controls.Add(this.textBoxServerName);
            this.panel1.Controls.Add(this.Delete);
            this.panel1.Controls.Add(this.textBoxModBus_Port);
            this.panel1.Controls.Add(this.Edit);
            this.panel1.Controls.Add(this.Modbus_Unit_ID);
            this.panel1.Controls.Add(this.textBoxUnitID);
            this.panel1.Controls.Add(this.Add);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 24);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(835, 140);
            this.panel1.TabIndex = 15;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 87);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(331, 13);
            this.label2.TabIndex = 19;
            this.label2.Text = "Caution Holding register Length In Modbus Slave Must Not Over 125";
            // 
            // StopPort
            // 
            this.StopPort.Location = new System.Drawing.Point(527, 61);
            this.StopPort.Name = "StopPort";
            this.StopPort.Size = new System.Drawing.Size(83, 23);
            this.StopPort.TabIndex = 18;
            this.StopPort.Text = "StopPort";
            this.StopPort.UseVisualStyleBackColor = true;
            this.StopPort.Visible = false;
            this.StopPort.Click += new System.EventHandler(this.StopPort_Click);
            // 
            // StartPort
            // 
            this.StartPort.Location = new System.Drawing.Point(438, 61);
            this.StartPort.Name = "StartPort";
            this.StartPort.Size = new System.Drawing.Size(83, 23);
            this.StartPort.TabIndex = 17;
            this.StartPort.Text = "StartPort";
            this.StartPort.UseVisualStyleBackColor = true;
            this.StartPort.Visible = false;
            this.StartPort.Click += new System.EventHandler(this.StartPort_Click);
            // 
            // PortStatus
            // 
            this.PortStatus.AutoSize = true;
            this.PortStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PortStatus.Location = new System.Drawing.Point(306, 64);
            this.PortStatus.Name = "PortStatus";
            this.PortStatus.Size = new System.Drawing.Size(59, 15);
            this.PortStatus.TabIndex = 16;
            this.PortStatus.Text = "dont know";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(223, 64);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 15;
            this.label1.Text = "Mosbus Status";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dataGridView1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 164);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(835, 321);
            this.panel2.TabIndex = 16;
            // 
            // timer2ReconnectOPC
            // 
            this.timer2ReconnectOPC.Enabled = true;
            this.timer2ReconnectOPC.Interval = 60000;
            this.timer2ReconnectOPC.Tick += new System.EventHandler(this.timer2ReconnectOPC_Tick);
            // 
            // timerModbus
            // 
            this.timerModbus.Enabled = true;
            this.timerModbus.Tick += new System.EventHandler(this.timerModbus_Tick);
            // 
            // textBoxOPCTAG
            // 
            this.textBoxOPCTAG.Location = new System.Drawing.Point(103, 103);
            this.textBoxOPCTAG.Name = "textBoxOPCTAG";
            this.textBoxOPCTAG.Size = new System.Drawing.Size(272, 20);
            this.textBoxOPCTAG.TabIndex = 20;
            this.textBoxOPCTAG.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(36, 106);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 13);
            this.label3.TabIndex = 21;
            this.label3.Text = "OPC TAG";
            this.label3.Visible = false;
            // 
            // OPC_To_ModBus_Server
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(835, 485);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "OPC_To_ModBus_Server";
            this.Text = "OPC To ModBus Server";
            this.Load += new System.EventHandler(this.OPC_To_ModBus_Server_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem settingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem oPCToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportCSVToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importCSVToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        public System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn RegNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn OPC_Tag;
        private System.Windows.Forms.DataGridViewTextBoxColumn Modbus_Adr;
        private System.Windows.Forms.DataGridViewTextBoxColumn Value;
        private System.Windows.Forms.DataGridViewTextBoxColumn TimeStamp;
        private System.Windows.Forms.DataGridViewTextBoxColumn Quality;
        private System.Windows.Forms.DataGridViewTextBoxColumn TagType;
        private System.Windows.Forms.Button Add;
        private System.Windows.Forms.Label OPC_Server_Name;
        private System.Windows.Forms.Label ModBus_Port;
        private System.Windows.Forms.TextBox textBoxServerName;
        private System.Windows.Forms.TextBox textBoxModBus_Port;
        private System.Windows.Forms.Label Modbus_Unit_ID;
        private System.Windows.Forms.TextBox textBoxUnitID;
        private System.Windows.Forms.Button Edit;
        private System.Windows.Forms.Button Delete;
        private System.Windows.Forms.Button Save;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.Label Server_Status;
        private System.Windows.Forms.Label ServerStatusInTime;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button StopPort;
        private System.Windows.Forms.Button StartPort;
        private System.Windows.Forms.Label PortStatus;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.Timer timer2ReconnectOPC;
        public System.Windows.Forms.Timer timerModbus;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxOPCTAG;
    }
}

