using FusionHUD.Overlay.Models;

namespace FusionHUD.Overlay.Interfaces
{
    public interface IHotkeyService
    {
        void Register(IntPtr Handle);

        void Unregister(IntPtr Handle);

        OverlayHotkeyAction? GetHotkeyAction(int Message, IntPtr WParam);
    }
}