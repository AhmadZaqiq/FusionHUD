using FusionHUD.Monitoring.Interfaces;
using FusionHUD.Monitoring.Models;
using Microsoft.Extensions.Hosting;

namespace FusionHUD.Monitoring
{
    public sealed class Worker : BackgroundService
    {
        private readonly IDailyMonitoringService _DailyMonitoringService;

        private readonly IPerformanceDataProvider _PerformanceDataProvider;

        private readonly IGameSessionTracker _GameSessionTracker;

        private readonly IGameSessionReportService _GameReportService;

        private readonly IGameReportSender _GameReportSender;

        private readonly MonitoringOptions _MonitoringOptions;

        public Worker(IDailyMonitoringService DailyMonitoringService, IPerformanceDataProvider PerformanceDataProvider,
                      IGameSessionTracker GameSessionTracker, IGameSessionReportService GameReportService,
                      IGameReportSender GameReportSender, MonitoringOptions MonitoringOptions)
        {
            _DailyMonitoringService = DailyMonitoringService;

            _PerformanceDataProvider = PerformanceDataProvider;

            _GameSessionTracker = GameSessionTracker;

            _GameReportService = GameReportService;

            _GameReportSender = GameReportSender;

            _MonitoringOptions = MonitoringOptions;
        }

        protected override async Task ExecuteAsync(CancellationToken StoppingToken)
        {
            await _DailyMonitoringService.InitializeAsync(StoppingToken);

            using PeriodicTimer Timer = new(TimeSpan.FromSeconds(_MonitoringOptions.SampleIntervalSeconds));

            while (!StoppingToken.IsCancellationRequested && await Timer.WaitForNextTickAsync(StoppingToken))
            {
                await _DailyMonitoringService.ProcessSampleAsync(StoppingToken);

                PerformanceSample Sample = _PerformanceDataProvider.GetPerformanceSample();

                GameSessionStatistics? Statistics = _GameSessionTracker.ProcessSample(Sample);

                if (Statistics is not null)
                {
                    string Report = _GameReportService.CreateReport(Statistics);

                    await _GameReportSender.SendAsync(Report, StoppingToken);
                }

                if (StoppingToken.IsCancellationRequested)
                {
                    break;
                }

                await _DailyMonitoringService.ProcessDailyReportIfDueAsync(StoppingToken);
            }
        }
    }
}