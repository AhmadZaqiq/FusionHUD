using FusionHUD.Performance.Interfaces;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Threading;

namespace FusionHUD.Overlay.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly IPerformanceService _PerformanceService;

        private readonly DispatcherTimer _UpdateTimer;

        private string _OverlayText = string.Empty;

        public MainViewModel(IPerformanceService PerformanceService)
        {
            _PerformanceService = PerformanceService;

            _UpdateTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(500)
            };

            _UpdateTimer.Tick += UpdateTimer_Tick;

            UpdateOverlay();

            _UpdateTimer.Start();
        }

        public string OverlayText
        {
            get
            {
                return _OverlayText;
            }

            private set
            {
                if (_OverlayText == value)
                {
                    return;
                }

                _OverlayText = value;

                OnPropertyChanged();
            }
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

            string CPUUsageDisplay = Snapshot.CPUUsage <= 0 ? "N/A" : $"{Snapshot.CPUUsage:F1}%";

            string CPUTemperatureDisplay = Snapshot.CPUTemperature <= 0 ? "N/A" : $"{Snapshot.CPUTemperature:F0}°C";

            string RAMDisplay = Snapshot.RAMUsage <= 0
                ? "N/A"
                : $"{Snapshot.RAMUsage:F1} GB";

            OverlayText = $"FPS {FPSDisplay} | " +
                $"{Snapshot.GPUName} {GPUUsageDisplay} {GPUTemperatureDisplay} {VRAMDisplay} | " +
                $"{Snapshot.CPUName} {CPUUsageDisplay} {CPUTemperatureDisplay} | " +
                $"RAM {RAMDisplay}";
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string? PropertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
    }

}