using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LCDPanelService
{
    public partial class LCDService : ServiceBase
    {
        private const int appPort = 5200;
        private const int serialBaudRate = 9600;
        private Thread lcdThread;
        private CPUTemp cpuTemp;
        private SerialPort LCDSerialPort;
        private Socket sock;
        
        public LCDService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            //System.Diagnostics.Debugger.Launch();
            string LCDcomPort = args[0];

            sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            var ipaddress = IPAddress.Loopback;
            IPAddress add = new IPAddress(ipaddress.GetAddressBytes());
            EndPoint ep = new IPEndPoint(add, appPort);

            try
            {
                sock.Connect(ep);
            }
            catch (Exception ex)
            {

                throw new Exception("Cant connect to CPUTemp server!!! " + ex.Message);
            }

            LCDSerialPort = new SerialPort(LCDcomPort, serialBaudRate);
            if (!LCDSerialPort.IsOpen)
            {
                LCDSerialPort.Open(); 
            }

            ThreadStart job = new ThreadStart(MainLCDThread);
            lcdThread = new Thread(job);
            lcdThread.Name = "LCDThread";
            lcdThread.Start();            
        }
        protected override void OnStop()
        {
            lcdThread.Abort();
            LCDSerialPort.Close();
        }

        protected override void OnPause()
        {
            lcdThread.Suspend(); //TODO
            LCDSerialPort.Close();
        }

        protected override void OnContinue()
        {
            if (!LCDSerialPort.IsOpen)
            {
                LCDSerialPort.Open(); 
            }
            lcdThread.Resume(); //TODO
            
        }

        

        protected override void OnShutdown()
        {
            lcdThread.Abort();
            LCDSerialPort.Close();
        }

        protected override bool OnPowerEvent(PowerBroadcastStatus powerStatus)
        {
            return base.OnPowerEvent(powerStatus);
            
        }

        private void MainLCDThread()
        {
            byte[] bytes = new byte[1024];

            while (lcdThread.IsAlive)
            {
                if (sock.Connected)
                {
                    
                    int i = sock.Receive(bytes);
                    string stream = Encoding.UTF8.GetString(bytes);
                    CPUTemp cpuTemp = JsonConvert.DeserializeObject<CPUTemp>(stream);
                    //Console.WriteLine(stream); 
                    //Console.WriteLine(string.Format("Core #0: {0}°C\tCore #1: {1}°C\tCore #2: {2}°C\tCore #3: {3}°C", cpuTemp.CpuInfo.fTemp[0], cpuTemp.CpuInfo.fTemp[1], cpuTemp.CpuInfo.fTemp[2], cpuTemp.CpuInfo.fTemp[3]));
                    string lcdOutput = string.Format("{0} {1} {2} {3}", cpuTemp.CpuInfo.fTemp[0], cpuTemp.CpuInfo.fTemp[1], cpuTemp.CpuInfo.fTemp[2], cpuTemp.CpuInfo.fTemp[3]);
                    LCDSerialPort.WriteLine(lcdOutput);
                }
                
            }
        }

    }
}
