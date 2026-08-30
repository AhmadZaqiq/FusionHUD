using System.Runtime.InteropServices;

namespace FusionHUD.Performance.Native
{
    internal static partial class WindowsMemoryInterop
    {
        [LibraryImport("kernel32.dll", EntryPoint = "GlobalMemoryStatusEx", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static partial bool GlobalMemoryStatusEx(ref MEMORYSTATUSEX MemoryStatus);

        [StructLayout(LayoutKind.Sequential)]
        internal struct MEMORYSTATUSEX
        {
            internal uint dwLength;
            internal uint dwMemoryLoad;
            internal ulong ullTotalPhys;
            internal ulong ullAvailPhys;
            internal ulong ullTotalPageFile;
            internal ulong ullAvailPageFile;
            internal ulong ullTotalVirtual;
            internal ulong ullAvailVirtual;
            internal ulong ullAvailExtendedVirtual;
        }
    }

}