using FusionHUD.Monitoring.Interfaces;
using FusionHUD.Monitoring.Models;
using Microsoft.Extensions.Hosting;

namespace FusionHUD.Monitoring
{
    public sealed class Worker : BackgroundService
    {
        private readonly IDailyMonitoringService _DailyMonitoringService;

        private readonly MonitoringOptions _MonitoringOptions;

        public Worker(IDailyMonitoringService DailyMonitoringService, MonitoringOptions MonitoringOptions)
        {
            _DailyMonitoringService = DailyMonitoringService;

            _MonitoringOptions = MonitoringOptions;
        }

        protected override async Task ExecuteAsync(CancellationToken StoppingToken)
        {
            await _DailyMonitoringService.InitializeAsync(StoppingToken);

            using PeriodicTimer Timer = new(TimeSpan.FromSeconds(_MonitoringOptions.SampleIntervalSeconds));

            while (!StoppingToken.IsCancellationRequested && await Timer.WaitForNextTickAsync(StoppingToken))
            {
                await _DailyMonitoringService.ProcessSampleAsync(StoppingToken);

                if (StoppingToken.IsCancellationRequested)
                {
                    break;
                }

                await _DailyMonitoringService.ProcessDailyReportIfDueAsync(StoppingToken);
            }
        }
    }

}