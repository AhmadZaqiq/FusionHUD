using FusionHUD.Monitoring.Interfaces;
using FusionHUD.Monitoring.Models;
using FusionHUD.Performance.Interfaces;

namespace FusionHUD.Monitoring.Services
{
    public sealed class PerformanceDataProvider : IPerformanceDataProvider
    {
        private readonly IPerformanceService _PerformanceService;

        public PerformanceDataProvider(IPerformanceService PerformanceService)
        {
            _PerformanceService = PerformanceService;
        }

        public PerformanceSample GetPerformanceSample()
        {
            FusionHUD.Performance.Models.PerformanceSnapshot Snapshot = _PerformanceService.GetPerformanceSnapshot();

            return new PerformanceSample
            {
                Timestamp = DateTime.Now,

                CpuUsage = Snapshot.CPUUsage,
                CpuTemperature = Snapshot.CPUTemperature,

                GpuUsage = Snapshot.GPUUsage,
                GpuTemperature = Snapshot.GPUTemperature,

                RamUsage = Snapshot.RAMUsage,

                Fps = Snapshot.FPS
            };
        }
    }

}