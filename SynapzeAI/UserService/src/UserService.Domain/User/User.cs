using Microsoft.AspNetCore.Identity;

namespace UserService.Domain.User;

public class User : IdentityUser<Guid>
{
    private List<Role> _roles;
    public IReadOnlyList<Role> Roles => _roles;
    
    public ParticipantAccount? ParticipantAccount { get; }
    public AdminAccount? AdminAccount { get; }

    private User()
    {
        
    }

    public static User Create(string username, string email, Role role) =>
        new User { UserName = username, _roles = [role], Email = email };
    
    public static User CreateParticipant(
        string user,
        string email,
        Role role)
    {
        if (role.Name != ParticipantAccount.PARTICIPANT)
            throw new ApplicationException($"Role {role.Name} does not exist");
        
        return Create(user, email, role);
    }
    
    public static User CreateAdmin(
        string user,
        string email,
        Role role)
    {
        if (role.Name != AdminAccount.ADMIN)
            throw new ApplicationException($"Role {role.Name} does not exist");
        
        return Create(user, email, role);
    }
}