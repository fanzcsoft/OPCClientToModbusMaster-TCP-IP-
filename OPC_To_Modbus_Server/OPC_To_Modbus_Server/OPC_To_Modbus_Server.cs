using System;
using System.Collections.Generic;
//using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Collections;
using System.Data.OleDb;
using System.Threading;
using System.IO;
using System.Net.Sockets; //check port open close
using System.Net; //start stop port
using System.Globalization;
//using System.Windows;
using System.Windows.Threading;
using System.Runtime;
using System.Runtime.InteropServices;

//nuget opc uafx advanced
using Opc.Ua;
using Opc.UaFx;

//from nmodbus 
using Modbus.Data;
using Modbus.Device;
using Modbus.Utility;
//using Modbus.Serial;


namespace OPC_To_Modbus_Server
{
    public partial class OPC_To_ModBus_Server : Form
    {
        OleDbConnection con = new OleDbConnection(@"Provider = Microsoft.ACE.OLEDB.12.0; Data Source = " + Directory.GetCurrentDirectory() + "\\TagMapping.mdb;Persist Security Info=False");

        DataTable Pub_dtTSetting = new DataTable();
        public DataTable Pub_dtTTAGMapping = new DataTable();
        int SelectedRowIndexdataGridView;
        Opc.Da.Subscription group;
        bool ShowMessage_Servername_Null = false;

        #region Variable for Log--

        bool write_log = false;
        static string dateTime = DateTime.Now.ToString();
        static string foldername = Convert.ToDateTime(dateTime).ToString("yyyy-MM-dd");
        static string filename = foldername;
        static string inlogFolderPath = Directory.GetCurrentDirectory() + "\\Log\\" + foldername;

        String AppendFilepath = (Path.Combine(inlogFolderPath, filename)) + ".txt";

        #endregion

        #region TcpListener TcpClient For Tomodbus,StartPort Function 

        //static TcpListener server = new TcpListener(IPAddress.Parse("127.0.0.1"), 502); //for start stop port
        static TcpListener server2Family = new TcpListener(IPAddress.Any, 502); //for start stop port

        TcpClient tcpClient = new TcpClient();
        static byte slaveId = 1;
        //public ModbusSlave slaveHost = ModbusTcpSlave.CreateTcp(slaveId, server);//must be outside funtion because this function can make memmory leak problem        
        public ModbusSlave slave2Family = ModbusTcpSlave.CreateTcp(slaveId, server2Family);//must be outside funtion because this function can make memmory leak problem        
        //ModbusIpMaster master = ModbusIpMaster.CreateIp(tcpClient); //master for 127.0.0.1

        #endregion

        string servercurrent_status = "";
        string servername = "";

        public OPC_To_ModBus_Server()
        {
            InitializeComponent();
            
            TextBox.CheckForIllegalCrossThreadCalls = false;
            Label.CheckForIllegalCrossThreadCalls = false;
            Button.CheckForIllegalCrossThreadCalls = false;
            timer2ReconnectOPC.Enabled = false;            
            Get_TSetting();
            //SetTcp();
            Checkport();
            InitialLogFile();
            Get_TagMapping();
            //Add EmptyRow To DataGridView      
            for (int roww = 0; roww < Pub_dtTTAGMapping.Rows.Count; roww++)
            {
                dataGridView1.Rows.Add();
            }
            //------------------------------
            AddValue2Datagridview();//Add 3Column(RegNo, Modbus_Adr, TagType) To  DataGridView           
            Connect_OPC();

            servername = Pub_dtTSetting.Rows[0][2].ToString();
            string check_server_status_firsttime = ServerStatusInTime.Text.ToString();
            if (servername != "" && check_server_status_firsttime != "Stop")
            {
                OPCDA_Read();
            }

            Console.WriteLine("main Form");

        }

        #region InitialLogFile

        public void InitialLogFile()
        {
            #region Create Directory--

            if (!Directory.Exists(inlogFolderPath))
            {
                System.IO.Directory.CreateDirectory(inlogFolderPath);
            }

            #endregion

            String q = "UPDATE TSetting SET LoggerFile = '" + inlogFolderPath + "' WHERE ID = 1";
            //Console.WriteLine(q);
            con.Open();
            OleDbCommand cmd = new OleDbCommand(q, con);
            cmd.ExecuteNonQuery();
            con.Close();            
            //--------------------------------------------------------------------------------------------

            #region//Create Log File------
            //Append in function ConnectOPC(),ReadCompleteCallback()
            if (!File.Exists(inlogFolderPath + "\\" + filename + ".txt"))
            {
                StreamWriter sw = File.CreateText(inlogFolderPath + "\\" + filename + ".txt");
                sw.Write("");
                sw.Close();
                //create file if file does not exist
            }
            #endregion//-----

        }

        #endregion

        #region Checkport

        public void Checkport()
        {
            int port = Convert.ToInt32(Pub_dtTSetting.Rows[0][4]); //<--- This is your value
            TcpListener server = new TcpListener(IPAddress.Parse("127.0.0.1"), 502); //for start stop port
            TcpClient tcpClient = new TcpClient();
            try
            {
                tcpClient.Connect("127.0.0.1", port);
                //Console.WriteLine("Port open");
                PortStatus.Text = "Running";
                tcpClient.Close();
                StartPort.Visible = false;
                StopPort.Visible = true;
            }
            catch (Exception)
            {
                //Console.WriteLine("1Port closed");
                PortStatus.Text = "Stop";
                StartPort.Visible = true;
                StopPort.Visible = false;
            }

        }

        #endregion

        #region Get_TSetting

        public void Get_TSetting()
        {
            //Console.WriteLine("Get_TSetting");
            String qsetting = "";
            qsetting = "SELECT ID,Node,ServerProgID,MB_UnitID,MB_Port,UpdateRate,LoggerInterval,LoggerFile FROM TSetting";
            //String conn_string = "Provider = Microsoft.ACE.OLEDB.12.0; Data Source = C:\\Users\\PorNB\\Documents\\Visual Studio 2015\\Projects1\\OPC_To_Modbus_Server\\OPC_To_Modbus_Server\\bin\\Debug\\TagMapping.mdb;Persist Security Info=False";
            //OleDbConnection con = new OleDbConnection(conn_string);
            con.Open();

            OleDbCommand cmd = new OleDbCommand(qsetting, con);
            OleDbDataAdapter c = new OleDbDataAdapter(cmd);
            
            Pub_dtTSetting = new DataTable();
            Pub_dtTSetting.Clear();
            c.SelectCommand = cmd;
            c.Fill(Pub_dtTSetting);

            c.Dispose(); //review
            cmd.Dispose();//review

            con.Close();
            textBoxServerName.Text = Pub_dtTSetting.Rows[0][2].ToString();
            textBoxModBus_Port.Text = Pub_dtTSetting.Rows[0][4].ToString();
            textBoxUnitID.Text = Pub_dtTSetting.Rows[0][3].ToString();
            timer2ReconnectOPC.Interval = (Convert.ToInt32(Pub_dtTSetting.Rows[0][5])) * 1000;
            timerModbus.Interval = (Convert.ToInt32(Pub_dtTSetting.Rows[0][5])) * 100;
            Invalidate(true);
            //con.Dispose();
        }
        #endregion

