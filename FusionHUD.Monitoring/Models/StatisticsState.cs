namespace FusionHUD.Monitoring.Models
{
    public sealed class StatisticsState
    {
        public DateTime Date { get; set; }

        public DailyStatistics Statistics { get; set; } = new();

        public double CpuUsageTotal { get; set; }

        public int CpuUsageSamples { get; set; }

        public double CpuTemperatureTotal { get; set; }

        public int CpuTemperatureSamples { get; set; }

        public double GpuUsageTotal { get; set; }

        public int GpuUsageSamples { get; set; }

        public double GpuTemperatureTotal { get; set; }

        public int GpuTemperatureSamples { get; set; }

        public double RamUsageTotal { get; set; }

        public int RamUsageSamples { get; set; }

        public double FpsTotal { get; set; }

        public int FpsSamples { get; set; }
    }

}