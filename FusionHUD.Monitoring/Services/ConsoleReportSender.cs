using System.Diagnostics;
using FusionHUD.Monitoring.Interfaces;

namespace FusionHUD.Monitoring.Services
{
    public sealed class ConsoleReportSender : IDailyReportSender
    {
        public Task SendAsync(
            string Report,
            CancellationToken CancellationToken = default)
        {
            Debug.WriteLine(string.Empty);
            Debug.WriteLine(Report);
            Debug.WriteLine(string.Empty);

            return Task.CompletedTask;
        }
    }
}