using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPU_Temp_LCD
{
   public class CPUTemp
    {
        public CpuInfo CpuInfo { get; set; }
        public MemoryInfo MemoryInfo { get; set; }
    }
    public class CpuInfo
    {
        public List<int> uiLoad { get; set; }
        public List<int> uiTjMax { get; set; }
        public int uiCoreCnt { get; set; }
        public int uiCPUCnt { get; set; }
        public List<int> fTemp { get; set; }
        public double fVID { get; set; }
        public double fCPUSpeed { get; set; }
        public double fFSBSpeed { get; set; }
        public int fMultiplier { get; set; }
        public string CPUName { get; set; }
        public int ucFahrenheit { get; set; }
        public int ucDeltaToTjMax { get; set; }
        public int ucTdpSupported { get; set; }
        public int ucPowerSupported { get; set; }
        public int uiStructVersion { get; set; }
        public List<int> uiTdp { get; set; }
        public List<double> fPower { get; set; }
        public List<int> fMultipliers { get; set; }
    }

    public class MemoryInfo
    {
        public int TotalPhys { get; set; }
        public int FreePhys { get; set; }
        public int TotalPage { get; set; }
        public int FreePage { get; set; }
        public int TotalVirtual { get; set; }
        public int FreeVirtual { get; set; }
        public int FreeExtendedVirtual { get; set; }
        public int MemoryLoad { get; set; }
    }

   
}
