namespace FusionHUD.Performance.Models
{
    public sealed record FPSData
    {
        public float FPS { get; init; }

        public string GameName { get; init; } = string.Empty;
    }
}