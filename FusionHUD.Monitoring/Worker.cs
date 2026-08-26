using FusionHUD.Monitoring.Interfaces;
using FusionHUD.Monitoring.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FusionHUD.Monitoring
{
    public sealed class Worker : BackgroundService
    {
        private readonly IDailyMonitoringService _DailyMonitoringService;
        private readonly MonitoringOptions _MonitoringOptions;
        private readonly ILogger<Worker> _Logger;

        private DateOnly _LastReportDate = DateOnly.MinValue;

        public Worker(IDailyMonitoringService DailyMonitoringService, MonitoringOptions MonitoringOptions, ILogger<Worker> Logger)
        {
            _DailyMonitoringService = DailyMonitoringService;
            _MonitoringOptions = MonitoringOptions;
            _Logger = Logger;
        }

        protected override async Task ExecuteAsync(CancellationToken StoppingToken)
        {
            await _DailyMonitoringService.InitializeAsync(StoppingToken);

            using PeriodicTimer Timer = new(TimeSpan.FromSeconds(_MonitoringOptions.SampleIntervalSeconds));

            _Logger.LogInformation("FusionHUD monitoring started. Sample interval: {Interval}s, Report time: {ReportTime}.", _MonitoringOptions.SampleIntervalSeconds, _MonitoringOptions.ReportTime);

            try
            {
                while (await Timer.WaitForNextTickAsync(StoppingToken))
                {
                    try
                    {
                        await _DailyMonitoringService.ProcessSampleAsync(StoppingToken);
                    }
                    catch (OperationCanceledException) when (StoppingToken.IsCancellationRequested)
                    {
                        break;
                    }
                    catch (Exception Exception)
                    {
                        _Logger.LogError(Exception, "Failed to process performance sample.");
                    }

                    DateTime CurrentTime = DateTime.Now;

                    if (ShouldGenerateReport(CurrentTime))
                    {
                        try
                        {
                            await _DailyMonitoringService.ProcessDailyReportAsync(StoppingToken);

                            _LastReportDate = DateOnly.FromDateTime(CurrentTime);

                            _Logger.LogInformation("Daily report generated and sent successfully.");
                        }
                        catch (OperationCanceledException) when (StoppingToken.IsCancellationRequested)
                        {
                            break;
                        }
                        catch (Exception Exception)
                        {
                            _Logger.LogError(Exception, "Failed to generate or send daily report.");
                        }
                    }
                }
            }
            catch (OperationCanceledException) when (StoppingToken.IsCancellationRequested)
            {
                _Logger.LogInformation("FusionHUD monitoring is stopping.");
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