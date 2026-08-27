using FusionHUD.Monitoring.Models;

namespace FusionHUD.Monitoring.Interfaces
{
    public interface IStatisticsService
    {
        DailyStatistics Statistics { get; }

        StatisticsState State { get; }

        DateOnly? LastReportDate { get; }

        void Update(PerformanceSample Sample);

        void UpdateUptime();

        void Restore(StatisticsState State);

        void Reset();

        void MarkReportSent(DateOnly ReportDate);
    }

}