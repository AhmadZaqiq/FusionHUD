namespace FusionHUD.Performance.Interfaces
{
    public interface ICPUDataProvider
    {
        string CPUName { get; }

        float GetCPUUsage();

        float GetCPUTemperature();
    }
}