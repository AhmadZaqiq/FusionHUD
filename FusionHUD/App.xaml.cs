using FusionHUD.Interfaces;
using FusionHUD.Monitoring.Extensions;
using FusionHUD.Overlay.Extensions;
using FusionHUD.Performance.Extensions;
using FusionHUD.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;

namespace FusionHUD
{
    public partial class App : Application
    {
        private IHost? _Host;

        private async void Application_Startup(object Sender, StartupEventArgs e)
        {
            HostApplicationBuilder Builder = Host.CreateApplicationBuilder();

            Builder.Services.AddPerformanceServices();

            Builder.Services.AddOverlayServices();

            Builder.Services.AddMonitoringServices();

            Builder.Services.AddSingleton<IStartupService, StartupService>();

            Builder.Services.AddSingleton<IOverlayLauncher, OverlayLauncher>();

            _Host = Builder.Build();

            await _Host.StartAsync();

            IOverlayLauncher OverlayLauncher = _Host.Services.GetRequiredService<IOverlayLauncher>();

            OverlayLauncher.Start();
        }

        private async void Application_Exit(object Sender, ExitEventArgs e)
        {
            if (_Host is not null)
            {
                await _Host.StopAsync();

                _Host.Dispose();
            }
        }
    }

}