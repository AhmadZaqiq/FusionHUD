using FusionHUD.Performance.Interfaces;
using FusionHUD.Performance.Models;

namespace FusionHUD.Performance.Services
{
    public sealed class PerformanceService : IPerformanceService
    {
        private readonly ICPUDataProvider _CPUDataProvider;

        private readonly IGPUDataProvider _GPUDataProvider;

        private readonly IRAMDataProvider _RAMDataProvider;

        private readonly IFPSDataProvider _FPSDataProvider;

        public PerformanceService(ICPUDataProvider CPUDataProvider, IGPUDataProvider GPUDataProvider, IRAMDataProvider RAMDataProvider, IFPSDataProvider FPSDataProvider)
        {
            _CPUDataProvider = CPUDataProvider;

            _GPUDataProvider = GPUDataProvider;

            _RAMDataProvider = RAMDataProvider;

            _FPSDataProvider = FPSDataProvider;
        }

        public PerformanceSnapshot GetPerformanceSnapshot()
        {
            FPSData FPSData = _FPSDataProvider.GetFPSData();

            return new PerformanceSnapshot
            {
                CPUName = _CPUDataProvider.CPUName,
                CPUUsage = _CPUDataProvider.GetCPUUsage(),
                CPUTemperature = _CPUDataProvider.GetCPUTemperature(),

                GPUName = _GPUDataProvider.GPUName,
                GPUUsage = _GPUDataProvider.GetGPUUsage(),
                GPUTemperature = _GPUDataProvider.GetGPUTemperature(),
                VRAM = _GPUDataProvider.GetVRAMUsage(),

                RAMUsage = _RAMDataProvider.GetRAMUsage(),

                FPS = FPSData.FPS,
                GameName = FPSData.GameName
            };
        }
    }
}