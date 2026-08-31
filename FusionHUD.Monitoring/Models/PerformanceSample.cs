namespace FusionHUD.Monitoring.Models
{
    public sealed class PerformanceSample
    {
        public DateTime Timestamp { get; init; }

        public string GameName { get; init; } = string.Empty;

        public double CpuUsage { get; init; }

        public double CpuTemperature { get; init; }

        public double GpuUsage { get; init; }

        public double GpuTemperature { get; init; }

        public double RamUsage { get; init; }

        public double Fps { get; init; }
    }

}