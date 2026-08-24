using FusionHUD.Overlay.Interfaces;
using FusionHUD.Overlay.Models;
using System.Windows;
using System.Windows.Controls;

namespace FusionHUD.Overlay.Services
{
    public class OverlayAlignmentService : IOverlayAlignmentService
    {
        public void ApplyAlignment(Window Window,OverlayPosition Position)
        {
            if (Window.Content is not Grid Grid)
            {
                return;
            }

            if (Grid.Children.Count == 0 ||Grid.Children[0] is not TextBlock TextBlock)
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
    }

}