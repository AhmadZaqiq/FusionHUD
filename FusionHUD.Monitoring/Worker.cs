using FusionHUD.Monitoring.Interfaces;
using FusionHUD.Monitoring.Models;
using Microsoft.Extensions.Hosting;

namespace FusionHUD.Monitoring
{
    public sealed class Worker : BackgroundService
    {
        private readonly IDailyMonitoringService _DailyMonitoringService;
        private readonly MonitoringOptions _MonitoringOptions;

        private DateOnly _LastReportDate = DateOnly.MinValue;

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

                DateTime CurrentTime = DateTime.Now;

                if (ShouldGenerateReport(CurrentTime))
                {
                    await _DailyMonitoringService.ProcessDailyReportAsync(StoppingToken);

                    _LastReportDate = DateOnly.FromDateTime(CurrentTime);
                }
            }
        }

        private bool ShouldGenerateReport(DateTime CurrentTime)
        {
            if (!TimeSpan.TryParse(_MonitoringOptions.ReportTime, out TimeSpan ReportTime))
            {
                return false;
            }

            DateOnly CurrentDate = DateOnly.FromDateTime(CurrentTime);

            if (CurrentDate == _LastReportDate)
            {
                return false;
            }

            return CurrentTime.TimeOfDay >= ReportTime;
        }
    }

}