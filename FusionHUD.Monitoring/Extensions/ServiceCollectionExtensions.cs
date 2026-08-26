using FusionHUD.Monitoring.Configuration;
using FusionHUD.Monitoring.Interfaces;
using FusionHUD.Monitoring.Models;
using FusionHUD.Monitoring.Persistence;
using FusionHUD.Monitoring.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FusionHUD.Monitoring.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMonitoringServices(this IServiceCollection Services, IConfiguration Configuration)
        {
            MonitoringOptions MonitoringOptions = new()
            {
                SampleIntervalSeconds = Configuration.GetValue<int?>("Monitoring:SampleIntervalSeconds") ?? 5,

                ReportTime = Configuration.GetValue<string>("Monitoring:ReportTime") ?? "00:00"
            };

            if (MonitoringOptions.SampleIntervalSeconds <= 0)
            {
                throw new InvalidOperationException("Monitoring:SampleIntervalSeconds must be greater than zero.");
            }

            if (!TimeSpan.TryParse(MonitoringOptions.ReportTime, out TimeSpan ReportTime))
            {
                throw new InvalidOperationException("Monitoring:ReportTime must be a valid time.");
            }

            MonitoringOptions.ReportTime = ReportTime.ToString(@"hh\:mm");

            Services.AddSingleton(MonitoringOptions);

            Services.AddSingleton<IPerformanceDataProvider, PerformanceDataProvider>();

            Services.AddSingleton<IStatisticsService, StatisticsService>();

            Services.AddSingleton<IDailyStatisticsStore>(
                ServiceProvider =>
                {
                    string DataDirectory = Path.Combine(AppContext.BaseDirectory, "Data");

                    string FilePath = Path.Combine(DataDirectory, "daily-statistics.json");

                    return new DailyStatisticsStore(FilePath);
                });

            Services.AddSingleton<IDailyReportService, DailyReportService>();

            Services.AddHttpClient<TelegramReportSender>();

            Services.AddSingleton<IDailyReportSender, TelegramReportSender>();

            Services.AddSingleton<IDailyMonitoringService, DailyMonitoringService>();

            Services.AddHostedService<Worker>();

            Services.Configure<TelegramOptions>(Configuration.GetSection("Telegram"));

            Services.AddHttpClient<TelegramReportSender>();

            Services.AddHttpClient<TelegramReportSender>(Client =>
                {
                    Client.Timeout = TimeSpan.FromSeconds(15);
                });

            return Services;
        }
    }

}