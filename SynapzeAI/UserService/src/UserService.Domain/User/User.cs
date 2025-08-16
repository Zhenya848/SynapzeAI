using Microsoft.AspNetCore.Identity;

namespace UserService.Domain.User;

public class User : IdentityUser<Guid>
{
    public string UniqueUserName { get; private set; }
    
    private List<Role> _roles;
    public IReadOnlyList<Role> Roles => _roles;
    
    public ParticipantAccount? ParticipantAccount { get; }
    public AdminAccount? AdminAccount { get; }

    private User()
    {
        
    }

    public static User Create(string username, string uniqueUserName, string email, Role role)
    {
        return new User { UserName = username, UniqueUserName = uniqueUserName, _roles = [role], Email = email };
    }
    
    public void UpdateUsername(string username)
    {
        UserName = username;

        var startCodeIndex = UniqueUserName.IndexOf('_');
        
        UniqueUserName = username + UniqueUserName.Substring(startCodeIndex,  UniqueUserName.Length - startCodeIndex);
    }
    
    public static User CreateParticipant(
        string user,
        string uniqueUserName,
        string email,
        Role role)
    {
        if (role.Name != ParticipantAccount.PARTICIPANT)
            throw new ApplicationException($"Role {role.Name} does not exist");
        
        return Create(user, uniqueUserName, email, role);
    }
    
    public static User CreateAdmin(
        string user,
        string email,
        Role role)
    {
        if (role.Name != AdminAccount.ADMIN)
            throw new ApplicationException($"Role {role.Name} does not exist");
        
        return Create(user, "ADMIN", email, role);
    }
}