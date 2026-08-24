namespace FusionHUD.Performance.Models
{
    public class PerformanceSnapshot
    {
        public float FPS { get; set; }

        public string GPUName { get; set; } = string.Empty;
        public float GPUUsage { get; set; }
        public float GPUTemperature { get; set; }
        public double VRAM { get; set; }

        public string CPUName { get; set; } = string.Empty;
        public float CPUUsage { get; set; }
        public float CPUTemperature { get; set; }

        public double RAMUsage { get; set; }
    }

}