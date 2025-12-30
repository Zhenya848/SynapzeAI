using System.Text.Json;
using Core;
using CSharpFunctionalExtensions;
using Framework.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types;
using UserService.Application.Providers;
using UserService.Domain.Shared;
using UserService.Infrastructure.Options;

namespace UserService.Infrastructure.Providers;

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
    
    public Result<ChatInfoDto, Error> GetChatInfo(JsonElement json)
    {
        var update = json.ConvertObjectToType<Update>();
        
        if (update.IsFailure)
            return update.Error;
        
        var username = update.Value.Message?.From?.Username;
        
        if (username == null)
            return Error.Failure("telegram.username.not.found", "Telegram username not found");
        
        var chatId = update.Value.Message?.Chat.Id;
        
        if (chatId.HasValue == false)
            return Error.Failure("telegram.chat.id.not.found", "Telegram chat id not found");
        
        var chatInfo = new ChatInfoDto(chatId.Value, username);
        
        return chatInfo;
    }

    public async Task<UnitResult<Error>> SendCode(ChatInfoDto chatInfo, string code)
    {
        try
        {
            var message = $"🔐 **Код подтверждения**\n\n" +
                          $"`{code}`\n\n" +
                          $"⏳ Код действителен 5 минут\n" +
                          $"⚠️ Никому не сообщайте этот код";
            
            await _botClient.SendTextMessageAsync(
                chatId: chatInfo.ChatId,
                text: message,
                parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown
            );
            
            _logger.LogInformation("Код {Code} отправлен пользователю {Username}", code, chatInfo.Username);
            
            return Result.Success<Error>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка отправки кода пользователю {Username}", chatInfo.Username);
            return Error.Failure("send.code.failure", "Failed to send code");
        }
    }
}