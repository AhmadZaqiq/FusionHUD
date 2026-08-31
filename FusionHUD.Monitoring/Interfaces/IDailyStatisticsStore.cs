using FusionHUD.Monitoring.Models;

namespace FusionHUD.Monitoring.Interfaces
{
    public interface IDailyStatisticsStore
    {
        Task<StatisticsState?> LoadAsync(CancellationToken CancellationToken = default);

        Task SaveAsync(StatisticsState State, CancellationToken CancellationToken = default);

        Task DeleteAsync(CancellationToken CancellationToken = default);
    }
}