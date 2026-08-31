using FusionHUD.Monitoring.Models;

namespace FusionHUD.Monitoring.Interfaces
{
    public interface IGameSessionReportService
    {
        string CreateReport(GameSessionStatistics Statistics);
    }

}