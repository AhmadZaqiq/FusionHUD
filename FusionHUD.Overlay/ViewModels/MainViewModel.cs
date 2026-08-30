using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FusionHUD.Overlay.Interfaces;
using FusionHUD.Performance.Interfaces;
using System.Windows.Threading;

namespace FusionHUD.Overlay.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly IPerformanceService _PerformanceService;

        private readonly IOverlaySettingsService _SettingsService;

        private readonly IOverlayWindowService _WindowService;

        private readonly DispatcherTimer _UpdateTimer;

        [ObservableProperty]
        private string _OverlayText = string.Empty;

        public MainViewModel(IPerformanceService PerformanceService, IOverlaySettingsService SettingsService, IOverlayWindowService WindowService)
        {
            _PerformanceService = PerformanceService;

            _SettingsService = SettingsService;

            _WindowService = WindowService;

            _UpdateTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(500)
            };

            _UpdateTimer.Tick += UpdateTimer_Tick;

            UpdateOverlay();

            _UpdateTimer.Start();
        }

        [RelayCommand]
        private void ToggleVisibility()
        {
            _WindowService.ToggleVisibility();
        }

        [RelayCommand]
        private void ChangePosition()
        {
            _SettingsService.MoveToNextPosition();

            _WindowService.ApplySettings();
        }

        [RelayCommand]
        private void ChangeSize()
        {
            _SettingsService.MoveToNextSize();

            _WindowService.ApplySettings();
        }

        [RelayCommand]
        private void ChangeColor()
        {
            _SettingsService.MoveToNextColor();

            _WindowService.ApplySettings();
        }

        private void UpdateTimer_Tick(object? Sender, EventArgs e)
        {
            UpdateOverlay();
        }

        private void UpdateOverlay()
        {
            var Snapshot = _PerformanceService.GetPerformanceSnapshot();

            string FPSDisplay = Snapshot.FPS <= 0 ? "N/A" : $"{Snapshot.FPS:F0}";
            string GPUUsageDisplay = Snapshot.GPUUsage < 0 ? "N/A" : $"{Snapshot.GPUUsage:F1}%";
            string GPUTemperatureDisplay = Snapshot.GPUTemperature <= 0 ? "N/A" : $"{Snapshot.GPUTemperature:F0}°C";
            string VRAMDisplay = Snapshot.VRAM < 0 ? "N/A" : $"{Snapshot.VRAM:F1} GB";
            string CPUUsageDisplay = Snapshot.CPUUsage < 0 ? "N/A" : $"{Snapshot.CPUUsage:F1}%";
            string CPUTemperatureDisplay = Snapshot.CPUTemperature <= 0 ? "N/A" : $"{Snapshot.CPUTemperature:F0}°C";
            string RAMDisplay = Snapshot.RAMUsage <= 0 ? "N/A" : $"{Snapshot.RAMUsage:F1} GB";

            OverlayText =
                $"FPS {FPSDisplay} | " +
                $"{Snapshot.GPUName} {GPUUsageDisplay} {GPUTemperatureDisplay} {VRAMDisplay} | " +
                $"{Snapshot.CPUName} {CPUUsageDisplay} {CPUTemperatureDisplay} | " +
                $"RAM {RAMDisplay}";
        }
    }

}