        #region Get_TagMapping

        public void Get_TagMapping()
        {
            //Console.WriteLine("Get_TagMapping");
            String q = "SELECT ID,RegNo,OPC_TAGNAME,MB_ADDRESS,TagType FROM TTAGMapping Order By TagType desc,MB_ADDRESS ";            

            con.Open();

            OleDbCommand cmd = new OleDbCommand(q, con);
            OleDbDataAdapter c = new OleDbDataAdapter(cmd);

            Pub_dtTTAGMapping = new DataTable();
            Pub_dtTTAGMapping.Clear();
            c.SelectCommand = cmd;
            c.Fill(Pub_dtTTAGMapping);

            con.Close();

            Invalidate(true);
            //-----------------------------------------------------------------------------------------
        }

        #endregion

        #region AddValue2Datagridview

        public void AddValue2Datagridview()
        {
            int row = 0;
            foreach (DataRow dtRow in Pub_dtTTAGMapping.Rows)
            {
                string RegNo = dtRow[1].ToString();
                string Tag_Name = dtRow[2].ToString();
                string OPC_TAGNAME = dtRow[2].ToString();
                string STR_MB_ADDRESS;
                int INTMB_ADDRESS;
                string TagType = dtRow[4].ToString();

                dataGridView1.Rows[row].Cells["RegNo"].Value = row + 1;

                if (TagType == "B")
                {
                    INTMB_ADDRESS = 100000 + Convert.ToInt32(dtRow[3]);
                    STR_MB_ADDRESS = INTMB_ADDRESS.ToString();
                }
                else
                {
                    INTMB_ADDRESS = 400000 + Convert.ToInt32(dtRow[3]);
                    STR_MB_ADDRESS = INTMB_ADDRESS.ToString();
                }
                dataGridView1.Rows[row].Cells["Modbus_Adr"].Value = STR_MB_ADDRESS;

                dataGridView1.Rows[row].Cells["TagType"].Value = TagType;
                dataGridView1.Rows[row].Cells["OPC_Tag"].Value = Tag_Name;
                dataGridView1.Rows[row].Cells["TimeStamp"].Value = "";
                if(TagType == "F")
                {
                    dataGridView1.Rows[row].Cells["Value"].Value = "-999";
                }
                else
                {
                    dataGridView1.Rows[row].Cells["Value"].Value = "0";
                }
                row = row + 1;

            }
            
        }

        #endregion

        #region Connect_OPC

