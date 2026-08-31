using FusionHUD.Monitoring.Models;

namespace FusionHUD.Monitoring.Interfaces
{
    public interface IPendingReportStore
    {
        Task<PendingReport?> LoadAsync(CancellationToken CancellationToken = default);

        Task SaveAsync(PendingReport Report, CancellationToken CancellationToken = default);

        Task DeleteAsync(CancellationToken CancellationToken = default);
    }
}