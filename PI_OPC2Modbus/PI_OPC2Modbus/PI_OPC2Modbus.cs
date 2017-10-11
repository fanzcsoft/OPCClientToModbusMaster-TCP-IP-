using System.ServiceProcess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Data.OleDb;
using System.IO;
using System.Net.Sockets; //check port open close
using System.Net; //start stop port
using System.Windows;

//nuget opc uafx advanced
using Opc.Ua;
using Opc.UaFx;

//from nmodbus4
using Modbus.Data;
using Modbus.Device;
using Modbus.Utility;

namespace PI_OPC2Modbus
{
    public partial class PI_OPC2Modbus : ServiceBase
    {
        //OleDbConnection con = new OleDbConnection(@"Provider = Microsoft.ACE.OLEDB.12.0; Data Source = " + Directory.GetCurrentDirectory() + "\\TagMapping.mdb;Persist Security Info=False");
        OleDbConnection con = new OleDbConnection(@"Provider = Microsoft.ACE.OLEDB.12.0; Data Source = " + AppDomain.CurrentDomain.BaseDirectory + "\\TagMapping.mdb;Persist Security Info=False");

        DataTable Pub_dtTSetting = new DataTable();
        public DataTable Pub_dtTTAGMapping = new DataTable();

        Opc.Da.Subscription group;

        bool is_Portrunning = false;        

        #region Variable for Log--

        bool write_log = false;
        static string dateTime = DateTime.Now.ToString();
        static string foldername = Convert.ToDateTime(dateTime).ToString("yyyy-MM-dd");
        static string filename = foldername;
        static string inlogFolderPath = AppDomain.CurrentDomain.BaseDirectory + "\\Log\\" + foldername;
        static string LogFolder = AppDomain.CurrentDomain.BaseDirectory + "\\Log";
        static string indebuglogFolderPath = AppDomain.CurrentDomain.BaseDirectory + "\\DebugLog\\" + foldername;
        String AppendFilepath = (Path.Combine(inlogFolderPath, filename)) + ".txt";


        #endregion

        #region TcpListener TcpClient For Tomodbus,StartPort Function 

        static TcpListener server2Family = new TcpListener(IPAddress.Any, 502); //for start stop port
        //static TcpClient tcpClient = new TcpClient();
        static byte slaveId = 1;
        public ModbusSlave slave2Family = ModbusTcpSlave.CreateTcp(slaveId, server2Family);//must be outside funtion because this function can make memmory leak problem
        //ModbusIpMaster master2Family = ModbusIpMaster.CreateIp(tcpClient);
        IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
        #endregion

        string servercurrent_status = "";
        string servername = "";
        

        private System.Timers.Timer timerModbus; 

        public PI_OPC2Modbus()
        {
            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\DebugLog\\"))
            {
                if (!Directory.Exists(indebuglogFolderPath))
                {
                    Directory.CreateDirectory(indebuglogFolderPath);
                }
            }
            
            InitializeComponent();        
        }

        #region InitialLogFile

        public void InitialLogFile()
        {            
            try
            {
                #region Create Directory--
                if (!Directory.Exists(LogFolder))
                {
                    System.IO.Directory.CreateDirectory(LogFolder);
                }
                
                if (!Directory.Exists(inlogFolderPath))
                {
                    System.IO.Directory.CreateDirectory(inlogFolderPath);
                }
                if (!Directory.Exists(indebuglogFolderPath))
                {
                    System.IO.Directory.CreateDirectory(indebuglogFolderPath);
                }
                String q = "UPDATE TSetting SET LoggerFile = '" + inlogFolderPath + "' WHERE ID = 1";
                //Console.WriteLine(q);
                con.Open();
                OleDbCommand cmd = new OleDbCommand(q, con);
                cmd.ExecuteNonQuery();
                con.Close();
                //--------------------------------------------------------------------------------------------
                #endregion

                #region//Create Log File------
                //Append in function ConnectOPC(),ReadCompleteCallback()
                if (!File.Exists(inlogFolderPath + "\\" + filename + ".txt"))
                {
                    StreamWriter sw = File.CreateText(inlogFolderPath + "\\" + filename + ".txt");
                    sw.Write("");
                    sw.Close();
                    //create file if file does not exist
                }
                if (!File.Exists(indebuglogFolderPath + "\\" + filename + ".txt"))
                {
                    StreamWriter sw = File.CreateText(indebuglogFolderPath + "\\" + filename + ".txt");
                    sw.Write("");
                    sw.Close();
                    //create file if file does not exist
                }
                #endregion//-----

            }
            catch (Exception ex)
            {
                using (StreamWriter sw = File.AppendText(indebuglogFolderPath + "\\" + filename + ".txt"))
                {
                    sw.WriteLine("ERROR : " + ex);
                }
            }                                          

        }

