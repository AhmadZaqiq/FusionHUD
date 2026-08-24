using FusionHUD.Monitoring.Interfaces;
using FusionHUD.Monitoring.Persistence;
using FusionHUD.Monitoring.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FusionHUD.Monitoring.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMonitoringServices(this IServiceCollection Services)
        {
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

            Services.AddSingleton<IDailyReportSender, ConsoleReportSender>();

            Services.AddSingleton<IDailyMonitoringService, DailyMonitoringService>();

            Services.AddHostedService<Worker>();

            return Services;
        }
    }

}