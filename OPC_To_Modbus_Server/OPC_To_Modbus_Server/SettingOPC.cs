using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OPC_To_Modbus_Server;
using System.Data.OleDb;
using System.Net;
using System.IO;
using System.Web;

using Opc.Ua;
using Opc.UaFx;
using Opc.UaFx.Client;
using Opc.UaFx.Server;

namespace OPC_To_Modbus_Server
{
    public partial class SettingOPC : Form
    {
        DataTable Pub_dtTSetting = new DataTable();
        private readonly OPC_To_ModBus_Server publicmainform;
        OleDbConnection conTSetting = new OleDbConnection(@"Provider = Microsoft.ACE.OLEDB.12.0; Data Source = " + Directory.GetCurrentDirectory() + "\\TagMapping.mdb;Persist Security Info=False");

        public SettingOPC(OPC_To_ModBus_Server mainform)
        {
            InitializeComponent();
            publicmainform = mainform;
            GetTSetting();
        }

        private void GetTSetting()
        {
            String qsetting = "";          
            qsetting = "SELECT ID,ServerProgID,MB_UnitID,MB_Port,UpdateRate,LoggerInterval,LoggerFile FROM TSetting";
            //String conn_string = "Provider = Microsoft.ACE.OLEDB.12.0; Data Source = C:\\Users\\PorNB\\Documents\\Visual Studio 2015\\Projects1\\OPC_To_Modbus_Server\\OPC_To_Modbus_Server\\TagMapping.mdb;Persist Security Info=False";
            //OleDbConnection conn = new OleDbConnection(conTSetting);
            conTSetting.Open();

            OleDbCommand cmd = new OleDbCommand(qsetting, conTSetting);
            OleDbDataAdapter c = new OleDbDataAdapter(cmd);

            Pub_dtTSetting = new DataTable();
            c.SelectCommand = cmd;
            c.Fill(Pub_dtTSetting);
            conTSetting.Close();
            textBoxUpdateRate.Text = Pub_dtTSetting.Rows[0][4].ToString();           
            richTextBoxFilename.Text = Pub_dtTSetting.Rows[0][6].ToString();
        }

        private void SaveSetting_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Confirmation Save ?", "Confirm", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                string servername = listBox1.GetItemText(listBox1.SelectedItem);
                //String UpdateOPCTag = "UPDATE TTAGMapping SET OPC_TAGNAME = '" + row[1].ToString() + "', TagType = '" + row[3] + "', MB_ADDRESS = '" + row[2] + "' WHERE RegNo = " + row[0];
                String q = "UPDATE TSetting SET ServerProgID = '" + servername + "', Node = '" + Node.Text.ToString()+ "', UpdateRate = '" + textBoxUpdateRate.Text.ToString() + "' WHERE ID = 1 ";
                //Console.WriteLine(q);
                conTSetting.Open();
                OleDbCommand cmd = new OleDbCommand(q, conTSetting);
                cmd.ExecuteNonQuery();
                conTSetting.Close();

                Opc.Da.Item[] items = new Opc.Da.Item[publicmainform.Pub_dtTTAGMapping.Rows.Count];
                publicmainform.Get_TSetting();
                publicmainform.Connect_OPC();
//write null output from opcserver-------------------------------------------------------------------------------------                
                for(int rowgridview = 0; rowgridview < publicmainform.Pub_dtTTAGMapping.Rows.Count; rowgridview++)
                {
                    publicmainform.dataGridView1.Rows[rowgridview].Cells["TimeStamp"].Value = "";
                    publicmainform.dataGridView1.Rows[rowgridview].Cells["Quality"].Value = "";
                    publicmainform.dataGridView1.Rows[rowgridview].Cells["Value"].Value = "";
                }
                //---------------------------------------------------------------------------------------------------------------------

                publicmainform.AddValue2Datagridview();
                publicmainform.dataGridView1.Update();
                this.Close();
            }
            else if (result == DialogResult.No)
            {
                //no...
            }
        }

        private void CancelSetting_Click(object sender, EventArgs e)
        {
            //SettingOPC formsetting = new SettingOPC();
            this.Close();
        }

        //protected override IEnumerable<IOpcNode> CreateNodes(OpcNodeReferenceCollection references)
        //{
        //    OpcFolderNode machineOne = new OpcFolderNode(new OpcName("Machine_1"));
        //    //references.Add(machineOne, OpcObjectTypes.ObjectsFolder);
        //    return new IOpcNode[] { machineOne };
        //}

        private void Brown_OPC_Server_Click(object sender, EventArgs e)
        {
            try
            {
                listBox1.Items.Clear();

                //OpcFolderNode machineOne = new OpcFolderNode("localhost");
                //String ss = machineOne.Parent.ToString();
                //Console.WriteLine(ss);

                System.Net.NetworkCredential mCredentials = new System.Net.NetworkCredential();
                Opc.ConnectData mConnectData = new Opc.ConnectData(mCredentials);

                OpcCom.ServerEnumerator se = new OpcCom.ServerEnumerator();
                Opc.Server[] servers = se.GetAvailableServers(Opc.Specification.COM_DA_20, Node.Text.ToString(), mConnectData);
                for (int i = 0; i < servers.Length; i++)
                {
                    //Console.WriteLine("Server {0}: >{1}<", i, servers[i].Name);
                    listBox1.Items.Add(servers[i].Name);

                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
