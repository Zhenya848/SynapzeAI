namespace UserService.Infrastructure.Options;

public class AdminOptions
{
    public const string ADMIN = "ADMIN";
    
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Patronymic { get; init; } = string.Empty;
    
    public string Telegram { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}