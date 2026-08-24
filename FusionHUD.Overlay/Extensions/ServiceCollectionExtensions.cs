using FusionHUD.Overlay.Interfaces;
using FusionHUD.Overlay.Services;
using FusionHUD.Overlay.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace FusionHUD.Overlay.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddOverlayServices(this IServiceCollection Services)
        {
            Services.AddSingleton<IOverlaySettingsService, OverlaySettingsService>();

            Services.AddSingleton<IOverlayPositionService, OverlayPositionService>();

            Services.AddSingleton<IOverlaySizeService, OverlaySizeService>();

            Services.AddSingleton<IOverlayColorService, OverlayColorService>();

            Services.AddSingleton<IHotkeyService, HotkeyService>();

            Services.AddSingleton<IOverlayAlignmentService, OverlayAlignmentService>();

            Services.AddSingleton<IOverlayClickThroughService, OverlayClickThroughService>();

            Services.AddSingleton<MainWindow>();

            Services.AddSingleton<MainViewModel>();

            return Services;
        }
    }

}