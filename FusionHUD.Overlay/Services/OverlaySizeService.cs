using FusionHUD.Overlay.Interfaces;
using FusionHUD.Overlay.Models;
using System.Windows;

namespace FusionHUD.Overlay.Services
{
    public class OverlaySizeService : IOverlaySizeService
    {
        public void ApplySize(Window Window, OverlaySize Size)
        {
            switch (Size)
            {
                case OverlaySize.Small:
                    Window.Width = 600;
                    Window.Height = 24;
                    Window.FontSize = 13;
                    break;

                case OverlaySize.Medium:
                    Window.Width = 700;
                    Window.Height = 30;
                    Window.FontSize = 15;
                    break;

                case OverlaySize.Large:
                    Window.Width = 800;
                    Window.Height = 36;
                    Window.FontSize = 17;
                    break;
            }
        }
    }

}