        public void Connect_OPC()
        {
            //Console.WriteLine("Connect_OPC");

            // 1st: Create a server object and connect to the RSLinx OPC Server
            servername = Pub_dtTSetting.Rows[0][2].ToString();
            if (servername != "")
            {
                Opc.URL url = new Opc.URL("opcda://" + Pub_dtTSetting.Rows[0][1].ToString() + "/" + Pub_dtTSetting.Rows[0][2].ToString());
                Opc.Da.Server serveropc = null;
                OpcCom.Factory fact = new OpcCom.Factory();
                //Kepware.KEPServerEX.V6
                Opc.Da.ServerStatus serverStatus = new Opc.Da.ServerStatus();
                //serveropc = new Opc.Da.Server(fact, null);
                serveropc = new Opc.Da.Server(fact, url);
                System.Net.NetworkCredential mCredentials = new System.Net.NetworkCredential();
                Opc.ConnectData mConnectData = new Opc.ConnectData(mCredentials);

                try
                {
                    //timer2ReconnectOPC.Stop();                    
                    //2nd: Connect to the created server
                    serveropc.Connect(url, mConnectData);
#if DEBUG_ERROR
#warning   you must install RSLinx server OR Kepware.KEPServerEX.V6 for install important .dll then you can easy test with Kepware.KEPServerEX.V6
#endif
                    //serveropc.Connect(url, new Opc.ConnectData(new System.Net.NetworkCredential())); //ไม่เปิดเซิฟเวอ จะติดตรงนี้           
                    serverStatus = serveropc.GetStatus();
                    servercurrent_status = serverStatus.ServerState.ToString();
                    ServerStatusInTime.Text = serverStatus.ServerState.ToString();

                    //Append Log file if server Status running-------------------------------------------------
                    string CompareServerstatus = "running";
                    if (serverStatus.ServerState.ToString() == CompareServerstatus)
                    {
                        if (PortStatus.Text.ToString() == "Stop")
                        {
                            StartPort.Visible = true;
                        }
                        string timeinlog = Convert.ToDateTime(DateTime.Now.ToString()).ToString("yyyy-MM-dd HH:mm:ss= ");
                        if (write_log == false) //First Time Write Log
                        {
                            using (StreamWriter sw = File.AppendText(AppendFilepath))
                            {
                                sw.WriteLine(timeinlog + Pub_dtTSetting.Rows[0][2].ToString() + " DCOM security The operation completed successfully");
                                sw.WriteLine(timeinlog + "Connected to OPC server");
                                sw.WriteLine(timeinlog + "MyGroup Added group to server The operation completed successfully");
                            }
                        }
                        write_log = true; // 1 mean don't write agian use in ReadCompleteCallback                    

                    }

                    //----------------------------------------------------------------

                    //3rd Create a group if items   
                    Opc.Da.SubscriptionState groupState = new Opc.Da.SubscriptionState();
                    groupState.Name = "group";
                    groupState.Active = true;
                    group = (Opc.Da.Subscription)serveropc.CreateSubscription(groupState);

                    // add items to the group

                    Opc.Da.Item[] items = new Opc.Da.Item[Pub_dtTTAGMapping.Rows.Count];

                    //Add item by DataTable From Function Get_TagMapping 
                    for (int index_Tag = 0; index_Tag < Pub_dtTTAGMapping.Rows.Count; index_Tag++)
                    {

                        items[index_Tag] = new Opc.Da.Item();
                        //Tag_Name
                        items[index_Tag].ItemName = Pub_dtTTAGMapping.Rows[index_Tag][2].ToString();//Pub_dtTTAGMapping.Rows[Row][Column]
                    }
                    //timer2ReconnectOPC.Interval = (Convert.ToInt32(Pub_dtTSetting.Rows[0][5])) * 1000;
                    //timer2ReconnectOPC.Start();
                    //timerModbus.Interval = (Convert.ToInt32(Pub_dtTSetting.Rows[0][5])) * 300;
                    //timerModbus.Start();
                    timer2ReconnectOPC.Enabled = true;
                    items = group.AddItems(items);

                }//end try
                catch (Exception ex)
                {
                    servercurrent_status = "Stop";
                    //Exception Server Not Run
                    string timeinlog = Convert.ToDateTime(DateTime.Now.ToString()).ToString("yyyy-MM-dd HH:mm:ss= ");
                    write_log = false; // 1 mean don't write agian use in ReadCompleteCallback
                    ServerStatusInTime.Text = "Stop";
                    if (PortStatus.Text.ToString() == "Stop")
                    {
                        StartPort.Visible = false;
                    }

                    using (StreamWriter sw = File.AppendText(AppendFilepath))
                    {
                        //Pub_dtTSetting.Rows[0][1].ToString() => ServerName
                        sw.WriteLine(timeinlog + Pub_dtTSetting.Rows[0][2].ToString() + " DCOM security The operation completed successfully");
                        sw.WriteLine(timeinlog + "Unable to connect to OPC server");
                        sw.WriteLine(timeinlog + "MyGroup Unable to add group to server Unspecified error");
                        sw.WriteLine(timeinlog + "Service will be Reconnect To OPC Server With in 1 minutes");
                    }
                    MessageBox.Show("Error \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //timer2ReconnectOPC.Interval = (Convert.ToInt32(Pub_dtTSetting.Rows[0][5])) * 1000;
                    //timer2ReconnectOPC.Start();
                    timer2ReconnectOPC.Enabled = true;
                    //timer2ReconnectOPC.Enabled = true;
                }//end catch
            }//end if
            else
            {
                if (ShowMessage_Servername_Null == false)
                {   //Show 1 time
                    MessageBox.Show(" Servername NULL Show One Time When You Start Program \n Fix it in Tools > Setting > OPC > Brown OPC Server > Select Server > Save ", "ERROR Servername NULL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ShowMessage_Servername_Null = true;
                }
                
            }
                        
            //Invalidate(true);
        }

        #endregion

        #region OPCDA_Read

        public void OPCDA_Read()
        {
            //Declare Variable for request
            try
            {
                Opc.IRequest req;

                group.Read(group.Items, 1, new Opc.Da.ReadCompleteEventHandler(ReadCompleteCallback), out req);
            }
            catch(Exception ex)
            {
                InvokeUpdateControls(); //solve cross thread exception
            }
        }
        #endregion

        #region InvokeUpdate Stop
        public delegate void UpdateControlsDelegate();
        public void InvokeUpdateControls()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new UpdateControlsDelegate(UpdateControls));
            }
            else
            {
                UpdateControls();
            }
        }

        private void UpdateControls()
        {
            ServerStatusInTime.Text = "Stop"; //solve cross thread exception
            servercurrent_status = "Stop"; //solve cross thread exception
            StartPort.Visible = false; //solve cross thread exception
            // update your controls here
        }
        #endregion

        #region ReadCallBack

        public void ReadCompleteCallback(object clientHandle, Opc.Da.ItemValueResult[] results)
        {            

            for (int row = 0; row < results.Count(); row++)
            {
                string Result_TagItem = results[row].ItemName.ToString();
                string Result_Quality = results[row].Quality.QualityBits.ToString().ToUpper();
                string Result_TimeStamp = results[row].Timestamp.ToLocalTime().ToString();
                double doubleRoundValue = 0;

                
                for (int rowgridview = 0; rowgridview < dataGridView1.Rows.Count; rowgridview++)
                {
                    string OPC_tag_indatagridview = dataGridView1.Rows[rowgridview].Cells["OPC_Tag"].Value.ToString();
                    string TagType_indatagridview = dataGridView1.Rows[rowgridview].Cells["TagType"].Value.ToString();
                    if (OPC_tag_indatagridview == Result_TagItem)
                    {
                        if (results[row].Value != null)
                        {
                            //Correct Tag Good Quality                                                
                            doubleRoundValue = Math.Round(Convert.ToDouble(results[row].Value), 3);
                            dataGridView1.Rows[rowgridview].Cells["Value"].Value = doubleRoundValue.ToString();
                            dataGridView1.Rows[rowgridview].Cells["TimeStamp"].Value = Result_TimeStamp;
                            dataGridView1.Rows[rowgridview].Cells["OPC_Tag"].Value = Result_TagItem;
                            dataGridView1.Rows[rowgridview].Cells["Quality"].Value = Result_Quality;
                                                        
                        }//end if Correct Tag Good Quality
                        else if (results[row].Value == null)
                        {
                            //Correct Tag But Bad Quality
                            if (TagType_indatagridview == "F")
                            {
                                dataGridView1.Rows[rowgridview].Cells["Value"].Value = "-999";
                            }
                            else
                            {
                                dataGridView1.Rows[rowgridview].Cells["Value"].Value = "0";
                            }
                            dataGridView1.Rows[rowgridview].Cells["TimeStamp"].Value = Result_TimeStamp;
                            dataGridView1.Rows[rowgridview].Cells["OPC_Tag"].Value = Result_TagItem;
                            dataGridView1.Rows[rowgridview].Cells["Quality"].Value = Result_Quality;

                                                        
                        }//end else if Correct Tag But Bad Quality
                    }//end if Correct Tag
                    
                }
                
                //textBox4.Invoke(new EventHandler(delegate { textBox4.Text = (results[row].Value).ToString(); }));                
            }//end for loop 
            if (write_log == true)//1 mean can connect opc server then write 1 time
            {
                for (int log = 0; log < dataGridView1.Rows.Count; log++)
                {
                    string timeinlog = Convert.ToDateTime(DateTime.Now.ToString()).ToString("yyyy-MM-dd HH:mm:ss= ");
                    string OPC_tag_indatagridview = dataGridView1.Rows[log].Cells["OPC_Tag"].Value.ToString();
                    //good quality
                    if (dataGridView1.Rows[log].Cells["Value"].Value.ToString() != "-999" && dataGridView1.Rows[log].Cells["TimeStamp"].Value.ToString() != "")
                    {
                        //Write Log--------------------------------------------------------------------------------------------------------                        
                        
                            using (StreamWriter sw = File.AppendText(AppendFilepath))
                            {
                                sw.WriteLine(timeinlog + OPC_tag_indatagridview + " Added item " + log + " to group The operation completed successfully");
                            }
                        
                        //-----------------------------------------------------------------------------------------------------------------                                                              

                    }
                    //bad quality
                    else if (dataGridView1.Rows[log].Cells["TimeStamp"].Value.ToString() != "")
                    {
                        //Write Log--------------------------------------------------------------------------------------------------------
                        
                            using (StreamWriter sw = File.AppendText(AppendFilepath))
                            {
                                sw.WriteLine(timeinlog + OPC_tag_indatagridview + " Added item " + log + " Callback received for item but quality BAD The operation completed successfully Incorrect function");
                            }
                        

                        //-----------------------------------------------------------------------------------------------------------------

                    }
                    else if (dataGridView1.Rows[log].Cells["TimeStamp"].Value.ToString() == "")
                    {
                        //Write Log--------------------------------------------------------------------------------------------------------

                            using (StreamWriter sw = File.AppendText(AppendFilepath))
                            {
                                sw.WriteLine(timeinlog + OPC_tag_indatagridview + " Unable to add item " + log + " to group The item is no longer available in the server address space.");
                            }
                        
                        //-----------------------------------------------------------------------------------------------------------------

                    }//end else if InvalidTag
                }
            }               
            write_log = false; // ++ mean don't write agian
            
        }
        #endregion

