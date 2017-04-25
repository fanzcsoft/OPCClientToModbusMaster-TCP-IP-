namespace OPC_To_Modbus_Server
{
    partial class AddForm
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
            this.OPC_Tag = new System.Windows.Forms.Label();
            this.Modbus_Adr = new System.Windows.Forms.Label();
            this.TagType = new System.Windows.Forms.Label();
            this.textBoxOPCTAG = new System.Windows.Forms.TextBox();
            this.textBoxModbusAdr = new System.Windows.Forms.TextBox();
            this.textBoxTagtype = new System.Windows.Forms.TextBox();
            this.SaveAdd = new System.Windows.Forms.Button();
            this.CancelAdd = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // OPC_Tag
            // 
            this.OPC_Tag.AutoSize = true;
            this.OPC_Tag.Location = new System.Drawing.Point(12, 15);
            this.OPC_Tag.Name = "OPC_Tag";
            this.OPC_Tag.Size = new System.Drawing.Size(54, 13);
            this.OPC_Tag.TabIndex = 0;
            this.OPC_Tag.Text = "OPC Tag.";
            // 
            // Modbus_Adr
            // 
            this.Modbus_Adr.AutoSize = true;
            this.Modbus_Adr.Location = new System.Drawing.Point(12, 41);
            this.Modbus_Adr.Name = "Modbus_Adr";
            this.Modbus_Adr.Size = new System.Drawing.Size(67, 13);
            this.Modbus_Adr.TabIndex = 1;
            this.Modbus_Adr.Text = "Modbus Adr.";
            // 
            // TagType
            // 
            this.TagType.AutoSize = true;
            this.TagType.Location = new System.Drawing.Point(12, 67);
            this.TagType.Name = "TagType";
            this.TagType.Size = new System.Drawing.Size(50, 13);
            this.TagType.TabIndex = 2;
            this.TagType.Text = "TagType";
            // 
            // textBoxOPCTAG
            // 
            this.textBoxOPCTAG.Location = new System.Drawing.Point(139, 12);
            this.textBoxOPCTAG.Name = "textBoxOPCTAG";
            this.textBoxOPCTAG.Size = new System.Drawing.Size(168, 20);
            this.textBoxOPCTAG.TabIndex = 3;
            // 
            // textBoxModbusAdr
            // 
            this.textBoxModbusAdr.Location = new System.Drawing.Point(139, 38);
            this.textBoxModbusAdr.Name = "textBoxModbusAdr";
            this.textBoxModbusAdr.Size = new System.Drawing.Size(168, 20);
            this.textBoxModbusAdr.TabIndex = 4;
            // 
            // textBoxTagtype
            // 
            this.textBoxTagtype.Location = new System.Drawing.Point(139, 64);
            this.textBoxTagtype.Name = "textBoxTagtype";
            this.textBoxTagtype.Size = new System.Drawing.Size(168, 20);
            this.textBoxTagtype.TabIndex = 5;
            // 
            // SaveAdd
            // 
            this.SaveAdd.Location = new System.Drawing.Point(161, 98);
            this.SaveAdd.Name = "SaveAdd";
            this.SaveAdd.Size = new System.Drawing.Size(69, 29);
            this.SaveAdd.TabIndex = 6;
            this.SaveAdd.Text = "save";
            this.SaveAdd.UseVisualStyleBackColor = true;
            this.SaveAdd.Click += new System.EventHandler(this.SaveAdd_Click);
            // 
            // CancelAdd
            // 
            this.CancelAdd.Location = new System.Drawing.Point(236, 98);
            this.CancelAdd.Name = "CancelAdd";
            this.CancelAdd.Size = new System.Drawing.Size(69, 29);
            this.CancelAdd.TabIndex = 7;
            this.CancelAdd.Text = "cancel";
            this.CancelAdd.UseVisualStyleBackColor = true;
            this.CancelAdd.Click += new System.EventHandler(this.CancelAdd_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(81, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "( 1-999 )";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(81, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "( B,F )";
            // 
            // AddForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(317, 139);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.CancelAdd);
            this.Controls.Add(this.SaveAdd);
            this.Controls.Add(this.textBoxTagtype);
            this.Controls.Add(this.textBoxModbusAdr);
            this.Controls.Add(this.textBoxOPCTAG);
            this.Controls.Add(this.TagType);
            this.Controls.Add(this.Modbus_Adr);
            this.Controls.Add(this.OPC_Tag);
            this.Name = "AddForm";
            this.Text = "AddForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label OPC_Tag;
        private System.Windows.Forms.Label Modbus_Adr;
        private System.Windows.Forms.Label TagType;
        private System.Windows.Forms.TextBox textBoxOPCTAG;
        private System.Windows.Forms.TextBox textBoxModbusAdr;
        private System.Windows.Forms.TextBox textBoxTagtype;
        private System.Windows.Forms.Button SaveAdd;
        private System.Windows.Forms.Button CancelAdd;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}