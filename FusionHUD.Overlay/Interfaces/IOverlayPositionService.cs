using FusionHUD.Overlay.Models;
using System.Windows;

namespace FusionHUD.Overlay.Interfaces
{
    public interface IOverlayPositionService
    {
        void ApplyPosition(Window Window, OverlayPosition Position);
    }

}