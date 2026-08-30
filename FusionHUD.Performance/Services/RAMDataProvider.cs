using FusionHUD.Performance.Interfaces;
using FusionHUD.Performance.Native;
using System.Runtime.InteropServices;

namespace FusionHUD.Performance.Services
{
    public sealed class RAMDataProvider : IRAMDataProvider
    {
        public double GetRAMUsage()
        {
            WindowsMemoryInterop.MEMORYSTATUSEX MemoryStatus = new()
            {
                dwLength = (uint)Marshal.SizeOf<WindowsMemoryInterop.MEMORYSTATUSEX>()
            };

            if (!WindowsMemoryInterop.GlobalMemoryStatusEx(ref MemoryStatus))
            {
                return 0;
            }

            ulong UsedMemory = MemoryStatus.ullTotalPhys - MemoryStatus.ullAvailPhys;

            return UsedMemory / 1024.0 / 1024.0 / 1024.0;
        }
    }

}