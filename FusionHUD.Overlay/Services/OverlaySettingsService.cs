using FusionHUD.Overlay.Interfaces;
using FusionHUD.Overlay.Models;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FusionHUD.Overlay.Services
{
    public sealed class OverlaySettingsService : IOverlaySettingsService
    {
        private readonly string _SettingsFilePath;

        private readonly JsonSerializerOptions _JsonOptions =
            new()
            {
                WriteIndented = true,
                Converters =
                {
                    new JsonStringEnumConverter()
                }
            };

        public OverlaySettings Settings { get; }

        public OverlaySettingsService()
        {
            string AppDataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "FusionHUD");

            Directory.CreateDirectory(AppDataFolder);

            _SettingsFilePath = Path.Combine(AppDataFolder, "settings.json");

            Settings = _LoadSettings();
        }

        public void SetPosition(OverlayPosition Position)
        {
            Settings.Position = Position;

            _SaveSettings();
        }

        public void SetSize(OverlaySize Size)
        {
            Settings.Size = Size;

            _SaveSettings();
        }

        public void SetColor(OverlayColor Color)
        {
            Settings.Color = Color;

            _SaveSettings();
        }

        public void MoveToNextPosition()
        {
            Settings.Position = Settings.Position switch
            {
                OverlayPosition.Left => OverlayPosition.Center,
                OverlayPosition.Center => OverlayPosition.Right,
                OverlayPosition.Right => OverlayPosition.Left,
                _ => OverlayPosition.Left
            };

            _SaveSettings();
        }

        public void MoveToNextSize()
        {
            Settings.Size = Settings.Size switch
            {
                OverlaySize.Small => OverlaySize.Medium,
                OverlaySize.Medium => OverlaySize.Large,
                OverlaySize.Large => OverlaySize.Small,
                _ => OverlaySize.Small
            };

            _SaveSettings();
        }

        public void MoveToNextColor()
        {
            Settings.Color = Settings.Color switch
            {
                OverlayColor.White => OverlayColor.LightGray,
                OverlayColor.LightGray => OverlayColor.Gray,
                OverlayColor.Gray => OverlayColor.White,
                _ => OverlayColor.White
            };

            _SaveSettings();
        }

        private OverlaySettings _LoadSettings()
        {
            try
            {
                if (!File.Exists(_SettingsFilePath))
                {
                    return new OverlaySettings();
                }

                string Json = File.ReadAllText(_SettingsFilePath);

                return JsonSerializer.Deserialize<OverlaySettings>(Json, _JsonOptions) ?? new OverlaySettings();
            }
            catch
            {
                return new OverlaySettings();
            }
        }

        private void _SaveSettings()
        {
            try
            {
                string Json = JsonSerializer.Serialize(Settings, _JsonOptions);

                File.WriteAllText(_SettingsFilePath, Json);
            }
            catch
            {
            }
        }
    }
}