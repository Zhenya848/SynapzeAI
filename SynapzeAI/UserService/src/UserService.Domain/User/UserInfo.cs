namespace UserService.Domain.User;

public record UserInfo()
{
    public Guid Id { get; set; }
    
    public string UserName { get; set; }
    public string UniqueUserName { get; set; }
    
    public string Telegram { get; set; }
}