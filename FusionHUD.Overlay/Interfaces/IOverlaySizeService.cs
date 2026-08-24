using FusionHUD.Overlay.Models;
using System.Windows;

namespace FusionHUD.Overlay.Interfaces
{
    public interface IOverlaySizeService
    {
        void ApplySize(Window Window, OverlaySize Size);
    }

}