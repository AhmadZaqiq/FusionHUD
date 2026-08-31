using FusionHUD.Monitoring.Interfaces;
using FusionHUD.Monitoring.Models;

namespace FusionHUD.Monitoring.Services
{
    public sealed class GameSessionReportService : IGameSessionReportService
    {
        public string CreateReport(GameSessionStatistics Statistics)
        {
            string CPUName = string.IsNullOrWhiteSpace(Statistics.CPUName) ? "CPU" : Statistics.CPUName;

            string GPUName = FormatGPUName(Statistics.GPUName);

            string FpsAverage = Statistics.FpsAverage > 0 ? $"{Statistics.FpsAverage:F0} FPS" : "N/A";

            string CpuTemperatureAverage = Statistics.CpuTemperatureAverage > 0 ? $"{Statistics.CpuTemperatureAverage:F0}°C" : "N/A";

            string CpuTemperatureMaximum = Statistics.CpuTemperatureMaximum > 0 ? $"{Statistics.CpuTemperatureMaximum:F0}°C" : "N/A";

            string GpuTemperatureAverage = Statistics.GpuTemperatureAverage > 0 ? $"{Statistics.GpuTemperatureAverage:F0}°C" : "N/A";

            string GpuTemperatureMaximum = Statistics.GpuTemperatureMaximum > 0 ? $"{Statistics.GpuTemperatureMaximum:F0}°C" : "N/A";

            return $"""
                🎮 <b>FusionHUD</b>
                <i>Game Session</i>

                🎯 <b>{Statistics.GameName}</b>
                ⏱ Duration: <b>{FormatDuration(Statistics.Duration)}</b>

                ━━━━━━━━━━━━━━━━━━
                📊 <b>PERFORMANCE</b>

                🎯 <b>FPS</b>
                Avg: <b>{FpsAverage}</b>

                🖥 <b>{CPUName}</b>
                Avg Usage: <b>{Statistics.CpuUsageAverage:F0}%</b>
                Max Usage: {Statistics.CpuUsageMaximum:F0}%
                Avg Temperature: <b>{CpuTemperatureAverage}</b>
                Max Temperature: {CpuTemperatureMaximum}

                🎮 <b>{GPUName}</b>
                Avg Usage: <b>{Statistics.GpuUsageAverage:F0}%</b>
                Max Usage: {Statistics.GpuUsageMaximum:F0}%
                Avg Temperature: <b>{GpuTemperatureAverage}</b>
                Max Temperature: {GpuTemperatureMaximum}

                💾 <b>RAM</b>
                Avg Usage: <b>{Statistics.RamUsageAverage:F1} GB</b>
                Max Usage: {Statistics.RamUsageMaximum:F1} GB
                ━━━━━━━━━━━━━━━━━━

                ✅ <i>Session completed successfully.</i>
                """;
        }

        private static string FormatGPUName(string Name)
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                return "GPU";
            }

            if (Name.StartsWith("NVIDIA ", StringComparison.OrdinalIgnoreCase))
            {
                Name = Name["NVIDIA ".Length..];
            }

            if (Name.StartsWith("GeForce ", StringComparison.OrdinalIgnoreCase))
            {
                Name = Name["GeForce ".Length..];
            }

            return Name;
        }

        private static string FormatDuration(TimeSpan Duration)
        {
            if (Duration.TotalHours >= 1)
            {
                return $"{(int)Duration.TotalHours}h {Duration.Minutes}m {Duration.Seconds}s";
            }

            return $"{Duration.Minutes}m {Duration.Seconds}s";
        }
    }
}