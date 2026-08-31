using FusionHUD.Interfaces;
using FusionHUD.Overlay;

namespace FusionHUD.Services
{
    public class OverlayLauncher : IOverlayLauncher
    {
        private readonly MainWindow _Window;

        public OverlayLauncher(MainWindow Window)
        {
            _Window = Window;
        }

        public void Start()
        {
            _Window.Show();
        }
    }
}