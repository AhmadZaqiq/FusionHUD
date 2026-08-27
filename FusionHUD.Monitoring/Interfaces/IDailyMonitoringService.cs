namespace FusionHUD.Monitoring.Interfaces
{
    public interface IDailyMonitoringService
    {
        Task InitializeAsync(CancellationToken CancellationToken = default);

        Task ProcessSampleAsync(CancellationToken CancellationToken = default);

        Task ProcessDailyReportIfDueAsync(CancellationToken CancellationToken = default);
    }

}