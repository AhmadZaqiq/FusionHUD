namespace FusionHUD.Performance.Interfaces
{
    public interface IGPUDataProvider
    {
        string GPUName { get; }

        float GetGPUUsage();

        float GetGPUTemperature();

        double GetVRAMUsage();
    }
}