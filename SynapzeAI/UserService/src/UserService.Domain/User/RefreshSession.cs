namespace UserService.Domain.User;

public class RefreshSession
{
    public User User { get; set; }
    
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public Guid RefreshToken { get; init; }
    public Guid Jti { get; init; }
    
    public DateTime CreatedAt { get; init; }
    public DateTime ExpiresIn { get; init; }
}