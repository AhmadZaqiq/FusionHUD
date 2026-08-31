using FusionHUD.Monitoring.Models;

namespace FusionHUD.Monitoring.Interfaces
{
    public interface IGameSessionTracker
    {
        GameSessionStatistics? ProcessSample(PerformanceSample Sample);
    }

}
