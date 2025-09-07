using Microsoft.AspNetCore.Identity;
using UserService.Domain.Shared;

namespace UserService.Domain.User;

public class User : IdentityUser<Guid>
{
    public string UniqueUserName { get; private set; }
    public string Telegram { get;  private set; }
    public bool IsVerified { get; private set; } = false;
    
    private List<Role> _roles;
    public IReadOnlyList<Role> Roles => _roles;
    
    public ParticipantAccount? ParticipantAccount { get; }
    public AdminAccount? AdminAccount { get; }

    private User()
    {
        
    }

    private static User Create(string username, string uniqueUserName, string telegram, Role role)
    {
        var user = new User
        {
            UserName = username,
            UniqueUserName = uniqueUserName,
            _roles = [role], 
            Telegram = telegram
        };

        return user;
    }
    
    public void UpdateUsername(string username)
    {
        UserName = username;

        var startCodeIndex = UniqueUserName.IndexOf('_');
        
        UniqueUserName = username + UniqueUserName
            .Substring(startCodeIndex,  UniqueUserName.Length - startCodeIndex);
    }
    
    public static User CreateParticipant(
        string user,
        string telegram,
        Role role,
        long usersCount)
    {
        if (role.Name != ParticipantAccount.PARTICIPANT)
            throw new ApplicationException($"Role {role.Name} does not exist");
        
        return Create(user, GenerateUniqueUserName(user, usersCount), telegram, role);
    }
    
    public static User CreateAdmin(
        string user,
        string telegram,
        Role role)
    {
        if (role.Name != AdminAccount.ADMIN)
            throw new ApplicationException($"Role {role.Name} does not exist");
        
        return Create(user, "ADMIN", telegram, role);
    }
    
    private static string GenerateUniqueUserName(string username, long usersCount)
    {
        var chars = Enumerable.Range('a', 26).Select(c => (char)c)
            .Concat(Enumerable.Range('A', 26).Select(c => (char)c))
            .Concat(Enumerable.Range('0', 10).Select(c => (char)c))
            .ToArray();

        username += "_";

        for (var i = 0; i < Constants.USER_UNIQUE_CODE_LENGTH; i++)
            username += chars[(usersCount / (int)Math.Pow(chars.Length, i)) % chars.Length];

        return username;
    }
    
    public void VerifyUser() => 
        IsVerified = true;
}