        #endregion

        #region Get_TSetting

        public void Get_TSetting()
        {
            try
            {
                //Console.WriteLine("Get_TSetting");
                String qsetting = "";
                qsetting = "SELECT ID,Node,ServerProgID,MB_UnitID,MB_Port,UpdateRate,LoggerInterval,LoggerFile FROM TSetting";

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

                timerModbus.Interval = (Convert.ToInt32(Pub_dtTSetting.Rows[0][5])) * 1000;
            }
            catch(Exception ex)
            {
                using (StreamWriter sw = File.AppendText(indebuglogFolderPath + "\\" + filename + ".txt"))
                {
                    sw.WriteLine("ERROR : " + ex);
                }
            }            

        }
        #endregion

        #region Get_TagMapping

        public void Get_TagMapping()
        {
            try
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

                //Invalidate(true);
                //-----------------------------------------------------------------------------------------
            }
            catch (Exception ex)
            {
                using (StreamWriter sw = File.AppendText(indebuglogFolderPath + "\\" + filename + ".txt"))
                {
                    sw.WriteLine("ERROR : " + ex);
                }
            }
            
        }

        #endregion

        #region AddValue2Datagridview

        public void AddValue2Datagridview()
        {
            try
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
                    dataGridView1.Rows[row].Cells["Quality"].Value = "";
                    if (TagType == "F")
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
            catch(Exception ex)
            {
                using (StreamWriter sw = File.AppendText(indebuglogFolderPath + "\\" + filename + ".txt"))
                {
                    sw.WriteLine("ERROR : " + ex);
                }
            }
            
        }

        #endregion

        #region Connect_OPC

