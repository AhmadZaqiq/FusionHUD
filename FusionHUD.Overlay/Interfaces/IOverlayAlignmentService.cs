using FusionHUD.Overlay.Models;
using System.Windows;

namespace FusionHUD.Overlay.Interfaces
{
    public interface IOverlayAlignmentService
    {
        void ApplyAlignment(Window Window, OverlayPosition Position);
    }

}