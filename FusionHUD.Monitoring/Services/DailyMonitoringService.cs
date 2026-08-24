using FusionHUD.Monitoring.Interfaces;
using FusionHUD.Monitoring.Models;

namespace FusionHUD.Monitoring.Services
{
    public sealed class DailyMonitoringService : IDailyMonitoringService
    {
        private readonly IPerformanceDataProvider _PerformanceDataProvider;

        private readonly IStatisticsService _StatisticsService;

        private readonly IDailyStatisticsStore _DailyStatisticsStore;

        private readonly IDailyReportService _DailyReportService;

        private readonly IDailyReportSender _DailyReportSender;

        public DailyMonitoringService(IPerformanceDataProvider PerformanceDataProvider, IStatisticsService StatisticsService,
                                      IDailyStatisticsStore DailyStatisticsStore, IDailyReportService DailyReportService, IDailyReportSender DailyReportSender)
        {
            _PerformanceDataProvider = PerformanceDataProvider;

            _StatisticsService = StatisticsService;

            _DailyStatisticsStore = DailyStatisticsStore;

            _DailyReportService = DailyReportService;

            _DailyReportSender = DailyReportSender;
        }

        public async Task InitializeAsync(CancellationToken CancellationToken = default)
        {
            StatisticsState? State = await _DailyStatisticsStore.LoadAsync(CancellationToken);

            if (State is null)
            {
                _StatisticsService.Reset();

                return;
            }

            _StatisticsService.Restore(State);
        }

        public async Task ProcessSampleAsync(CancellationToken CancellationToken = default)
        {
            PerformanceSample Sample = _PerformanceDataProvider.GetPerformanceSample();

            _StatisticsService.Update(Sample);

            _StatisticsService.UpdateUptime();

            await _DailyStatisticsStore.SaveAsync(_StatisticsService.State, CancellationToken);
        }

        public async Task ProcessDailyReportAsync(CancellationToken CancellationToken = default)
        {
            string Report = _DailyReportService.CreateReport(_StatisticsService.Statistics);

            await _DailyReportSender.SendAsync(Report, CancellationToken);

            await _DailyStatisticsStore.DeleteAsync(CancellationToken);

            _StatisticsService.Reset();
        }
    }

}