        public void Connect_OPC()
        {
            //Console.WriteLine("Connect_OPC");
            try
            {
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
                                //StartPort.Visible = true;
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
                        items = group.AddItems(items);
                        this.timerModbus.Interval = (Convert.ToInt32(Pub_dtTSetting.Rows[0][5]) * 100);
                        this.timerModbus.AutoReset = false;
                        this.timerModbus.Start();

                    }//end try
                    catch (Exception)
                    {
                        servercurrent_status = "Stop";
                        //Exception Server Not Run
                        string timeinlog = Convert.ToDateTime(DateTime.Now.ToString()).ToString("yyyy-MM-dd HH:mm:ss= ");
                        write_log = false; // 1 mean don't write agian use in ReadCompleteCallback
                        ServerStatusInTime.Text = "Stop";

                        using (StreamWriter sw = File.AppendText(AppendFilepath))
                        {
                            //Pub_dtTSetting.Rows[0][2].ToString() => ServerName
                            sw.WriteLine(timeinlog + Pub_dtTSetting.Rows[0][2].ToString() + " DCOM security The operation completed successfully");
                            sw.WriteLine(timeinlog + "Unable to connect to OPC server");
                            sw.WriteLine(timeinlog + "MyGroup Unable to add group to server Unspecified error");
                            sw.WriteLine(timeinlog + "Service will be Reconnect To OPC Server With in 1 minutes");
                        }
                        this.timerModbus.Interval = (Convert.ToInt32(Pub_dtTSetting.Rows[0][5])) * 1000;
                        this.timerModbus.AutoReset = false;
                        this.timerModbus.Start();
                    }//end catch
                }//end if
            }
            catch(Exception ex)
            {
                using (StreamWriter sw = File.AppendText(indebuglogFolderPath + "\\" + filename + ".txt"))
                {
                    sw.WriteLine("ERROR : " + ex);
                }
            }
            // 1st: Create a server object and connect to the RSLinx OPC Server            
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
            catch (Exception ex)
            {
                ServerStatusInTime.Text = "Stop"; //solve cross thread exception
                servercurrent_status = "Stop"; //solve cross thread exception
                //StartPort.Visible = false; //solve cross thread exception
                //InvokeUpdateControls(); //solve cross thread exception
                using (StreamWriter sw = File.AppendText(indebuglogFolderPath + "\\" + filename + ".txt"))
                {
                    sw.WriteLine("ERROR : " + ex);
                }
            }
        }

        #endregion

        #region ReadCallBack

        public void ReadCompleteCallback(object clientHandle, Opc.Da.ItemValueResult[] results)
        {
            for (int rowgridview = 0; rowgridview < Pub_dtTTAGMapping.Rows.Count; rowgridview++)
            {
                dataGridView1.Rows[rowgridview].Cells["TimeStamp"].Value = "";
                dataGridView1.Rows[rowgridview].Cells["Quality"].Value = "";
            }                

            for (int row = 0; row < results.Count(); row++)
            {
                string Result_TagItem = results[row].ItemName.ToString();
                string Result_Quality = results[row].Quality.QualityBits.ToString().ToUpper();
                string Result_TimeStamp = results[row].Timestamp.ToLocalTime().ToString();
                double doubleRoundValue = 0;


                for (int rowgridview = 0; rowgridview < Pub_dtTTAGMapping.Rows.Count; rowgridview++)
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
                for (int log = 0; log < Pub_dtTTAGMapping.Rows.Count; log++)
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

        #region Tomodbus
        DataStore datastore = DataStoreFactory.CreateDefaultDataStore();
        public void Tomodbus()
        {            
            this.timerModbus.Stop();
            try
            {
                int port = Convert.ToInt32(Pub_dtTSetting.Rows[0][4]); //<--- This is your value
                // define array to keep address Input Register---------------------------------------
                int tagB = 0;
                for (int row = 0; row < Pub_dtTTAGMapping.Rows.Count; row++)
                {
                    string tagtype = dataGridView1.Rows[row].Cells[6].Value.ToString();//TagType
                    if (tagtype == "B")
                    {
                        tagB++;
                    }
                }
                int[][] mosbusAdrInputRegister = new int[tagB][];
                for (int index1 = 0; index1 < tagB; index1++)
                {
                    mosbusAdrInputRegister[index1] = new int[2];
                }
                //-----------------------------------------------------------------------------------
                //IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        using(TcpClient tcpClient = new TcpClient())
                        {
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

                                    if (result == true)
                                    {
                                        byte[] buffer = BitConverter.GetBytes(strTofloat);
                                        int intVal = BitConverter.ToInt32(buffer, 0);//if don't have this line then program do like string to hex
                                                                                     //https://gregstoll.com/~gregstoll/floattohex/
                                                                                     //http://string-functions.com/string-hex.aspx
                                        string hexstring = intVal.ToString("X8");

                                        //separate hex string-------------------------------------------------------------------------------------
                                        string hexstrinng_left = hexstring.Substring(0, 4);
                                        ushort hexshort_left = Convert.ToUInt16(hexstrinng_left, 16); //like under line

                                        string hexstrinng_right = hexstring.Substring(4);
                                        ushort hexshort_right = Convert.ToUInt16(hexstrinng_right, 16);
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

                                    mosbusAdrInputRegister[Count_Boolean][0] = int32Mb_Address;
                                    mosbusAdrInputRegister[Count_Boolean][1] = Convert.ToInt32(Value);
                                    ++Count_Boolean;
                                    //if (boolvalue == true)
                                    //{
                                    //    //slave.DataStore.InputDiscretes.Add(true);
                                    //    slave2Family.DataStore.InputDiscretes.Add(false);
                                    //}
                                    //else
                                    //{
                                    //    //slave.DataStore.InputDiscretes.Add(false);
                                    //    slave2Family.DataStore.InputDiscretes.Add(true);
                                    //}

                                }//end else if Tagtype B

                            }//end for loop   
                            int getlastaddress = mosbusAdrInputRegister[mosbusAdrInputRegister.Length - 1][0];
                            int matchIndexInputregister = 0;
                            for (int indexInputRegister = 0; indexInputRegister <= getlastaddress; indexInputRegister++)
                            {
                                if (indexInputRegister == mosbusAdrInputRegister[matchIndexInputregister][0])
                                {
                                    if (mosbusAdrInputRegister[matchIndexInputregister][1] == 1)
                                    {
                                        //slave.DataStore.InputDiscretes.Add(true);
                                        slave2Family.DataStore.InputDiscretes.Add(true);
                                    }
                                    else
                                    {
                                        //slave.DataStore.InputDiscretes.Add(false);
                                        slave2Family.DataStore.InputDiscretes.Add(false);
                                    }
                                    matchIndexInputregister++;
                                }
                                else
                                {
                                    slave2Family.DataStore.InputDiscretes.Add(false);
                                }
                            }
                            datastore.InputRegisters.Clear();
                        }
                                                                                             
                    }//end if ipv4
                }//end foreach
                
            }
            catch (Exception ex)
            {
                using (StreamWriter sw = File.AppendText(indebuglogFolderPath + "\\" + filename + ".txt"))
                {
                    sw.WriteLine("ERROR : " + ex);
                }
            }

        }

        #endregion

        public void OnDebug()
        {
            OnStart(null);
        }

        void InitializeAllStart()
        {
            try
            {
                this.timerModbus = new System.Timers.Timer();  // 30000 milliseconds = 30 seconds
                this.timerModbus.Elapsed += new System.Timers.ElapsedEventHandler(this.timerModbus_Elapsed);
                this.timerModbus.AutoReset = false; // prevent race condition
                Get_TSetting();
                InitialLogFile();
                Get_TagMapping();
                //Add EmptyRow To DataGridView
                dataGridView1.Rows.Clear();
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
            }
            catch(Exception ex)
            {
                using (StreamWriter sw = File.AppendText(indebuglogFolderPath + "\\" + filename + ".txt"))
                {
                    sw.WriteLine("ERROR : " + ex);
                }
            }
            
        }

        void ServerStart()
        {
            try
            {
                int port = Convert.ToInt32(Pub_dtTSetting.Rows[0][4]); //<--- This is your value
                byte slaveId = Convert.ToByte(Pub_dtTSetting.Rows[0][3]);//row1 column4            
                //Family IP
                Thread.Sleep(8000); //wait 8sec for datagrid update data
                IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        //break because  it can open port 1 time
                        server2Family.Start(); //Any IP 0.0.0.0:502
                        slave2Family = ModbusTcpSlave.CreateTcp(slaveId, server2Family);//must be outside loop funtion because this function can make memmory leak problem
                        slave2Family.Listen();
                        break;
                    }
                    
                }//End Family IP
                Tomodbus();
                this.timerModbus.Interval = (Convert.ToInt32(Pub_dtTSetting.Rows[0][5])) * 1000;
                this.timerModbus.AutoReset = false;
                this.timerModbus.Start();
                PortStatus.Text = "Running";
                is_Portrunning = true;
            }
            catch (Exception ex)
            {
                using (StreamWriter sw = File.AppendText(indebuglogFolderPath + "\\" + filename + ".txt"))
                {
                    sw.WriteLine("ERROR : " + ex);
                }
            }
        }

        protected override void OnStart(string[] args)
        {
            InitializeAllStart();
            ServerStart();

        }

        protected override void OnStop()
        {
            //File.Create(AppDomain.CurrentDomain.BaseDirectory + "Onstop.txt");
        }
        int counttoReconnectOPC = 0;
        private void timerModbus_Elapsed(object sender, EventArgs e)
        {
            //try to connect server every 60sec
            //if server down reconnect automatic
            //use thread window form not stuck            
            InitialLogFile();                       
            if (servercurrent_status == "Stop")
            {
                this.timerModbus.Stop();
                Connect_OPC();
            }
            if (servercurrent_status == "running" && is_Portrunning == true)
            {
                OPCDA_Read();
                Thread.Sleep(8000);
                int port = Convert.ToInt32(Pub_dtTSetting.Rows[0][4]); //<--- This is your value               
                                                                      
                Tomodbus();
                counttoReconnectOPC++;                                                            
                this.timerModbus.Interval = (Convert.ToInt32(Pub_dtTSetting.Rows[0][5])) * 800;
                this.timerModbus.AutoReset = false; // prevent race condition
                this.timerModbus.Start();
            }
            //if (counttoReconnectOPC == 10)//5min
            //{
            //    counttoReconnectOPC = 0;
            //    Get_TagMapping();
            //    //Add EmptyRow To DataGridView
            //    dataGridView1.Rows.Clear();
            //    for (int roww = 0; roww < Pub_dtTTAGMapping.Rows.Count; roww++)
            //    {
            //        dataGridView1.Rows.Add();
            //    }
            //    //------------------------------
            //    AddValue2Datagridview();//Add 3Column(RegNo, Modbus_Adr, TagType) To  DataGridView           
            //    Connect_OPC();
            //}
        }
    }
}
