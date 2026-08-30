namespace FusionHUD.Overlay.Interfaces
{
    public interface IOverlayWindowService
    {
        void Initialize(System.Windows.Window Window);

        void Show();

        void Hide();

        void ToggleVisibility();

        void ApplySettings();

        void Dispose();
    }

}