        #region Add click

        private void Add_Click(object sender, EventArgs e)
        {
            AddForm form = new AddForm(this);
            form.Show();
            //Invalidate(true);            
        }

        #endregion

        #region edit_Click


        public void Edit_Click(object sender, EventArgs e)
        {       
            Save.Visible = true;
            Cancel.Visible = true;
            textBoxOPCTAG.Visible = true;
            label3.Visible = true;
            Add.Visible = false;
            Edit.Visible = false;
            Delete.Visible = false;
            textBoxOPCTAG.Text = dataGridView1.Rows[SelectedRowIndexdataGridView].Cells["OPC_Tag"].Value.ToString();
            dataGridView1.Enabled = false;
        }

        #endregion

        #region Save_Click

        private void Save_Click(object sender, EventArgs e)
        {
            dataGridView1.Enabled = true;
            //int rowIndex = dataGridView1.CurrentCell.RowIndex;
            string UPDATE_OPCTAG = textBoxOPCTAG.Text.ToString();//USE IN UPDATE SET
            string opctagselected = dataGridView1.Rows[SelectedRowIndexdataGridView].Cells["OPC_Tag"].Value.ToString();
            string q = "UPDATE TTAGMapping SET OPC_TAGNAME = '" + UPDATE_OPCTAG + "' WHERE OPC_TAGNAME = '" + opctagselected + "'  ";
            int checkduplicateTag = 0;
            for (int row = 0; row < Pub_dtTTAGMapping.Rows.Count; row++)
            {
                if(UPDATE_OPCTAG == opctagselected)
                {
                    MessageBox.Show("Error Tag_Name Duplicate", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    checkduplicateTag = 1;
                    break;
                }
            }                

            if(checkduplicateTag == 0)
            {
                dataGridView1.Rows[SelectedRowIndexdataGridView].Cells["OPC_Tag"].Value = UPDATE_OPCTAG;
                con.Open();
                OleDbCommand cmd = new OleDbCommand(q, con);
                cmd.ExecuteNonQuery();
                con.Close();

                //Get_TagMapping();
                var th = new Thread(Get_TagMapping);
                th.IsBackground = true;
                th.Start();
                Thread.Sleep(1000);
                //Console.WriteLine("Main thread ({0}) exiting...",
                //                  Thread.CurrentThread.ManagedThreadId);
                //Connect_OPC();
                var th2 = new Thread(Connect_OPC);
                th2.IsBackground = true;
                th2.Start();
                Thread.Sleep(1000);
                //Console.WriteLine("Main thread ({0}) exiting...",
                //                  Thread.CurrentThread.ManagedThreadId);
                th.Join();
                th2.Join();
                label3.Visible = false;
                textBoxOPCTAG.Visible = false;
                textBoxOPCTAG.Clear();
                dataGridView1.Update();
                Save.Visible = false;
                Cancel.Visible = false;
                Add.Visible = true;
                Edit.Visible = true;
                Delete.Visible = true;
            }          
            
        }

        #endregion

        #region Cancal_Click

        public void Cancel_Click(object sender, EventArgs e)
        {
            dataGridView1.Enabled = true;
            label3.Visible = false;
            textBoxOPCTAG.Visible = false;
            textBoxOPCTAG.Clear();
            Save.Visible = false;
            Cancel.Visible = false;
            Add.Visible = true;
            Edit.Visible = true;
            Delete.Visible = true;

        }

        #endregion

        #region Delete_Click

        private void Delete_Click(object sender, EventArgs e)
        {
            int rowIndex = dataGridView1.CurrentCell.RowIndex;

            String ModbusAdrSelected = dataGridView1.Rows[rowIndex].Cells["OPC_Tag"].Value.ToString();
            dataGridView1.Rows.RemoveAt(rowIndex);

            String q = "DELETE FROM TTAGMapping WHERE OPC_TAGNAME = '" + ModbusAdrSelected + "' ";
            Console.WriteLine(q);
            con.Open();
            OleDbCommand cmd = new OleDbCommand(q, con);
            cmd.ExecuteNonQuery();
            con.Close();


            //Get_TagMapping();
            var th = new Thread(Get_TagMapping);
            th.IsBackground = true;
            th.Start();
            Thread.Sleep(1000);
            //Console.WriteLine("Main thread ({0}) exiting...",
            //                  Thread.CurrentThread.ManagedThreadId);
            //AddValue2Datagridview();
            var th2 = new Thread(AddValue2Datagridview);
            th2.IsBackground = true;
            th2.Start();
            Thread.Sleep(100);
            
            dataGridView1.Update();
        }

        #endregion

        #region Exit

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are You Sure To Exit Programme ?", "Exit", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
            else if (result == DialogResult.No)
            {
                //no...
            }
        }

