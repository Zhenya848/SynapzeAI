namespace UserService.Infrastructure.Options;

public record TelegramBotOptions
{
    public const string BotOptions = "TelegramBotOptions";
    public string Token { get; init; }
}