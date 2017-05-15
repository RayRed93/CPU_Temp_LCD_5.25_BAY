using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO.Ports;

namespace CPU_Temp_LCD
{
    class Program
    {
        static void Main(string[] args)
        {

            Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            var ipaddress = IPAddress.Loopback;       
            IPAddress add = new IPAddress(ipaddress.GetAddressBytes());
            EndPoint ep = new IPEndPoint(add, 5200);
            sock.Connect(ep);

            SerialPort serialPort = new SerialPort("COM4", 9600);
            serialPort.Open();

           CPUTemp cpuTemp = new CPUTemp();
            
            while (true)
            {
                if (sock.Connected)
                {
                    byte[] bytes = new byte[1024];
                    int i = sock.Receive(bytes);
                    string stream = Encoding.UTF8.GetString(bytes);
                    cpuTemp = JsonConvert.DeserializeObject<CPUTemp>(stream);
                    //Console.WriteLine(stream); 
                    Console.WriteLine(string.Format("Core #0: {0}°C\tCore #1: {1}°C\tCore #2: {2}°C\tCore #3: {3}°C", cpuTemp.CpuInfo.fTemp[0], cpuTemp.CpuInfo.fTemp[1], cpuTemp.CpuInfo.fTemp[2], cpuTemp.CpuInfo.fTemp[3]));
                    serialPort.WriteLine(string.Format("{0} {1} {2} {3}", cpuTemp.CpuInfo.fTemp[0], cpuTemp.CpuInfo.fTemp[1], cpuTemp.CpuInfo.fTemp[2], cpuTemp.CpuInfo.fTemp[3]));
                }
              //  Console.ReadLine();
            }
        }
    }
}
