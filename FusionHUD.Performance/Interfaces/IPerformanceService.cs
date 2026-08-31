using FusionHUD.Performance.Models;

namespace FusionHUD.Performance.Interfaces
{
    public interface IPerformanceService
    {
        PerformanceSnapshot GetPerformanceSnapshot();
    }
}