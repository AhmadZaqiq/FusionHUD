namespace FusionHUD.Monitoring.Interfaces
{
    public interface IDailyReportService
    {
        string CreateReport(Models.DailyStatistics Statistics);
    }
}