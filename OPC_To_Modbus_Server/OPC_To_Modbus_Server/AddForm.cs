using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Data.OleDb;
using OPC_To_Modbus_Server;
using System.IO;
using System.Threading;

namespace OPC_To_Modbus_Server
{
    
    public partial class AddForm : Form
    {
        //DataTable Pub_dtTTAGMapping = new DataTable();
        private readonly OPC_To_ModBus_Server publicmainform;        
        
        //String conn_string = "Provider = Microsoft.ACE.OLEDB.12.0; Data Source = C:\\Users\\PorNB\\Documents\\Visual Studio 2015\\Projects1\\OPC_To_Modbus_Server\\OPC_To_Modbus_Server\\bin\\Debug\\TagMapping.mdb;Persist Security Info=False";
        OleDbConnection conn = new OleDbConnection(@"Provider = Microsoft.ACE.OLEDB.12.0; Data Source = " + Directory.GetCurrentDirectory() + "\\TagMapping.mdb;Persist Security Info=False");

        public AddForm(OPC_To_ModBus_Server mainform)
        {
            InitializeComponent();
            //Get_TagMapping();
            publicmainform = mainform;
            Console.WriteLine("Add Form");                          
        }

        //public void Get_TagMapping()
        //{
            
        //    conn.Open();
        //    String q = "SELECT ID,RegNo,OPC_TAGNAME,MB_ADDRESS,TagType FROM TTAGMapping Order By RegNo";
        //    OleDbCommand cmd = new OleDbCommand(q, conn);
        //    OleDbDataAdapter c = new OleDbDataAdapter(cmd);           
        //    Pub_dtTTAGMapping = new DataTable();
        //    Pub_dtTTAGMapping.Clear();
        //    c.SelectCommand = cmd;
        //    c.Fill(Pub_dtTTAGMapping);
        //    conn.Close();
        //}

        public void SaveAdd_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Confirmation Save ?", "Confirm", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                //insert
                TagMappingDataSetTableAdapters.TTAGMappingTableAdapter regionTableAdapter =
               new TagMappingDataSetTableAdapters.TTAGMappingTableAdapter();

                conn.Open();
                String q = "SELECT MAX(RegNo) FROM TTAGMapping;";
                OleDbCommand cmd = new OleDbCommand(q, conn);
                int maxRegNo = Convert.ToInt32(cmd.ExecuteScalar());
                conn.Close();
//Check Tag name-------And Modbus Address------------------------------------------------------------------------------------------
                int checksametagname = 0;
                int errorModbusAddress = 0;
                //int modbusadd = Convert.ToInt32(textBoxModbusAdr.Text);
                int checkint_MBaddress_addform = 0;
                string New_opctag = textBoxOPCTAG.Text;
                
                for (int row=0;row< publicmainform.Pub_dtTTAGMapping.Rows.Count; row++)
                {
                    string opctag_Ondatagridview = publicmainform.dataGridView1.Rows[row].Cells["OPC_Tag"].Value.ToString();
                    //check same TagName
                    if (New_opctag == opctag_Ondatagridview)
                    {
                        MessageBox.Show("Error Tag_Name same Added", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        checksametagname = 1;
                        break;
                    }
                    //check modbus is integer--------------------------------------------
                    
                    string string_Mbaddress = textBoxModbusAdr.Text;
                    if (int.TryParse(string_Mbaddress, out checkint_MBaddress_addform))
                    {
                        int mbvalueinput = Convert.ToInt32(textBoxModbusAdr.Text);
                        try
                        {
                            if (mbvalueinput < 0 && mbvalueinput > 99999)
                            {
                                MessageBox.Show("Error ModbusValue", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                errorModbusAddress = 1;
                                break;
                            }
                            //check same modbus_address
                            int modbusgridview = Convert.ToInt32(publicmainform.dataGridView1.Rows[row].Cells["Modbus_Adr"].Value);
                            string tagtype = publicmainform.dataGridView1.Rows[row].Cells["TagType"].Value.ToString();
                            string tagtypeaddform = textBoxTagtype.Text;
                            if (tagtype == "F" && tagtypeaddform == "F")
                            {
                                if (checkint_MBaddress_addform == (modbusgridview - 400000))
                                {
                                    MessageBox.Show("Error ModbusAddress same Added", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    errorModbusAddress = 1;
                                    break;
                                }
                            }
                            else if (tagtype == "B" && tagtypeaddform == "B")
                            {
                                if (checkint_MBaddress_addform == (modbusgridview- 100000))
                                {
                                    MessageBox.Show("Error ModbusAddress same Added", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    errorModbusAddress = 1;
                                    break;
                                }
                            }
                        }
                        catch(Exception ex)
                        {
                            
                        }
                    }//end try parseInt True
                    else
                    {
                        MessageBox.Show("Error ModbusValue is not Integer", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        errorModbusAddress = 1;
                        break;
                    }//end try parseInt False
                    //---------------------------------------------------------------------                    
                }//end for loop                
//--------------------------------------------------------------------------------------------------------------------------------------------------------
//Decide insert or not---------------------------------------------------------------------------------------------------------------------------------------
                if (checksametagname == 0 && errorModbusAddress == 0)
                {
                    regionTableAdapter.Insert(maxRegNo + 1, textBoxOPCTAG.Text, 0, "", Convert.ToInt32(textBoxModbusAdr.Text), 0, textBoxTagtype.Text);
                    publicmainform.dataGridView1.Rows.Add();

                    //publicmainform.Get_TagMapping();
                    var th = new Thread(publicmainform.Get_TagMapping);
                    th.IsBackground = true;
                    th.Start();
                    Thread.Sleep(100);
                    //Console.WriteLine("Main thread ({0}) exiting...",
                    //                  Thread.CurrentThread.ManagedThreadId);
                    
                    //publicmainform.AddValue2Datagridview();
                    var th2 = new Thread(publicmainform.AddValue2Datagridview);
                    th2.IsBackground = true;
                    th2.Start();
                    Thread.Sleep(100);
                    //Console.WriteLine("Main thread ({0}) exiting...",
                    //                  Thread.CurrentThread.ManagedThreadId);

                    publicmainform.dataGridView1.Update();
                    
                    //publicmainform.Connect_OPC();
                    var th3 = new Thread(publicmainform.Connect_OPC);
                    th3.IsBackground = true;
                    th3.Start();
                    Thread.Sleep(100);
                    //Console.WriteLine("Main thread ({0}) exiting...",
                    //                  Thread.CurrentThread.ManagedThreadId);
                                       
                    this.Close();
                }
//--------------------------------------------------------------------------------------------------------------------------------------------------------------                
            }
            else if (result == DialogResult.No)
            {
                //no...
            }
        }

        public void CancelAdd_Click(object sender, EventArgs e)
        {
            //AddForm formsetting = new AddForm();
            this.Close();
        }
    }
}
