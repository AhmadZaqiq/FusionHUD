using FusionHUD.Performance.Interfaces;
using System.Runtime.InteropServices;

namespace FusionHUD.Performance.Services
{
    public sealed class RAMDataProvider : IRAMDataProvider
    {
        public double GetRAMUsage()
        {
            MEMORYSTATUSEX MemoryStatus = new()
            {
                dwLength = (uint)Marshal.SizeOf<MEMORYSTATUSEX>()
            };

            if (!GlobalMemoryStatusEx(ref MemoryStatus))
            {
                return 0;
            }

            ulong UsedMemory = MemoryStatus.ullTotalPhys - MemoryStatus.ullAvailPhys;

            return UsedMemory / 1024.0 / 1024.0 / 1024.0;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MEMORYSTATUSEX
        {
            public uint dwLength;
            public uint dwMemoryLoad;
            public ulong ullTotalPhys;
            public ulong ullAvailPhys;
            public ulong ullTotalPageFile;
            public ulong ullAvailPageFile;
            public ulong ullTotalVirtual;
            public ulong ullAvailVirtual;
            public ulong ullAvailExtendedVirtual;
        }

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GlobalMemoryStatusEx(ref MEMORYSTATUSEX lpBuffer);
    }

}