using FusionHUD.Performance.Interfaces;
using FusionHUD.Performance.Models;

namespace FusionHUD.Performance.Services
{
    public sealed class PerformanceService : IPerformanceService
    {
        private readonly IGPUDataProvider _GPUDataProvider;

        private readonly ICPUDataProvider _CPUDataProvider;

        private readonly IRAMDataProvider _RAMDataProvider;

        private readonly IFPSDataProvider _FPSDataProvider;

        public PerformanceService(IGPUDataProvider GPUDataProvider, ICPUDataProvider CPUDataProvider, IRAMDataProvider RAMDataProvider, IFPSDataProvider FPSDataProvider)
        {
            _GPUDataProvider = GPUDataProvider;

            _CPUDataProvider = CPUDataProvider;

            _RAMDataProvider = RAMDataProvider;

            _FPSDataProvider = FPSDataProvider;
        }

        public PerformanceSnapshot GetPerformanceSnapshot()
        {
            return new PerformanceSnapshot
            {
                FPS = _FPSDataProvider.GetFPS(),

                GPUName = _GPUDataProvider.GPUName,
                GPUUsage = _GPUDataProvider.GetGPUUsage(),
                GPUTemperature = _GPUDataProvider.GetGPUTemperature(),
                VRAM = _GPUDataProvider.GetVRAMUsage(),

                CPUName = _CPUDataProvider.CPUName,
                CPUUsage = _CPUDataProvider.GetCPUUsage(),
                CPUTemperature = _CPUDataProvider.GetCPUTemperature(),

                RAMUsage = _RAMDataProvider.GetRAMUsage()
            };
        }
    }

}