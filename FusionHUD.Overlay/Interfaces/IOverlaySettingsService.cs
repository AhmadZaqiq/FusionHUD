using FusionHUD.Overlay.Models;

namespace FusionHUD.Overlay.Interfaces
{
    public interface IOverlaySettingsService
    {
        OverlaySettings Settings { get; }

        void SetPosition(OverlayPosition Position);

        void SetSize(OverlaySize Size);

        void SetColor(OverlayColor Color);

        void MoveToNextPosition();

        void MoveToNextSize();

        void MoveToNextColor();
    }

}