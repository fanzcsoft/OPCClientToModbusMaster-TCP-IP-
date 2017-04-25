using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
//using System.Collections;
//using System.ComponentModel;
//using System.Configuration.Install;
//using System.Data;
//using System.Drawing;
//using System.Data.OleDb;
using System.IO;
//using System.Net.Sockets; //check port open close
//using System.Net; //start stop port
//using System.Globalization;
//using System.Windows;
//using System.Runtime;
//using System.Runtime.InteropServices;

namespace PI_OPC2Modbus
{
    static class Program
    {
        public static string serviceName = "";
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 
        public static PI_OPC2Modbus myservice = new PI_OPC2Modbus();

    static void Main()
        {
#if DEBUG
            //Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);

            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            PI_OPC2Modbus myservice = new PI_OPC2Modbus();
            serviceName = myservice.ServiceName;
            myservice.OnDebug();
            System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);

#else          
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
                {
                    new PI_OPC2Modbus()
                };
            ServiceBase.Run(ServicesToRun);

#endif
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            //Console.WriteLine("servicename : " + serviceName);
            ServiceController serviceController = new ServiceController(serviceName);

            try
            {
#if DEBUG
                if ((serviceController.Status.Equals(ServiceControllerStatus.Running)) || (serviceController.Status.Equals(ServiceControllerStatus.StartPending)))
                {
                    serviceController.Stop();
                }
                serviceController.WaitForStatus(ServiceControllerStatus.Stopped);
                Thread.Sleep(10000);

                myservice.Stop();
                myservice.Dispose();
                //myservice = new PI_OPC2Modbus();
                myservice.OnDebug();
                System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);
#else           
#endif
            }
            catch (Exception ex)
            {
                //string dateTime = DateTime.Now.ToString();
                //string foldername = Convert.ToDateTime(dateTime).ToString("yyyy-MM-dd");
                //string filename = foldername;
                //string inlogFolderPath = AppDomain.CurrentDomain.BaseDirectory + "\\Log\\" + foldername;
                //String AppendFilepath = (Path.Combine(inlogFolderPath, filename)) + ".txt";
                //Console.WriteLine("Exception : " + ex);
                //string timeinlog = Convert.ToDateTime(DateTime.Now.ToString()).ToString("yyyy-MM-dd HH:mm:ss= ");
                //using (StreamWriter sw = File.AppendText(AppendFilepath))
                //{
                //    //Pub_dtTSetting.Rows[0][2].ToString() => ServerName                  
                //    sw.WriteLine(timeinlog + ex);
                //}
            }
            // here you can log the exception ...

        }

    }
}
