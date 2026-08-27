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

        private readonly IReportScheduleService _ReportScheduleService;

        public DailyMonitoringService(IPerformanceDataProvider PerformanceDataProvider, IStatisticsService StatisticsService,
                                      IDailyStatisticsStore DailyStatisticsStore, IPendingReportStore PendingReportStore, IDailyReportService DailyReportService,
                                      IDailyReportSender DailyReportSender, IReportScheduleService ReportScheduleService)
        {
            _PerformanceDataProvider = PerformanceDataProvider;

            _StatisticsService = StatisticsService;

            _DailyStatisticsStore = DailyStatisticsStore;

            _PendingReportStore = PendingReportStore;

            _DailyReportService = DailyReportService;

            _DailyReportSender = DailyReportSender;

            _ReportScheduleService = ReportScheduleService;
        }

        public async Task InitializeAsync(CancellationToken CancellationToken = default)
        {
            await SendPendingReportAsync(CancellationToken);

            StatisticsState? State = await _DailyStatisticsStore.LoadAsync(CancellationToken);

            if (State is null)
            {
                _StatisticsService.Reset();

                await SaveCurrentStateAsync(CancellationToken);

                return;
            }

            DateOnly CurrentDate = DateOnly.FromDateTime(DateTime.Now);

            DateOnly StateDate = DateOnly.FromDateTime(State.Date);

            if (StateDate < CurrentDate)
            {
                await MoveStateToPendingReportAsync(State, CancellationToken);

                await _DailyStatisticsStore.DeleteAsync(CancellationToken);

                _StatisticsService.Restore(State);

                _StatisticsService.MarkReportSent(StateDate);

                _StatisticsService.Reset();

                await SaveCurrentStateAsync(CancellationToken);

                await SendPendingReportAsync(CancellationToken);

                return;
            }

            if (StateDate > CurrentDate)
            {
                _StatisticsService.Reset();

                await SaveCurrentStateAsync(CancellationToken);

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

        public async Task ProcessDailyReportIfDueAsync(CancellationToken CancellationToken = default)
        {
            DateTime CurrentTime = DateTime.Now;

            if (!_ReportScheduleService.IsReportDue(CurrentTime, _StatisticsService.LastReportDate))
            {
                return;
            }

            DateOnly ReportDate = DateOnly.FromDateTime(CurrentTime);

            StatisticsState State = _StatisticsService.State;

            await MoveStateToPendingReportAsync(State, CancellationToken);

            await _DailyStatisticsStore.DeleteAsync(CancellationToken);

            await SendPendingReportAsync(CancellationToken);

            _StatisticsService.MarkReportSent(ReportDate);

            _StatisticsService.Reset();

            await SaveCurrentStateAsync(CancellationToken);
        }

        private async Task ProcessDateChangeAsync(CancellationToken CancellationToken)
        {
            StatisticsState State = _StatisticsService.State;

            DateOnly StatisticsDate = DateOnly.FromDateTime(State.Date);

            await MoveStateToPendingReportAsync(State, CancellationToken);

            await _DailyStatisticsStore.DeleteAsync(CancellationToken);

            _StatisticsService.MarkReportSent(StatisticsDate);

            _StatisticsService.Reset();

            await SaveCurrentStateAsync(CancellationToken);

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

        private async Task SaveCurrentStateAsync(CancellationToken CancellationToken)
        {
            await _DailyStatisticsStore.SaveAsync(_StatisticsService.State, CancellationToken);
        }
    }

}