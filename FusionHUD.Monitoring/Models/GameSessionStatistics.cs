namespace FusionHUD.Monitoring.Models
{
    public sealed class GameSessionStatistics
    {
        public string GameName { get; init; } = string.Empty;

        public TimeSpan Duration { get; init; }

        public double FpsAverage { get; init; }

        public double CpuUsageAverage { get; init; }
        public double CpuUsageMaximum { get; init; }

        public double CpuTemperatureAverage { get; init; }
        public double CpuTemperatureMaximum { get; init; }

        public double GpuUsageAverage { get; init; }
        public double GpuUsageMaximum { get; init; }

        public double GpuTemperatureAverage { get; init; }
        public double GpuTemperatureMaximum { get; init; }

        public double RamUsageAverage { get; init; }
        public double RamUsageMaximum { get; init; }
    }

}