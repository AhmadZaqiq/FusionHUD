using FusionHUD.Monitoring.Interfaces;

namespace FusionHUD.Monitoring.Interfaces
{
    public interface IGameReportSender
    {
        Task SendAsync(string Report, CancellationToken CancellationToken = default);
    }

}