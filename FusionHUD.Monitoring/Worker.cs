using FusionHUD.Monitoring.Interfaces;
using FusionHUD.Monitoring.Models;
using Microsoft.Extensions.Hosting;

namespace FusionHUD.Monitoring
{
    public sealed class Worker : BackgroundService
    {
        private readonly IDailyMonitoringService _DailyMonitoringService;

        private readonly IReportScheduleService _ReportScheduleService;

        private readonly MonitoringOptions _MonitoringOptions;

        public Worker(IDailyMonitoringService DailyMonitoringService, IReportScheduleService ReportScheduleService, MonitoringOptions MonitoringOptions)
        {
            _DailyMonitoringService = DailyMonitoringService;

            _ReportScheduleService = ReportScheduleService;

            _MonitoringOptions = MonitoringOptions;
        }

        protected override async Task ExecuteAsync(CancellationToken StoppingToken)
        {
            await _DailyMonitoringService.InitializeAsync(StoppingToken);

            using PeriodicTimer Timer = new(TimeSpan.FromSeconds(_MonitoringOptions.SampleIntervalSeconds));

            DateOnly LastProcessedReportDate = DateOnly.MinValue;

            while (!StoppingToken.IsCancellationRequested && await Timer.WaitForNextTickAsync(StoppingToken))
            {
                await _DailyMonitoringService.ProcessSampleAsync(StoppingToken);

                if (StoppingToken.IsCancellationRequested)
                {
                    break;
                }

                DateTime CurrentTime = DateTime.Now;

                if (!_ReportScheduleService.IsReportDue(CurrentTime))
                {
                    continue;
                }

                DateOnly CurrentDate = DateOnly.FromDateTime(CurrentTime);

                if (CurrentDate == LastProcessedReportDate)
                {
                    continue;
                }

                await _DailyMonitoringService.ProcessDailyReportAsync(StoppingToken);

                LastProcessedReportDate = CurrentDate;
            }
        }
    }

}