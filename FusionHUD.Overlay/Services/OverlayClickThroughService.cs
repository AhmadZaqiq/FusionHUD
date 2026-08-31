using FusionHUD.Overlay.Interfaces;
using System.Runtime.InteropServices;

namespace FusionHUD.Overlay.Services
{
    public class OverlayClickThroughService : IOverlayClickThroughService
    {
        private const int GWL_EXSTYLE = -20; // Index used to access the window's extended styles.

        private const long WS_EX_TRANSPARENT = 0x00000020; // Enables mouse input to pass through the window.

        [DllImport("user32.dll", EntryPoint = "GetWindowLongPtrW")]
        private static extern IntPtr GetWindowLongPtr(IntPtr HWnd, int Index);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtrW")]
        private static extern IntPtr SetWindowLongPtr(IntPtr HWnd, int Index, IntPtr NewStyle);

        public void Apply(IntPtr Handle)
        {
            IntPtr CurrentStyle = GetWindowLongPtr(Handle, GWL_EXSTYLE);

            long NewStyle = CurrentStyle.ToInt64() | WS_EX_TRANSPARENT;

            SetWindowLongPtr(Handle, GWL_EXSTYLE, new IntPtr(NewStyle));
        }
    }
}