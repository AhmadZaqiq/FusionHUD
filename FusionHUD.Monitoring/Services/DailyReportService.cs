using FusionHUD.Monitoring.Interfaces;
using FusionHUD.Monitoring.Models;

namespace FusionHUD.Monitoring.Services
{
    public sealed class DailyReportService : IDailyReportService
    {
        public string CreateReport(DailyStatistics Statistics)
        {
            string CpuTemperatureMaximum = Statistics.CpuTemperatureMaximum > 0 ? $"{Statistics.CpuTemperatureMaximum:F0}°C" : "N/A";

            string CpuTemperatureAverage = Statistics.CpuTemperatureAverage > 0 ? $"{Statistics.CpuTemperatureAverage:F0}°C" : "N/A";

            string GpuTemperatureMaximum = Statistics.GpuTemperatureMaximum > 0 ? $"{Statistics.GpuTemperatureMaximum:F0}°C" : "N/A";

            string GpuTemperatureAverage = Statistics.GpuTemperatureAverage > 0 ? $"{Statistics.GpuTemperatureAverage:F0}°C" : "N/A";

            string FpsAverage = Statistics.FpsAverage > 0 ? $"{Statistics.FpsAverage:F0}" : "N/A";

            string Report = $"""
                📊 FusionHUD Daily Report

                📅 Date: {Statistics.StartTime:dd/MM/yyyy}
                ⏱ Uptime: {Statistics.Uptime:hh\:mm\:ss}

                🖥 CPU
                Average Usage: {Statistics.CpuUsageAverage:F0}%
                Maximum Usage: {Statistics.CpuUsageMaximum:F0}%
                Max Temperature: {CpuTemperatureMaximum}
                Average Temperature: {CpuTemperatureAverage}

                🎮 GPU
                Average Usage: {Statistics.GpuUsageAverage:F0}%
                Maximum Usage: {Statistics.GpuUsageMaximum:F0}%
                Max Temperature: {GpuTemperatureMaximum}
                Average Temperature: {GpuTemperatureAverage}

                💾 RAM
                Average: {Statistics.RamUsageAverage:F1} GB
                Maximum: {Statistics.RamUsageMaximum:F1} GB

                🎯 FPS
                Average: {FpsAverage}
                """;

            return Report;
        }
    }

}