using FusionHUD.Monitoring.Interfaces;
using FusionHUD.Monitoring.Models;

namespace FusionHUD.Monitoring.Services
{
    public sealed class DailyMonitoringService : IDailyMonitoringService
    {
        private readonly IPerformanceDataProvider _PerformanceDataProvider;

        private readonly IStatisticsService _StatisticsService;

        private readonly IDailyStatisticsStore _DailyStatisticsStore;

        private readonly IPendingReportStore _PendingReportStore;

        private readonly IDailyReportService _DailyReportService;

        private readonly IDailyReportSender _DailyReportSender;

        public DailyMonitoringService(IPerformanceDataProvider PerformanceDataProvider, IStatisticsService StatisticsService,
                                      IDailyStatisticsStore DailyStatisticsStore, IPendingReportStore PendingReportStore,
                                      IDailyReportService DailyReportService, IDailyReportSender DailyReportSender)
        {
            _PerformanceDataProvider = PerformanceDataProvider;

            _StatisticsService = StatisticsService;

            _DailyStatisticsStore = DailyStatisticsStore;

            _PendingReportStore = PendingReportStore;

            _DailyReportService = DailyReportService;

            _DailyReportSender = DailyReportSender;
        }

        public async Task InitializeAsync(CancellationToken CancellationToken = default)
        {
            await SendPendingReportAsync(CancellationToken);

            StatisticsState? State = await _DailyStatisticsStore.LoadAsync(CancellationToken);

            if (State is null)
            {
                _StatisticsService.Reset();

                return;
            }

            DateOnly CurrentDate = DateOnly.FromDateTime(DateTime.Now);

            DateOnly StateDate = DateOnly.FromDateTime(State.Date);

            if (StateDate != CurrentDate)
            {
                await MoveStateToPendingReportAsync(State, CancellationToken);

                await SendPendingReportAsync(CancellationToken);

                _StatisticsService.Reset();

                return;
            }

            _StatisticsService.Restore(State);
        }

        public async Task ProcessSampleAsync(CancellationToken CancellationToken = default)
        {
            DateOnly CurrentDate = DateOnly.FromDateTime(DateTime.Now);

            DateOnly StatisticsDate = DateOnly.FromDateTime(_StatisticsService.Statistics.StartTime);

            if (StatisticsDate != CurrentDate)
            {
                await ProcessDateChangeAsync(CancellationToken);
            }

            PerformanceSample Sample = _PerformanceDataProvider.GetPerformanceSample();

            _StatisticsService.Update(Sample);

            _StatisticsService.UpdateUptime();

            await _DailyStatisticsStore.SaveAsync(_StatisticsService.State, CancellationToken);
        }

        public async Task ProcessDailyReportAsync(CancellationToken CancellationToken = default)
        {
            StatisticsState? State = await _DailyStatisticsStore.LoadAsync(CancellationToken);

            if (State is null)
            {
                return;
            }

            await MoveStateToPendingReportAsync(State, CancellationToken);

            await _DailyStatisticsStore.DeleteAsync(CancellationToken);

            _StatisticsService.Reset();

            await SendPendingReportAsync(CancellationToken);
        }

        private async Task ProcessDateChangeAsync(CancellationToken CancellationToken)
        {
            StatisticsState State = _StatisticsService.State;

            await MoveStateToPendingReportAsync(State, CancellationToken);

            await _DailyStatisticsStore.DeleteAsync(CancellationToken);

            _StatisticsService.Reset();

            await SendPendingReportAsync(CancellationToken);
        }

        private async Task MoveStateToPendingReportAsync(StatisticsState State, CancellationToken CancellationToken)
        {
            PendingReport PendingReport =
                new()
                {
                    Date = State.Date,

                    Statistics = State.Statistics
                };

            await _PendingReportStore.SaveAsync(PendingReport, CancellationToken);
        }

        private async Task SendPendingReportAsync(CancellationToken CancellationToken)
        {
            PendingReport? PendingReport = await _PendingReportStore.LoadAsync(CancellationToken);

            if (PendingReport is null)
            {
                return;
            }

            string Report = _DailyReportService.CreateReport(PendingReport.Statistics);

            await _DailyReportSender.SendAsync(Report, CancellationToken);

            await _PendingReportStore.DeleteAsync(CancellationToken);
        }
    }

}