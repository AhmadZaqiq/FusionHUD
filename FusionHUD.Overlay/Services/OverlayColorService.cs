using FusionHUD.Overlay.Interfaces;
using FusionHUD.Overlay.Models;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace FusionHUD.Overlay.Services
{
    public class OverlayColorService : IOverlayColorService
    {
        public void ApplyColor(Window Window, OverlayColor Color)
        {
            if (Window.Content is not FrameworkElement Root)
            {
                return;
            }

            SolidColorBrush Brush = Color switch
            {
                OverlayColor.White => Brushes.White,
                OverlayColor.LightGray => Brushes.LightGray,
                OverlayColor.Gray => Brushes.Gray,
                _ => Brushes.White
            };

            Root.SetValue(TextElement.ForegroundProperty, Brush);
        }
    }

}