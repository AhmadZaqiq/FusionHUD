using System.Runtime.InteropServices;

namespace FusionHUD.Performance.Native
{
    internal static partial class WindowsProcessInterop
    {
        [LibraryImport("user32.dll")]
        private static partial IntPtr GetForegroundWindow();

        [LibraryImport("user32.dll")]
        private static partial uint GetWindowThreadProcessId(IntPtr WindowHandle, out uint ProcessID);

        internal static uint GetForegroundProcessID()
        {
            IntPtr WindowHandle = GetForegroundWindow();

            if (WindowHandle == IntPtr.Zero)
            {
                return 0;
            }

            return GetWindowThreadProcessId(WindowHandle, out uint ProcessID) == 0 ? 0 : ProcessID;
        }
    }

}