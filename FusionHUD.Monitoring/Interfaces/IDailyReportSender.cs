namespace FusionHUD.Monitoring.Interfaces
{
    public interface IDailyReportSender
    {
        Task SendAsync(string Report, CancellationToken CancellationToken = default);
    }

}