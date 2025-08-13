namespace UserService.Domain.User;

public record UserInfo()
{
    public Guid Id { get; set; }
    
    public string UserName { get; set; }
    public string Email { get; set; }
}