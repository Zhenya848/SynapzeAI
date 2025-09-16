using System.ComponentModel.DataAnnotations;
using System.Text;
using Core;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types;
using UserService.Application.Providers;
using UserService.Domain.Shared;
using UserService.Infrastructure.Options;

namespace UserService.Infrastructure.Providers;

record SendCodeResult(bool Success, string ErrorMessage = null);

record SendCodeRequest(string Username, int Code);

record SendCodeResponse(bool Success, string Message, string Error);

public class TelegramProvider : IMessageProvider
{
    private readonly TelegramBotClient _botClient;
    private readonly ILogger<TelegramProvider> _logger;

    public TelegramProvider(
        IOptions<TelegramBotOptions> options,
        ILogger<TelegramProvider> logger)
    {
        _logger = logger;
        _botClient = new TelegramBotClient(options.Value.Token);
    }

    private async Task<long?> GetChatIdByUsernameAsync(string username)
    {
        try
        {
            var cleanUsername = username.StartsWith('@') ? username[1..] : username;
            var updates = await _botClient.GetUpdatesAsync(limit: 20);
            
            foreach (var update in updates)
            {
                if (update.Message?.From?.Username != null &&
                    update.Message.From.Username.Equals(cleanUsername, StringComparison.OrdinalIgnoreCase))
                {
                    return update.Message.Chat.Id;
                }
            }
            
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка поиска chatId для {Username}", username);
            return null;
        }
    }

    public async Task<UnitResult<Error>> SendCode(string code, string userMessageName)
    {
        try
        {
            var chatId = await GetChatIdByUsernameAsync(userMessageName);
            
            if (!chatId.HasValue)
                return Error.Failure("find.chat.failure", "Failed to find chat");
            
            var message = $"🔐 **Код подтверждения**\n\n" +
                          $"`{code}`\n\n" +
                          $"⏳ Код действителен 5 минут\n" +
                          $"⚠️ Никому не сообщайте этот код";
            
            await _botClient.SendTextMessageAsync(
                chatId: chatId.Value,
                text: message,
                parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown
            );

            _logger.LogInformation("Код {Code} отправлен пользователю {Username}", code, userMessageName);
            
            return Result.Success<Error>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка отправки кода пользователю {Username}", userMessageName);
            return Error.Failure("send.code.failure", "Failed to send code");
        }
    }
}