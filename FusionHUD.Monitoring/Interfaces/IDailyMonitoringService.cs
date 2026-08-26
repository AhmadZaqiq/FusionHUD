using System.Threading;
using System.Threading.Tasks;

namespace FusionHUD.Monitoring.Interfaces
{
    public interface IDailyMonitoringService
    {
        Task InitializeAsync(CancellationToken CancellationToken = default);

        Task ProcessSampleAsync(CancellationToken CancellationToken = default);

        Task ProcessDailyReportAsync(CancellationToken CancellationToken = default);
    }

}