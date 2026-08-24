using FusionHUD.Monitoring.Interfaces;
using FusionHUD.Monitoring.Models;

namespace FusionHUD.Monitoring.Services
{
    public sealed class StatisticsService : IStatisticsService
    {
        private double _CpuUsageTotal;
        private int _CpuUsageSamples;

        private double _CpuTemperatureTotal;
        private int _CpuTemperatureSamples;

        private double _GpuUsageTotal;
        private int _GpuUsageSamples;

        private double _GpuTemperatureTotal;
        private int _GpuTemperatureSamples;

        private double _RamUsageTotal;
        private int _RamUsageSamples;

        private double _FpsTotal;
        private int _FpsSamples;

        public DailyStatistics Statistics { get; } = new();

        public StatisticsState State
        {
            get
            {
                return new StatisticsState
                {
                    Date = Statistics.StartTime.Date,

                    Statistics = Statistics,

                    CpuUsageTotal = _CpuUsageTotal,
                    CpuUsageSamples = _CpuUsageSamples,

                    CpuTemperatureTotal = _CpuTemperatureTotal,
                    CpuTemperatureSamples = _CpuTemperatureSamples,

                    GpuUsageTotal = _GpuUsageTotal,
                    GpuUsageSamples = _GpuUsageSamples,

                    GpuTemperatureTotal = _GpuTemperatureTotal,
                    GpuTemperatureSamples = _GpuTemperatureSamples,

                    RamUsageTotal = _RamUsageTotal,
                    RamUsageSamples = _RamUsageSamples,

                    FpsTotal = _FpsTotal,
                    FpsSamples = _FpsSamples
                };
            }
        }

        public void Update(PerformanceSample Sample)
        {
            UpdateCpuUsage(Sample.CpuUsage);

            UpdateCpuTemperature(Sample.CpuTemperature);

            UpdateGpuUsage(Sample.GpuUsage);

            UpdateGpuTemperature(Sample.GpuTemperature);

            UpdateRamUsage(Sample.RamUsage);

            UpdateFps(Sample.Fps);
        }

        public void UpdateUptime()
        {
            Statistics.Uptime = DateTime.Now - Statistics.StartTime;
        }

        public void Restore(StatisticsState State)
        {
            _CpuUsageTotal = State.CpuUsageTotal;
            _CpuUsageSamples = State.CpuUsageSamples;

            _CpuTemperatureTotal = State.CpuTemperatureTotal;
            _CpuTemperatureSamples = State.CpuTemperatureSamples;

            _GpuUsageTotal = State.GpuUsageTotal;
            _GpuUsageSamples = State.GpuUsageSamples;

            _GpuTemperatureTotal = State.GpuTemperatureTotal;
            _GpuTemperatureSamples = State.GpuTemperatureSamples;

            _RamUsageTotal = State.RamUsageTotal;
            _RamUsageSamples = State.RamUsageSamples;

            _FpsTotal = State.FpsTotal;
            _FpsSamples = State.FpsSamples;

            Statistics.StartTime = State.Statistics.StartTime;
            Statistics.Uptime = State.Statistics.Uptime;

            Statistics.CpuUsageAverage = State.Statistics.CpuUsageAverage;

            Statistics.CpuUsageMaximum = State.Statistics.CpuUsageMaximum;

            Statistics.CpuTemperatureAverage = State.Statistics.CpuTemperatureAverage;

            Statistics.CpuTemperatureMaximum = State.Statistics.CpuTemperatureMaximum;

            Statistics.GpuUsageAverage = State.Statistics.GpuUsageAverage;

            Statistics.GpuUsageMaximum = State.Statistics.GpuUsageMaximum;

            Statistics.GpuTemperatureAverage = State.Statistics.GpuTemperatureAverage;

            Statistics.GpuTemperatureMaximum = State.Statistics.GpuTemperatureMaximum;

            Statistics.RamUsageAverage = State.Statistics.RamUsageAverage;

            Statistics.RamUsageMaximum = State.Statistics.RamUsageMaximum;

            Statistics.FpsAverage = State.Statistics.FpsAverage;
        }

        public void Reset()
        {
            _CpuUsageTotal = 0;
            _CpuUsageSamples = 0;

            _CpuTemperatureTotal = 0;
            _CpuTemperatureSamples = 0;

            _GpuUsageTotal = 0;
            _GpuUsageSamples = 0;

            _GpuTemperatureTotal = 0;
            _GpuTemperatureSamples = 0;

            _RamUsageTotal = 0;
            _RamUsageSamples = 0;

            _FpsTotal = 0;
            _FpsSamples = 0;

            Statistics.StartTime = DateTime.Now;
            Statistics.Uptime = TimeSpan.Zero;

            Statistics.CpuUsageAverage = 0;
            Statistics.CpuUsageMaximum = 0;

            Statistics.CpuTemperatureAverage = 0;
            Statistics.CpuTemperatureMaximum = 0;

            Statistics.GpuUsageAverage = 0;
            Statistics.GpuUsageMaximum = 0;

            Statistics.GpuTemperatureAverage = 0;
            Statistics.GpuTemperatureMaximum = 0;

            Statistics.RamUsageAverage = 0;
            Statistics.RamUsageMaximum = 0;

            Statistics.FpsAverage = 0;
        }

        private void UpdateCpuUsage(double Value)
        {
            _CpuUsageTotal += Value;

            _CpuUsageSamples++;

            Statistics.CpuUsageAverage = _CpuUsageTotal / _CpuUsageSamples;

            if (Value > Statistics.CpuUsageMaximum)
            {
                Statistics.CpuUsageMaximum = Value;
            }
        }

        private void UpdateCpuTemperature(double Value)
        {
            _CpuTemperatureTotal += Value;

            _CpuTemperatureSamples++;

            Statistics.CpuTemperatureAverage = _CpuTemperatureTotal / _CpuTemperatureSamples;

            if (Value > Statistics.CpuTemperatureMaximum)
            {
                Statistics.CpuTemperatureMaximum = Value;
            }
        }

        private void UpdateGpuUsage(double Value)
        {
            _GpuUsageTotal += Value;

            _GpuUsageSamples++;

            Statistics.GpuUsageAverage = _GpuUsageTotal / _GpuUsageSamples;

            if (Value > Statistics.GpuUsageMaximum)
            {
                Statistics.GpuUsageMaximum = Value;
            }
        }

        private void UpdateGpuTemperature(double Value)
        {
            _GpuTemperatureTotal += Value;

            _GpuTemperatureSamples++;

            Statistics.GpuTemperatureAverage = _GpuTemperatureTotal / _GpuTemperatureSamples;

            if (Value > Statistics.GpuTemperatureMaximum)
            {
                Statistics.GpuTemperatureMaximum = Value;
            }
        }

        private void UpdateRamUsage(double Value)
        {
            _RamUsageTotal += Value;

            _RamUsageSamples++;

            Statistics.RamUsageAverage = _RamUsageTotal / _RamUsageSamples;

            if (Value > Statistics.RamUsageMaximum)
            {
                Statistics.RamUsageMaximum = Value;
            }
        }

        private void UpdateFps(double Value)
        {
            if (Value <= 0)
            {
                return;
            }

            _FpsTotal += Value;

            _FpsSamples++;

            Statistics.FpsAverage = _FpsTotal / _FpsSamples;
        }
    }

}