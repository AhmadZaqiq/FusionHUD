using FusionHUD.Overlay.Interfaces;
using FusionHUD.Overlay.Models;
using System.Windows;

namespace FusionHUD.Overlay.Services
{
    public class OverlayPositionService : IOverlayPositionService
    {
        public void ApplyPosition(Window Window, OverlayPosition Position)
        {
            Rect WorkArea = SystemParameters.WorkArea;

            Window.Left = Position switch
            {
                OverlayPosition.Left => WorkArea.Left,

                OverlayPosition.Center => WorkArea.Left + (WorkArea.Width - Window.Width) / 2,

                OverlayPosition.Right => WorkArea.Right - Window.Width,

                _ =>
                    WorkArea.Left
            };

            Window.Top = WorkArea.Top;
        }
    }

}