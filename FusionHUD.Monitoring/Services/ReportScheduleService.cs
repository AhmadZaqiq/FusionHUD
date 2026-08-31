using FusionHUD.Monitoring.Interfaces;
using FusionHUD.Monitoring.Models;

namespace FusionHUD.Monitoring.Services
{
    public sealed class ReportScheduleService : IReportScheduleService
    {
        private readonly MonitoringOptions _MonitoringOptions;

        public ReportScheduleService(MonitoringOptions MonitoringOptions)
        {
            _MonitoringOptions = MonitoringOptions;
        }

        public bool IsReportDue(DateTime CurrentTime, DateOnly? LastReportDate)
        {
            if (!TimeSpan.TryParse(_MonitoringOptions.ReportTime, out TimeSpan ReportTime))
            {
                return false;
            }

            DateOnly CurrentDate = DateOnly.FromDateTime(CurrentTime);

            if (LastReportDate == CurrentDate)
            {
                return false;
            }

            return CurrentTime.TimeOfDay >= ReportTime;
        }
    }
}