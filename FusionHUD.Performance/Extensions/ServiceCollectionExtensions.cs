using FusionHUD.Performance.Interfaces;
using FusionHUD.Performance.Providers;
using FusionHUD.Performance.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FusionHUD.Performance.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPerformanceServices(this IServiceCollection Services)
        {
            Services.AddSingleton<IGPUDataProvider, GPUDataProvider>();

            Services.AddSingleton<ICPUDataProvider, CPUDataProvider>();

            Services.AddSingleton<IRAMDataProvider, RAMDataProvider>();

            Services.AddSingleton<IFPSDataProvider, FPSDataProvider>();

            Services.AddSingleton<IPerformanceService, PerformanceService>();

            return Services;
        }
    }
}