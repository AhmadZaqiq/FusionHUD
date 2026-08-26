using FusionHUD.Monitoring.Configuration;
using FusionHUD.Monitoring.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using System.Text.Json;

namespace FusionHUD.Monitoring.Services
{
    public sealed class TelegramReportSender : IDailyReportSender
    {
        private readonly HttpClient _HttpClient;

        private readonly TelegramOptions _TelegramOptions;

        private readonly ILogger<TelegramReportSender> _Logger;

        public TelegramReportSender(HttpClient HttpClient, IOptions<TelegramOptions> TelegramOptions, ILogger<TelegramReportSender> Logger)
        {
            _HttpClient = HttpClient;

            _TelegramOptions = TelegramOptions.Value;

            _Logger = Logger;
        }

        public async Task SendAsync(string Report, CancellationToken CancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_TelegramOptions.BotToken))
            {
                throw new InvalidOperationException("Telegram:BotToken is not configured.");
            }

            if (_TelegramOptions.ChatId == 0)
            {
                throw new InvalidOperationException("Telegram:ChatId is not configured.");
            }

            string Url = $"https://api.telegram.org/bot{_TelegramOptions.BotToken}/sendMessage";

            object Request = new
            {
                chat_id = _TelegramOptions.ChatId,
                text = Report
            };

            using HttpResponseMessage Response = await _HttpClient.PostAsJsonAsync(Url, Request, CancellationToken);

            string ResponseContent = await Response.Content.ReadAsStringAsync(CancellationToken);

            if (!Response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Telegram API request failed. " + $"Status: {(int)Response.StatusCode}. " + $"Response: {ResponseContent}");
            }

            using JsonDocument Json = JsonDocument.Parse(ResponseContent);

            if (!Json.RootElement.TryGetProperty("ok", out JsonElement Ok) || !Ok.GetBoolean())
            {
                throw new InvalidOperationException($"Telegram API rejected the request: {ResponseContent}");
            }
        }
    }

}