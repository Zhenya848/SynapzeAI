using UserService.Domain.Shared;

namespace UserService.Domain.User;

public class AdminAccount
{
    public const string ADMIN = "Admin";
    public FullName FullName { get; set; }
    
    public Guid Id { get; set; }
    
    public Guid UserId { get; set; }
    public User User { get; set; }

    private AdminAccount()
    {
        
    }
    
    public static AdminAccount CreateAdmin(FullName fullName, User user)
    {
        return new AdminAccount()
        {
            Id = Guid.NewGuid(),
            
            FullName = fullName,
            User = user
        };
    }
}