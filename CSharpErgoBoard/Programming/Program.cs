// using
using System;
using System.Windows.Forms;

namespace CSharpErgoBoard
{
    /// <summary>
    /// This is the main program class. 
    /// This is the main entry point for the entire application.
    /// </summary>
    /// <remarks>
    /// This is intentionally made to be the only class that can initalize settings. 
    /// </remarks>
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// <remarks>
        /// This thread is created by windows forms. The main purpose of this is to start or initalize all processes. 
        /// The processes started are : Logging, ErgoBoard, System Monitor.
        /// </remarks>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Programming.Logging.Instance.Log("Program Has started");    // Have the logging system start
            Programming.SystemMonitor.Instance.Existance(); // Have the system monitor start
            Application.Run(new Design.FreeErgonomics());
        }
    }
}