        #endregion

        #region OPCsetting

        private void oPCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SettingOPC formsetting = new SettingOPC(this);
            formsetting.Show();
            Invalidate(true);
        }

        #endregion

        #region ExportCSV

        private void exportCSVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Prepare ExportTable for Write Out--------------------------------------------------------------------------
            String q = "SELECT RegNo,OPC_TAGNAME,MB_ADDRESS,TagType FROM TTAGMapping Order By RegNo";
            con.Open();

            OleDbCommand cmd = new OleDbCommand(q, con);
            OleDbDataAdapter c = new OleDbDataAdapter(cmd);

            DataTable ExportTable = new DataTable();
            ExportTable.Clear();
            c.SelectCommand = cmd;
            c.Fill(ExportTable);
            con.Close();
            ExportTable.Columns.Add("Action", typeof(String));

            foreach (DataRow dr in ExportTable.Rows) // search whole table
            {
                dr["Action"] = "E"; //change the name
                                    //break; break or not depending on you            
            }
            //OpenSaveDialog Then Write CSV File-------------------------------------------------------------------
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "CSV file (*.csv)|*.csv| All Files (*.*)|*.*";
            if (save.ShowDialog() == DialogResult.OK)
            {
                StringBuilder sb = new StringBuilder();

                IEnumerable<string> columnNames = ExportTable.Columns.Cast<DataColumn>().
                                                  Select(column => column.ColumnName);
                sb.AppendLine(string.Join(",", columnNames));

                foreach (DataRow row in ExportTable.Rows)
                {
                    IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString());
                    sb.AppendLine(string.Join(",", fields));
                }
                File.WriteAllText(save.FileName + ".csv", sb.ToString());
            }
            ExportTable.Dispose();
        }

        #endregion

        #region Import CSV

        public void importCSVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            if (open.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //Console.WriteLine(open.FileName);//get path
                String path = open.FileName;
                string filenameWithoutExtension = Path.GetFileNameWithoutExtension(path);//get filenameWithout type
                                                                                         //Console.WriteLine(filenameWithoutExtension);

                DataTable importdatatableReadonly = new DataTable();
                importdatatableReadonly.Clear();
                DataTable importdatatable = new DataTable();
                importdatatable.Clear();

                using (StreamReader sr = new StreamReader(open.FileName))
                {
                    string[] headers = sr.ReadLine().Split(',');
                    foreach (string header in headers)
                    {
                        importdatatableReadonly.Columns.Add(header);
                        importdatatable.Columns.Add(header);
                    }
                    while (!sr.EndOfStream)
                    {
                        string[] rows = sr.ReadLine().Split(',');
                        DataRow drReadonly = importdatatableReadonly.NewRow();
                        DataRow dr = importdatatableReadonly.NewRow();
                        for (int i = 0; i < headers.Length; i++)
                        {
                            drReadonly[i] = rows[i];
                            
                        }
                        importdatatableReadonly.Rows.Add(drReadonly);
                        importdatatable.ImportRow(drReadonly);
                    }

                }
//Update Database--------------------------------------------------------------------------------------------------------------------------------------------------------
                con.Open();
                int RowIndexInFile = 0;

                foreach (DataRow row in importdatatableReadonly.Rows)
                {
                    String actionE = "E";
                    String actionA = "A";
                    String actionD = "D";
                    if (row[4].ToString() == actionE)
                    {
                        //Check Tag name-------And Modbus Address------------------------------------------------------------------------------------------
                        int checksametagname = 0;
                        int errorModbusAddress = 0;

                        int MBaddress_ImportAdd = Convert.ToInt32(importdatatableReadonly.Rows[RowIndexInFile].ItemArray[2]);

                        for (int RowimportOtherEdit_Or_OtherAdd = 0; RowimportOtherEdit_Or_OtherAdd < importdatatableReadonly.Rows.Count; RowimportOtherEdit_Or_OtherAdd++)
                        {

                            if ((RowimportOtherEdit_Or_OtherAdd != RowIndexInFile) && (importdatatableReadonly.Rows[RowimportOtherEdit_Or_OtherAdd].ItemArray[4].ToString() != actionD)) //Don't check myself & Action D
                            {
                                //check TagName
                                if (row[1].ToString() == importdatatableReadonly.Rows[RowimportOtherEdit_Or_OtherAdd].ItemArray[1].ToString())
                                {
                                    MessageBox.Show("Error Tag_Name same Add or Edit = " + importdatatableReadonly.Rows[RowimportOtherEdit_Or_OtherAdd].ItemArray[1].ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    checksametagname++;
                                    ++RowIndexInFile;
                                    break;
                                }
                                //check modbus is integer----------------------------------------------------------------------------
                                
                                try
                                {
                                    int mbvalueAddnew = Convert.ToInt32(importdatatableReadonly.Rows[RowimportOtherEdit_Or_OtherAdd].ItemArray[2]);
                                    if (mbvalueAddnew < 0 && mbvalueAddnew > 999)
                                    {
                                        MessageBox.Show("Error ModbusValue " + mbvalueAddnew, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        errorModbusAddress = 1;
                                        ++RowIndexInFile;

                                        break;
                                    }
                                    //check modbus_address--------------------------------------------------------------------------------------------------------------
                                    int ModbusimportEdit_Or_OtherAdd = Convert.ToInt32(importdatatableReadonly.Rows[RowimportOtherEdit_Or_OtherAdd].ItemArray[2]);
                                    string tagtype = importdatatableReadonly.Rows[RowimportOtherEdit_Or_OtherAdd].ItemArray[3].ToString();
                                    string tagtypeaddnew = row[3].ToString();
                                    if (tagtype == "F" && tagtypeaddnew == "F")
                                    {
                                        if (MBaddress_ImportAdd == ModbusimportEdit_Or_OtherAdd)
                                        {
                                            MessageBox.Show("Error ModbusAddress same Added Address = " + mbvalueAddnew, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            errorModbusAddress++;
                                            ++RowIndexInFile;
                                            break;
                                        }
                                    }
                                    else if (tagtype == "B" && tagtypeaddnew == "B")
                                    {
                                        if (MBaddress_ImportAdd == (ModbusimportEdit_Or_OtherAdd))
                                        {
                                            MessageBox.Show("Error ModbusAddress same Added = " + mbvalueAddnew, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            errorModbusAddress++;
                                            ++RowIndexInFile;
                                            break;
                                        }
                                    }

                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("ERROR\n" + ex, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }//end if don't check myself            
//-------------------------------------------------------------------------------------------------------------------------------
                        }//end For loop Check All Row Logic E
                        ++RowIndexInFile;
//Decide Update or not---------------------------------------------------------------------------------------------------------------------------------------
                        if (checksametagname == 0 && errorModbusAddress == 0)
                        {
                            String UpdateOPCTag = "UPDATE TTAGMapping SET OPC_TAGNAME = '" + row[1].ToString() + "', TagType = '" + row[3] + "', MB_ADDRESS = '" + row[2] + "' WHERE RegNo = " + row[0];

                            OleDbCommand cmd = new OleDbCommand(UpdateOPCTag, con);
                            cmd.ExecuteNonQuery();
                        }//end if Decide Update
                    }//end Action E                   
                    
//Delete--------------------------------------------------------------------------------------------------
                    else if (row[4].ToString() == actionD)
                    {

                        String DeleteOPCTag = "Delete FROM TTAGMapping WHERE OPC_TAGNAME = '" + row[1].ToString() + "' ";

                        OleDbCommand cmd = new OleDbCommand(DeleteOPCTag, con);
                        cmd.ExecuteNonQuery();
                        //dataGridView1.Rows.RemoveAt(RowIndexInFile);
                        importdatatable.Clear();

                        using (StreamReader sr = new StreamReader(open.FileName))
                        {
                            string[] headers = sr.ReadLine().Split(',');
                            foreach (string header in headers)
                            {
                                //importdatatable.Columns.Add(header);
                            }
                            while (!sr.EndOfStream)
                            {
                                string[] rows = sr.ReadLine().Split(',');
                                DataRow dr = importdatatable.NewRow();
                                for (int i = 0; i < headers.Length; i++)
                                {
                                    dr[i] = rows[i];
                                }
                                importdatatable.Rows.Add(dr);
                            }

                        }
                        dataGridView1.Rows.RemoveAt((Convert.ToInt32(dataGridView1.Rows.Count))-1);//delete last row in datagridview
                        ++RowIndexInFile;
                    }
//end Action D

//Logic Action A---------------------------------------------------------------------------------------------
                    else if (row[4].ToString() == actionA)
                    {
                        //Check Tag name-------And Modbus Address------------------------------------------------------------------------------------------
                        int checksametagname = 0;
                        int errorModbusAddress = 0;

                        int MBaddress_ImportAdd = Convert.ToInt32(importdatatableReadonly.Rows[RowIndexInFile].ItemArray[2]);

                        for (int RowimportEdit_Or_OtherAdd = 0; RowimportEdit_Or_OtherAdd < importdatatableReadonly.Rows.Count; RowimportEdit_Or_OtherAdd++)
                        {
                            
                            if ((RowimportEdit_Or_OtherAdd != RowIndexInFile) && (importdatatableReadonly.Rows[RowimportEdit_Or_OtherAdd].ItemArray[4].ToString() != actionD)) //Don't check myself & Action D
                            {
                                //check TagName
                                if (row[1].ToString() == importdatatableReadonly.Rows[RowimportEdit_Or_OtherAdd].ItemArray[1].ToString())
                                {
                                    MessageBox.Show("Error Tag_Name same Add or Edit = " + importdatatableReadonly.Rows[RowimportEdit_Or_OtherAdd].ItemArray[1].ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    checksametagname++;
                                    ++RowIndexInFile;
                                    break;
                                }
                                //check modbus is integer----------------------------------------------------------------------------
                                int mbvalueAddnew = Convert.ToInt32(importdatatableReadonly.Rows[RowimportEdit_Or_OtherAdd].ItemArray[2]);
                                try
                                {
                                    if (mbvalueAddnew < 0 && mbvalueAddnew > 999)
                                    {
                                        MessageBox.Show("Error ModbusValue " + mbvalueAddnew, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        errorModbusAddress = 1;
                                        ++RowIndexInFile;

                                        break;
                                    }
                                    //check modbus_address--------------------------------------------------------------------------------------------------------------
                                    int ModbusimportEdit_Or_OtherAdd = Convert.ToInt32(importdatatableReadonly.Rows[RowimportEdit_Or_OtherAdd].ItemArray[2]);
                                    string tagtype = importdatatableReadonly.Rows[RowimportEdit_Or_OtherAdd].ItemArray[3].ToString();
                                    string tagtypeaddnew = row[3].ToString();
                                    if (tagtype == "F" && tagtypeaddnew == "F")
                                    {
                                        if (MBaddress_ImportAdd == ModbusimportEdit_Or_OtherAdd)
                                        {
                                            MessageBox.Show("Error ModbusAddress same Added Address = " + mbvalueAddnew, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            errorModbusAddress++;
                                            ++RowIndexInFile;
                                            break;
                                        }
                                    }
                                    else if (tagtype == "B" && tagtypeaddnew == "B")
                                    {
                                        if (MBaddress_ImportAdd == (ModbusimportEdit_Or_OtherAdd))
                                        {
                                            MessageBox.Show("Error ModbusAddress same Added = " + mbvalueAddnew, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            errorModbusAddress++;
                                            ++RowIndexInFile;
                                            break;
                                        }
                                    }

                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("ERROR\n" + ex, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }                  
                            }//end if don't check myself              
                            //-------------------------------------------------------------------------------------------------------------------------------
                        }//end For loop Check All Row Logic A
                        ++RowIndexInFile;
//Decide insert or not---------------------------------------------------------------------------------------------------------------------------------------
                        if (checksametagname == 0 && errorModbusAddress == 0)
                        {
                            //insert
                            TagMappingDataSetTableAdapters.TTAGMappingTableAdapter regionTableAdapter =
                           new TagMappingDataSetTableAdapters.TTAGMappingTableAdapter();
                            regionTableAdapter.Insert(Convert.ToInt32(row[0]), row[1].ToString(), 0, "", Convert.ToInt32(row[2]), 0, row[3].ToString());
                            dataGridView1.Rows.Add();
                        }
                    }//end Action A
                }
                //end for each
                con.Close();


                //Get_TagMapping();
                var th = new Thread(Get_TagMapping);
                th.IsBackground = true;
                th.Start();
                Thread.Sleep(100);

                dataGridView1.Update();
                //Console.WriteLine("Main thread ({0}) exiting...",
                //                  Thread.CurrentThread.ManagedThreadId);
                //AddValue2Datagridview();
                var th2 = new Thread(AddValue2Datagridview);
                th2.IsBackground = true;
                th2.Start();
                Thread.Sleep(100);
                //Console.WriteLine("Main thread ({0}) exiting...",
                //                  Thread.CurrentThread.ManagedThreadId);
                //Connect_OPC();
                var th3 = new Thread(Connect_OPC);
                th3.IsBackground = true;
                th3.Start();
                Thread.Sleep(100);
                //Console.WriteLine("Main thread ({0}) exiting...",
                //                  Thread.CurrentThread.ManagedThreadId);
                
            }
        }

        #endregion

        #region ResetConnection

        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Thread threadReset = new Thread(Connect_OPC);
            threadReset.IsBackground = true;
            threadReset.Start();
            Thread.Sleep(1000);
            threadReset.Join();
            //Connect_OPC();
        }

        #endregion

        #region Tomodbus
        DataStore datastore = DataStoreFactory.CreateDefaultDataStore();
        public void Tomodbus()
        {
            int port = Convert.ToInt32(Pub_dtTSetting.Rows[0][4]); //<--- This is your value
                                    
            timerModbus.Stop();
            try
            {
                IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        TcpClient tcpClient = new TcpClient();
                        tcpClient.Connect(ip.ToString(), port); //if port 502 open, server not start throw exception
                        ModbusIpMaster master2Family = ModbusIpMaster.CreateIp(tcpClient);

                        int Count_Boolean = 0;
                        bool[] boolvalueto_inputRegister = new bool[dataGridView1.Rows.Count];
                        slave2Family.DataStore.InputDiscretes.Clear(); // clear Input Status for manual add
                        for (int row = 0; row < Pub_dtTTAGMapping.Rows.Count; row++)
                        {                                                         
                            string Mb_Address = dataGridView1.Rows[row].Cells[2].Value.ToString();//MB address
                            string Value = dataGridView1.Rows[row].Cells[3].Value.ToString();//Value
                            string tagtype = dataGridView1.Rows[row].Cells[6].Value.ToString();//TagType

                            if (tagtype == "F")
                            {
                                string ValueString = dataGridView1.Rows[row].Cells[3].Value.ToString();//Value

                                //Convert float to Hex---------------------------------------------------------------------------------
                                float strTofloat;
                                bool result = float.TryParse(ValueString, out strTofloat);
                                //Console.WriteLine("parseinput : " + strTofloat);
                                if (result == true)
                                {
                                    byte[] buffer = BitConverter.GetBytes(strTofloat);
                                    int intVal = BitConverter.ToInt32(buffer, 0);//if don't have this line then program do like string to hex
                                                                                 //https://gregstoll.com/~gregstoll/floattohex/
                                                                                 //http://string-functions.com/string-hex.aspx
                                    string hexstring = intVal.ToString("X8");

                                    //Console.WriteLine("hex : " + hexstring);
                                    //-----------------------------------------------------------------------------------------------------                    
                                    //float sample = float.Parse("41.273", CultureInfo.InvariantCulture.NumberFormat);
                                    //CultureInfo ci = new CultureInfo("en-us"); //use with underline
                                    //string value_S = value_Int.ToString("D8", ci); //format input 1234 to output 00001234

        //separate hex string-------------------------------------------------------------------------------------
                                    string hexstrinng_left = hexstring.Substring(0, 4);
                                    ushort hexshort_left = Convert.ToUInt16(hexstrinng_left, 16); //like under line
                                    //ushort hexshort_left = ushort.Parse(hexstrinng_left, System.Globalization.NumberStyles.AllowHexSpecifier);
                                    //Console.WriteLine("left : " + hexshort_left);

                                    string hexstrinng_right = hexstring.Substring(4);
                                    ushort hexshort_right = Convert.ToUInt16(hexstrinng_right, 16);

                                    //Console.WriteLine("R : " + hexshort_right);
        //---------------------------------------------------------------------------------------------------------

        //Prepare MbAddress to send---------------------------------------------------------------------------------                            

                                    int int32Mb_Address = Convert.ToInt32(Mb_Address) - 400000 - 1;//-1 cause modscan32 start index 0
                                    ushort RightShortMbAddress = Convert.ToUInt16(int32Mb_Address);
                                    //ushort RightShortMbAddress = (ushort)int32Mb_Address;

                                    int LeftMbAddress = int32Mb_Address + 1;
                                    ushort LeftShortMbAddress = Convert.ToUInt16(LeftMbAddress);
                                    //ushort LeftShortMbAddress = (ushort)LeftMbAddress;
                                    //---------------------------------------------------------------------------------------------------------------
                                    master2Family.WriteSingleRegister(RightShortMbAddress, hexshort_right);
                                    master2Family.WriteSingleRegister(LeftShortMbAddress, hexshort_left);
                                }//end if result

                            }//end if Tagtype F
                            else if (tagtype == "B")
                            {

                                bool boolvalue = Convert.ToBoolean(Convert.ToInt16(Value));
                                int int32Mb_Address = Convert.ToInt32(Mb_Address) - 100000 - 1;//-1 cause modscan32 start index 0


                                master2Family.WriteSingleCoil(Convert.ToUInt16(int32Mb_Address), boolvalue);

                                boolvalueto_inputRegister[Count_Boolean] = boolvalue;
                                ++Count_Boolean;
                                //lock (slave.DataStore.CoilDiscretes){                       
                                //slave.DataStore.InputDiscretes.Clear();
                                //for (int Address_input_status = 0; Address_input_status < dataGridView1.Rows.Count; Address_input_status++)
                                //{
                                if (boolvalue == true)
                                {
                                    //slave.DataStore.InputDiscretes.Add(true);
                                    slave2Family.DataStore.InputDiscretes.Add(true);
                                }
                                else
                                {
                                    //slave.DataStore.InputDiscretes.Add(false);
                                    slave2Family.DataStore.InputDiscretes.Add(false);
                                }
                                //}
                                //}//end lock
                            }//end else if Tagtype B

                        }//end for loop  
                    }
                }//end foreach
            }
            catch(ArgumentOutOfRangeException e)
            {
                MessageBox.Show("ERROR WriteToModbus \n" + e, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // Log exception              
            }
            finally
            {
                //tcpClient.Close();  //close like dispose it's not good function
                //timer2ReconnectOPC.Enabled = true;
                timerModbus.Interval = (Convert.ToInt32(Pub_dtTSetting.Rows[0][5])) * 100;
                timerModbus.Start();
            }

        }

        #endregion

        #region StartPort_Click

        bool is_Portrunning = false;

        public void StartPort_Click(object sender, EventArgs e)
        {
            int port = Convert.ToInt32(Pub_dtTSetting.Rows[0][4]); //<--- This is your value
            PortStatus.Visible = true;
            byte slaveId = Convert.ToByte(Pub_dtTSetting.Rows[0][3]);//row1 column4

            try
            {
                //Family IP
                IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        Console.WriteLine(ip.ToString());
                        //server2Family = new TcpListener(IPAddress.Parse(ip.ToString()), port); //for start stop port
                        server2Family.Start();
                 
                        slave2Family = ModbusTcpSlave.CreateTcp(slaveId, server2Family);//must be outside loop funtion because this function can make memmory leak problem
                        slave2Family.Listen(); //Any IP 0.0.0.0:502

                        tcpClient.Connect(ip.ToString(), port); //if port 502 open, server not start throw exception
                        Tomodbus();

                        break;
                    }
                    
                }//End Family IP

                //tcpClient.Connect("127.0.0.1", port); //if port 502 open, server not start throw exception
                //Console.WriteLine("Port open");               
                PortStatus.Text = "Running";
                is_Portrunning = true ;
                StartPort.Visible = false;
                StopPort.Visible = true;
                //slaveHost.Listen(); //IP 127.0.0.1
                //Tomodbus();

                Invalidate(true);
            }
            catch (Exception)
            {                
                PortStatus.Text = "Running Before Start";
                MessageBox.Show("Can't Start Port because Port Running", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                StartPort.Visible = false;
                StopPort.Visible = true;
                Invalidate(true);
            }
        }

        #endregion

        #region StopPort_Click Have bug Don't Finish

        public void StopPort_Click(object sender, EventArgs e)
        {                                
            try
            {
                TcpListener server = new TcpListener(IPAddress.Any, 502); //for start stop port
                IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        tcpClient = new TcpClient(ip.ToString(),502);
                        tcpClient.Close();
                    }
                }
                server.Stop();                             
                PortStatus.Text = "Stop in 4 min " + Convert.ToDateTime(DateTime.Now.ToString()).ToString("HH:mm:ss");
                StartPort.Visible = true;
                StopPort.Visible = false;
                MessageBox.Show("Wait 4 min For Start Again");
            }
            catch (Exception ex)
            {               
                                              
            }            
        }

        #endregion


        #region timerRoconnectOPC

        private void timer2ReconnectOPC_Tick(object sender, EventArgs e)
        {
            //try to connect server every 40sec
            //if server down reconnect automatic
            //use thread window form not stuck
            if(servercurrent_status == "Stop")
            {
                timerModbus.Stop();
                var th3 = new Thread(Connect_OPC);
                th3.IsBackground = false;
                if (th3.ThreadState != ThreadState.Running)
                {
                    th3.Start();
                }         
                Thread.Sleep(1000);
                th3.Join();         
            }                                                                                       
            else if (servercurrent_status == "running")
            {
                OPCDA_Read();

            }
        }//end timer2Reconnect

        #endregion

        private void OPC_To_ModBus_Server_Load(object sender, EventArgs e)
        {
            this.Visible = true;
        }

        private void timerModbus_Tick(object sender, EventArgs e)
        {
            if (is_Portrunning == true)
            {
                int port = Convert.ToInt32(Pub_dtTSetting.Rows[0][4]); //<--- This is your value
                IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        Console.WriteLine(ip.ToString());
                        Tomodbus();
                    }
                }//End Family IP
                //tcpClient.Connect("127.0.0.1", port); //if port 502 open, server not start throw exception
                //Tomodbus();
            }
        }

        #region mouseclick on datagridview

        private void dataGridView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                SelectedRowIndexdataGridView = dataGridView1.HitTest(e.X, e.Y).RowIndex;
            }
            else
            {

                ContextMenuStrip my_menu = new ContextMenuStrip();
                SelectedRowIndexdataGridView = dataGridView1.HitTest(e.X, e.Y).RowIndex;
                dataGridView1.ClearSelection();
                dataGridView1.Rows[SelectedRowIndexdataGridView].Selected = true;


                if (SelectedRowIndexdataGridView >= 0)
                {
                    my_menu.Items.Add("Edit").Name = "Edit";
                    my_menu.Items.Add("Delete").Name = "Delete";
                }
                my_menu.Show(dataGridView1, new Point(e.X, e.Y));

                //Event menu Click
                my_menu.ItemClicked += new ToolStripItemClickedEventHandler(my_menu_ItemClicked);
            }
        }

        private void my_menu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.Name.ToString())
            {
                case "Delete":
                    //Console.WriteLine("Delete case");
                    try
                    {
                        String OPCSelected = dataGridView1.Rows[SelectedRowIndexdataGridView].Cells["OPC_Tag"].Value.ToString();
                        dataGridView1.Rows.RemoveAt(SelectedRowIndexdataGridView);

                        String q = "DELETE FROM TTAGMapping WHERE OPC_TAGNAME = '" + OPCSelected + "' ";
                        Console.WriteLine(q);
                        con.Open();
                        OleDbCommand cmd = new OleDbCommand(q, con);
                        cmd.ExecuteNonQuery();
                        con.Close();


                        //Get_TagMapping();
                        var th = new Thread(Get_TagMapping);
                        th.IsBackground = true;
                        th.Start();
                        Thread.Sleep(1000);
                        //Console.WriteLine("Main thread ({0}) exiting...",
                        //                  Thread.CurrentThread.ManagedThreadId);
                        //AddValue2Datagridview();
                        var th2 = new Thread(AddValue2Datagridview);
                        th2.IsBackground = true;
                        th2.Start();
                        Thread.Sleep(100);

                        dataGridView1.Update();
                    
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error \n" + ex, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;
                case "Edit":
                    //Do Something            

                    try
                    {
                        Save.Visible = true;
                        Cancel.Visible = true;
                        textBoxOPCTAG.Visible = true;
                        label3.Visible = true;
                        Add.Visible = false;
                        Edit.Visible = false;
                        Delete.Visible = false;
                        textBoxOPCTAG.Text = dataGridView1.Rows[SelectedRowIndexdataGridView].Cells["OPC_Tag"].Value.ToString();
                        dataGridView1.Enabled = false;

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error \n" + ex, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;
            }
        }

#endregion
    }
}
