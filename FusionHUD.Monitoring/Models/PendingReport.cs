namespace FusionHUD.Monitoring.Models
{
    public sealed class PendingReport
    {
        public DateTime Date { get; set; }

        public DailyStatistics Statistics { get; set; } = new();
    }
}