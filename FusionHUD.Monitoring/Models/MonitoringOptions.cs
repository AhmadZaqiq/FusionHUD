namespace FusionHUD.Monitoring.Models
{
    public sealed class MonitoringOptions
    {
        public int SampleIntervalSeconds { get; set; } = 5;

        public string ReportTime { get; set; } = "00:00";
    }

}