using FusionHUD.Monitoring.Interfaces;
using FusionHUD.Monitoring.Models;
using Microsoft.Extensions.Hosting;

namespace FusionHUD.Monitoring
{
    public sealed class Worker(IDailyMonitoringService DailyMonitoringService) : BackgroundService
    {
        private readonly MonitoringOptions _MonitoringOptions =
            new()
            {
                SampleIntervalSeconds = 5
            };

        protected override async Task ExecuteAsync(CancellationToken StoppingToken)
        {
            await DailyMonitoringService.InitializeAsync(StoppingToken);

            using PeriodicTimer Timer = new(TimeSpan.FromSeconds(_MonitoringOptions.SampleIntervalSeconds));

            DateTime ReportStartTime = DateTime.Now;

            while (await Timer.WaitForNextTickAsync(StoppingToken))
            {
                await DailyMonitoringService.ProcessSampleAsync(StoppingToken);

                if (DateTime.Now - ReportStartTime >= TimeSpan.FromSeconds(30))
                {
                    await DailyMonitoringService.ProcessDailyReportAsync(StoppingToken);

                    ReportStartTime = DateTime.Now;
                }
            }
        }
    }

}