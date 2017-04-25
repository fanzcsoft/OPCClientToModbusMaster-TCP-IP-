using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;
using System.Threading;

namespace OPC_To_Modbus_Server
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]        
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            // Add the event handler for handling UI thread exceptions to the event.

            //Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);

            //// Set the unhandled exception mode to force all Windows Forms errors to go through
            //// our handler.
            //Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

            //// Add the event handler for handling non-UI thread exceptions to the event. 
            //AppDomain.CurrentDomain.UnhandledException +=
            //    new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            Application.Run(new OPC_To_ModBus_Server());
        }

        static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            //MessageBox.Show(e.Exception.Message, "Unhandled Thread Exception");
            Application.Restart();
            Application.ExitThread();
            // here you can log the exception ...
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {           
            //MessageBox.Show((e.ExceptionObject as Exception).Message, "Unhandled UI Exception");
            Application.Restart();
            Application.ExitThread();
            // here you can log the exception ...
            ;
        }       
        
    }
}
