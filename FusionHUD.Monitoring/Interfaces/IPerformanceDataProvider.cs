using FusionHUD.Monitoring.Models;

namespace FusionHUD.Monitoring.Interfaces
{
    public interface IPerformanceDataProvider
    {
        PerformanceSample GetPerformanceSample();
    }
}