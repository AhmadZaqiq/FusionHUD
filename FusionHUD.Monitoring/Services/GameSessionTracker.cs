using FusionHUD.Monitoring.Interfaces;
using FusionHUD.Monitoring.Models;

namespace FusionHUD.Monitoring.Services
{
    public sealed class GameSessionTracker : IGameSessionTracker
    {
        private readonly object _Lock = new();

        private bool _IsSessionActive;

        private string _GameName = string.Empty;

        private DateTime _SessionStartTime;

        private readonly List<PerformanceSample> _Samples = new();

        public GameSessionStatistics? ProcessSample(PerformanceSample Sample)
        {
            lock (_Lock)
            {
                bool GameActive = Sample.Fps > 0;

                if (!GameActive)
                {
                    if (!_IsSessionActive)
                    {
                        return null;
                    }

                    return EndSession(Sample.Timestamp);
                }

                if (!_IsSessionActive)
                {
                    StartSession(Sample);
                }

                _Samples.Add(Sample);

                return null;
            }
        }

        private void StartSession(PerformanceSample Sample)
        {
            _IsSessionActive = true;

            _GameName = Sample.GameName;

            _SessionStartTime = Sample.Timestamp;

            _Samples.Clear();
        }

        private GameSessionStatistics EndSession(DateTime EndTime)
        {
            if (_Samples.Count == 0)
            {
                ResetSession();

                return new GameSessionStatistics
                {
                    GameName = _GameName,
                    Duration = EndTime - _SessionStartTime
                };
            }

            GameSessionStatistics Statistics = new()
            {
                GameName = _GameName,

                Duration = EndTime - _SessionStartTime,

                FpsAverage = _Samples.Where(Sample => Sample.Fps > 0).Select(Sample => Sample.Fps).DefaultIfEmpty().Average(),

                CpuUsageAverage = _Samples.Average(Sample => Sample.CpuUsage),
                CpuUsageMaximum = _Samples.Max(Sample => Sample.CpuUsage),

                CpuTemperatureAverage = _Samples.Where(Sample => Sample.CpuTemperature > 0).Select(Sample => Sample.CpuTemperature).DefaultIfEmpty().Average(),

                CpuTemperatureMaximum = _Samples.Max(Sample => Sample.CpuTemperature),

                GpuUsageAverage = _Samples.Average(Sample => Sample.GpuUsage),
                GpuUsageMaximum = _Samples.Max(Sample => Sample.GpuUsage),

                GpuTemperatureAverage = _Samples.Where(Sample => Sample.GpuTemperature > 0).Select(Sample => Sample.GpuTemperature).DefaultIfEmpty().Average(),

                GpuTemperatureMaximum = _Samples.Max(Sample => Sample.GpuTemperature),

                RamUsageAverage = _Samples.Average(Sample => Sample.RamUsage),
                RamUsageMaximum = _Samples.Max(Sample => Sample.RamUsage)
            };

            ResetSession();

            return Statistics;
        }

        private void ResetSession()
        {
            _IsSessionActive = false;

            _GameName = string.Empty;

            _SessionStartTime = default;

            _Samples.Clear();
        }
    }

}