namespace PI_OPC2Modbus
{
    partial class PI_OPC2Modbus
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.RegNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OPC_Tag = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Modbus_Adr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TimeStamp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Quality = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TagType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ServerStatusInTime = new System.Windows.Forms.Label();
            this.PortStatus = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.RegNo,
            this.OPC_Tag,
            this.Modbus_Adr,
            this.Value,
            this.TimeStamp,
            this.Quality,
            this.TagType});
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(240, 150);
            this.dataGridView1.TabIndex = 0;
            // 
            // RegNo
            // 
            this.RegNo.HeaderText = "RegNo";
            this.RegNo.Name = "RegNo";
            // 
            // OPC_Tag
            // 
            this.OPC_Tag.HeaderText = "OPC_Tag";
            this.OPC_Tag.Name = "OPC_Tag";
            // 
            // Modbus_Adr
            // 
            this.Modbus_Adr.HeaderText = "Modbus_Adr";
            this.Modbus_Adr.Name = "Modbus_Adr";
            // 
            // Value
            // 
            this.Value.HeaderText = "Value";
            this.Value.Name = "Value";
            // 
            // TimeStamp
            // 
            this.TimeStamp.HeaderText = "TimeStamp";
            this.TimeStamp.Name = "TimeStamp";
            // 
            // Quality
            // 
            this.Quality.HeaderText = "Quality";
            this.Quality.Name = "Quality";
            // 
            // TagType
            // 
            this.TagType.HeaderText = "TagType";
            this.TagType.Name = "TagType";
            // 
            // ServerStatusInTime
            // 
            this.ServerStatusInTime.AutoSize = true;
            this.ServerStatusInTime.Location = new System.Drawing.Point(0, 0);
            this.ServerStatusInTime.Name = "ServerStatusInTime";
            this.ServerStatusInTime.Size = new System.Drawing.Size(100, 23);
            this.ServerStatusInTime.TabIndex = 0;
            this.ServerStatusInTime.Text = "label1";
            // 
            // PortStatus
            // 
            this.PortStatus.AutoSize = true;
            this.PortStatus.Location = new System.Drawing.Point(0, 0);
            this.PortStatus.Name = "PortStatus";
            this.PortStatus.Size = new System.Drawing.Size(100, 23);
            this.PortStatus.TabIndex = 0;
            this.PortStatus.Text = "label1";
            // 
            // PI_OPC2Modbus
            // 
            this.ServiceName = "PI_OPC2Modbus";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label ServerStatusInTime;
        private System.Windows.Forms.Label PortStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn RegNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn OPC_Tag;
        private System.Windows.Forms.DataGridViewTextBoxColumn Modbus_Adr;
        private System.Windows.Forms.DataGridViewTextBoxColumn Value;
        private System.Windows.Forms.DataGridViewTextBoxColumn TimeStamp;
        private System.Windows.Forms.DataGridViewTextBoxColumn Quality;
        private System.Windows.Forms.DataGridViewTextBoxColumn TagType;
    }
}
