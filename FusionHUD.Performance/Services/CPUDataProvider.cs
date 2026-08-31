using FusionHUD.Performance.Interfaces;
using FusionHUD.Performance.Native;
using System.Diagnostics;
using System.Runtime.Versioning;

namespace FusionHUD.Performance.Services
{
    /// <summary>
    /// Provides CPU utilization and temperature data for the system.
    /// </summary>
    [SupportedOSPlatform("windows")]
    public sealed class CPUDataProvider : ICPUDataProvider, IDisposable
    {
        private const int SMOOTH_SAMPLES = 5; // Number of recent samples used to smooth performance readings.

        private readonly PerformanceCounter _CPUUsageCounter;

        private readonly Queue<float> _CPUUsageHistory = new();

        private readonly Queue<float> _CPUTemperatureHistory = new();

        private readonly object _Lock = new();

        private bool _AMDInitialized;

        private bool _Disposed;

        public string CPUName { get; }

        public CPUDataProvider()
        {
            CPUName = GetCPUName();

            _CPUUsageCounter = new PerformanceCounter(categoryName: "Processor", counterName: "% Processor Time", instanceName: "_Total");

            _CPUUsageCounter.NextValue(); // The first reading is not meaningful for this counter.

            _AMDInitialized = InitializeAMD();
        }

        public float GetCPUUsage()
        {
            ThrowIfDisposed();

            try
            {
                float CPUUsage = _CPUUsageCounter.NextValue();

                if (CPUUsage < 0)
                {
                    return 0;
                }

                return AddAndAverage(_CPUUsageHistory, CPUUsage);
            }
            catch
            {
                return 0;
            }
        }

        public float GetCPUTemperature()
        {
            ThrowIfDisposed();

            if (!_AMDInitialized)
            {
                return 0;
            }

            try
            {
                float Temperature = (float)FusionHUDAMDInterop.GetCPUTemperature();

                if (Temperature <= 0 || Temperature > 150)
                {
                    return 0;
                }

                return AddAndAverage(_CPUTemperatureHistory, Temperature);
            }
            catch
            {
                _AMDInitialized = false;

                return 0;
            }
        }

        private static string GetCPUName()
        {
            using Microsoft.Win32.RegistryKey? Key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"HARDWARE\DESCRIPTION\System\CentralProcessor\0");

            string Name = Key?.GetValue("ProcessorNameString")?.ToString() ?? "N/A";

            return FormatCPUName(Name);
        }

        private static string FormatCPUName(string Name)
        {
            const string AMD_RYZEN_PREFIX = "AMD Ryzen ";

            if (Name.StartsWith(AMD_RYZEN_PREFIX, StringComparison.OrdinalIgnoreCase))
            {
                Name = Name[AMD_RYZEN_PREFIX.Length..];
            }

            int CoreIndex = Name.IndexOf(" 6-Core", StringComparison.OrdinalIgnoreCase);

            if (CoreIndex >= 0)
            {
                Name = Name[..CoreIndex];
            }

            int ProcessorIndex = Name.IndexOf(" Processor", StringComparison.OrdinalIgnoreCase);

            if (ProcessorIndex >= 0)
            {
                Name = Name[..ProcessorIndex];
            }

            return Name == "N/A" ? Name : $"R{Name}";
        }

        private static bool InitializeAMD()
        {
            try
            {
                return FusionHUDAMDInterop.InitAMDMonitor();
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Adds a new sample to the history and returns the average of the retained samples.
        /// </summary>
        private float AddAndAverage(Queue<float> History, float Value)
        {
            lock (_Lock)
            {
                History.Enqueue(Value);

                if (History.Count > SMOOTH_SAMPLES)
                {
                    History.Dequeue();
                }

                return History.Average();
            }
        }

        private void ThrowIfDisposed()
        {
            ObjectDisposedException.ThrowIf(_Disposed, this);
        }

        public void Dispose()
        {
            if (_Disposed)
            {
                return;
            }

            if (_AMDInitialized)
            {
                try
                {
                    FusionHUDAMDInterop.ShutdownAMDMonitor();
                }
                catch
                {
                    // Native shutdown failure must not prevent
                    // managed resources from being released.
                }

                _AMDInitialized = false;
            }

            _CPUUsageCounter.Dispose();

            _CPUUsageHistory.Clear();
            _CPUTemperatureHistory.Clear();

            _Disposed = true;

            GC.SuppressFinalize(this);
        }
    }
}