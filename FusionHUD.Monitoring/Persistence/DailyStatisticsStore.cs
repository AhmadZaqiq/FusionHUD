using System.Text.Json;
using FusionHUD.Monitoring.Interfaces;
using FusionHUD.Monitoring.Models;

namespace FusionHUD.Monitoring.Persistence
{
    public sealed class DailyStatisticsStore : IDailyStatisticsStore
    {
        private readonly string _FilePath;

        private readonly JsonSerializerOptions _JsonOptions =
            new()
            {
                WriteIndented = true
            };

        public DailyStatisticsStore(string FilePath)
        {
            _FilePath = FilePath;
        }

        public async Task<StatisticsState?> LoadAsync(CancellationToken CancellationToken = default)
        {
            if (!File.Exists(_FilePath))
            {
                return null;
            }

            try
            {
                await using FileStream Stream = File.OpenRead(_FilePath);

                return await JsonSerializer.DeserializeAsync<StatisticsState>(Stream, _JsonOptions, CancellationToken);
            }
            catch (JsonException)
            {
                return null;
            }
            catch (IOException)
            {
                return null;
            }
        }

        public async Task SaveAsync(StatisticsState State, CancellationToken CancellationToken = default)
        {
            string? DirectoryPath = Path.GetDirectoryName(_FilePath);

            if (!string.IsNullOrEmpty(DirectoryPath))
            {
                Directory.CreateDirectory(DirectoryPath);
            }

            string TemporaryFilePath = _FilePath + ".tmp";

            await using (FileStream Stream = File.Create(TemporaryFilePath))
            {
                await JsonSerializer.SerializeAsync(Stream, State, _JsonOptions, CancellationToken);

                await Stream.FlushAsync(CancellationToken);
            }

            File.Move(TemporaryFilePath, _FilePath, true);
        }

        public Task DeleteAsync(CancellationToken CancellationToken = default)
        {
            if (File.Exists(_FilePath))
            {
                File.Delete(_FilePath);
            }

            return Task.CompletedTask;
        }
    }

}