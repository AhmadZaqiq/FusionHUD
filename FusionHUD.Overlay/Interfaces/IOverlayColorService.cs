using FusionHUD.Overlay.Models;
using System.Windows;

namespace FusionHUD.Overlay.Interfaces
{
    public interface IOverlayColorService
    {
        void ApplyColor(Window Window, OverlayColor Color);
    }

}