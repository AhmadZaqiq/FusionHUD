namespace FusionHUD.Monitoring.Interfaces
{
    public interface IReportScheduleService
    {
        bool IsReportDue(DateTime CurrentTime, DateOnly? LastReportDate);
    }
}