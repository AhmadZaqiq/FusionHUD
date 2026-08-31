namespace FusionHUD.Performance.Models
{
    public sealed record PerformanceSnapshot
    {
        public float FPS { get; init; }

        public string GameName { get; init; } = string.Empty;

        public string GPUName { get; init; } = string.Empty;
        public float GPUUsage { get; init; }
        public float GPUTemperature { get; init; }
        public double VRAM { get; init; }

        public string CPUName { get; init; } = string.Empty;
        public float CPUUsage { get; init; }
        public float CPUTemperature { get; init; }

        public double RAMUsage { get; init; }
    }

}