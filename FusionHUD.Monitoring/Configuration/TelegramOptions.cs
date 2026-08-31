namespace FusionHUD.Monitoring.Configuration
{
    public sealed class TelegramOptions
    {
        public string BotToken { get; set; } = string.Empty;

        public long ChatId { get; set; }
    }
}