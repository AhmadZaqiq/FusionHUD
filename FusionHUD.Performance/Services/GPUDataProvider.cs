using FusionHUD.Performance.Interfaces;
using NvAPIWrapper.GPU;
using System.Runtime.Versioning;

namespace FusionHUD.Performance.Providers
{
    [SupportedOSPlatform("windows")]
    public sealed class GPUDataProvider : IGPUDataProvider
    {
        private const int SMOOTH_SAMPLES = 5;

        private readonly PhysicalGPU? _GraphicsCard;

        private readonly Queue<float> _GPUUsageHistory = new();

        private readonly Queue<float> _GPUTemperatureHistory = new();

        private readonly object _Lock = new();

        public string GPUName { get; }

        public GPUDataProvider()
        {
            try
            {
                PhysicalGPU[] GPUs = PhysicalGPU.GetPhysicalGPUs();

                _GraphicsCard = GPUs.FirstOrDefault();

                GPUName = _GraphicsCard is null? "N/A": FormatGPUName(_GraphicsCard.FullName);
            }
            catch
            {
                _GraphicsCard = null;

                GPUName = "N/A";
            }
        }

        public float GetGPUUsage()
        {
            if (_GraphicsCard is null)
            {
                return 0;
            }

            try
            {
                float GPUUsage =_GraphicsCard.UsageInformation.GPU.Percentage;

                if (GPUUsage < 0 || GPUUsage > 100)
                {
                    return 0;
                }

                return AddAndAverage(_GPUUsageHistory,GPUUsage);
            }
            catch
            {
                return 0;
            }
        }

        public float GetGPUTemperature()
        {
            if (_GraphicsCard is null)
            {
                return 0;
            }

            try
            {
                GPUThermalSensor? Sensor =_GraphicsCard.ThermalInformation.ThermalSensors.FirstOrDefault();

                if (Sensor is null)
                {
                    return 0;
                }

                float Temperature = Sensor.CurrentTemperature;

                if (Temperature <= 0 || Temperature > 150)
                {
                    return 0;
                }

                return AddAndAverage(_GPUTemperatureHistory,Temperature);
            }
            catch
            {
                return 0;
            }
        }

        public double GetVRAMUsage()
        {
            if (_GraphicsCard is null)
            {
                return 0;
            }

            try
            {
                GPUMemoryInformation Memory =_GraphicsCard.MemoryInformation;

                double TotalGB =Memory.DedicatedVideoMemoryInkB/ 1024.0/ 1024.0;

                double AvailableGB =Memory.CurrentAvailableDedicatedVideoMemoryInkB/ 1024.0/ 1024.0;

                double UsedGB = TotalGB - AvailableGB;

                return UsedGB < 0 ? 0 : UsedGB;
            }
            catch
            {
                return 0;
            }
        }

        private static string FormatGPUName(string Name)
        {
            if (Name.StartsWith("NVIDIA ",StringComparison.OrdinalIgnoreCase))
            {
                Name = Name["NVIDIA ".Length..];
            }

            if (Name.StartsWith("GeForce ",StringComparison.OrdinalIgnoreCase))
            {
                Name = Name["GeForce ".Length..];
            }

            return Name;
        }

        private float AddAndAverage(Queue<float> History,float Value)
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
    }

}