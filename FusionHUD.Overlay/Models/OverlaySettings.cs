namespace FusionHUD.Overlay.Models
{
    public class OverlaySettings
    {
        public OverlayPosition Position { get; set; } = OverlayPosition.Left;

        public OverlaySize Size { get; set; } = OverlaySize.Small;

        public OverlayColor Color { get; set; } = OverlayColor.White;
    }

    public enum OverlayPosition
    {
        Left,
        Center,
        Right
    }

    public enum OverlaySize
    {
        Small,
        Medium,
        Large
    }

    public enum OverlayColor
    {
        White,
        LightGray,
        Gray
    }

}