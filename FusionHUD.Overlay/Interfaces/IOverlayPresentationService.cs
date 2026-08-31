using FusionHUD.Overlay.Models;
using System.Windows;

namespace FusionHUD.Overlay.Interfaces
{
    public interface IOverlayPresentationService
    {
        void ApplyPosition(Window Window, OverlayPosition Position);

        void ApplySize(Window Window, OverlaySize Size);

        void ApplyColor(Window Window, OverlayColor Color);

        void ApplyAlignment(Window Window, OverlayPosition Position);

        void ApplySettings(Window Window, OverlaySettings Settings);
    }
}