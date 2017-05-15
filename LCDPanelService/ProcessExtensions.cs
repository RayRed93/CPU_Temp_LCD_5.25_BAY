using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCDPanelService
{
    public static class ProcessExtensions
    {
        public static Process process { get; set; }
        public delegate void ExitFunction();

        public class MyEventArgs : EventArgs
        {
            public ExitFunction ext { get; set; }
        }

        public static bool IsRunning(Process process)
        {
            if (process == null)
                throw new ArgumentNullException("process");

            process.Refresh();

            if (process.HasExited)
                return false;
            else
                return true;          
        }

        public static Process GetProcessByName(string procName)
        {
            Process proc;
            try
            {
                proc = Process.GetProcessesByName(procName).First<Process>();
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Process not found :(", ex);
            }
            return proc;
        }

        public static ExitFunction RegisterProcessExit(Process process, ExitFunction exit)
        {
            MyEventArgs e = new MyEventArgs();
            e.ext = exit;
           
            process.Exited += process_Exited;
            
            return e.ext;
        }

        private static void process_Exited(object sender, EventArgs e)
        {
            throw new NotImplementedException();
            
        }
    }
}
