namespace FusionHUD.Monitoring.Models
{
    public sealed class DailyStatistics
    {
        public DateTime Date { get; init; }

        public DateTime StartTime { get; set; }

        public TimeSpan Uptime { get; set; }

        public double CpuUsageAverage { get; set; }
        public double CpuUsageMaximum { get; set; }

        public double CpuTemperatureAverage { get; set; }
        public double CpuTemperatureMaximum { get; set; }

        public double GpuUsageAverage { get; set; }
        public double GpuUsageMaximum { get; set; }

        public double GpuTemperatureAverage { get; set; }
        public double GpuTemperatureMaximum { get; set; }

        public double RamUsageAverage { get; set; }
        public double RamUsageMaximum { get; set; }

        public bool IsReportSent { get; set; }
    }

}