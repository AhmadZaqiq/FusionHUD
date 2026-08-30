using System.Runtime.InteropServices;

namespace FusionHUD.Performance.Native
{
    internal static partial class FusionHUDAMDInterop
    {
        private const string AMD_DLL = "FusionHUD.AMD.dll";

        [LibraryImport(AMD_DLL, EntryPoint = "InitAMDMonitor")]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static partial bool InitAMDMonitor();

        [LibraryImport(AMD_DLL, EntryPoint = "GetCPUTemperature")]
        internal static partial double GetCPUTemperature();

        [LibraryImport(AMD_DLL, EntryPoint = "ShutdownAMDMonitor")]
        internal static partial void ShutdownAMDMonitor();
    }

}