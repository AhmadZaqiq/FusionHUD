using FusionHUD.Overlay.Interfaces;
using FusionHUD.Overlay.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace FusionHUD.Overlay.Services
{
    public class OverlayPresentationService : IOverlayPresentationService
    {
        public void ApplyPosition(Window Window, OverlayPosition Position)
        {
            Rect WorkArea = SystemParameters.WorkArea;

            Window.Left = Position switch
            {
                OverlayPosition.Left => WorkArea.Left,

                OverlayPosition.Center => WorkArea.Left + (WorkArea.Width - Window.Width) / 2,

                OverlayPosition.Right => WorkArea.Right - Window.Width,

                _ => WorkArea.Left
            };

            Window.Top = WorkArea.Top;
        }

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

        public void ApplyAlignment(Window Window, OverlayPosition Position)
        {
            if (Window.Content is not Grid Grid)
            {
                return;
            }

            if (Grid.Children.Count == 0 || Grid.Children[0] is not TextBlock TextBlock)
            {
                return;
            }

            TextBlock.TextAlignment = Position switch
            {
                OverlayPosition.Left => TextAlignment.Left,
                OverlayPosition.Center => TextAlignment.Center,
                OverlayPosition.Right => TextAlignment.Right,
                _ => TextAlignment.Left
            };
        }

        public void ApplySettings(Window Window, OverlaySettings Settings)
        {
            ApplySize(Window, Settings.Size);
            ApplyPosition(Window, Settings.Position);
            ApplyColor(Window, Settings.Color);
            ApplyAlignment(Window, Settings.Position);
        }
    }
}