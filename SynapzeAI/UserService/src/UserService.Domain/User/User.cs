using System.Text;
using Core;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using UserService.Domain.Shared;

namespace UserService.Domain.User;

public class User : IdentityUser<Guid>
{
    public string Name { get; private set; }
    public string Telegram { get;  private set; }
    public bool IsVerified { get; private set; } = false;
    
    private List<Role> _roles;
    public IReadOnlyList<Role> Roles => _roles;
    
    public ParticipantAccount? ParticipantAccount { get; }
    public AdminAccount? AdminAccount { get; }
    
    public int Balance { get; private set; }

    private User()
    {
        
    }

    private static User Create(string username, string uniqueUserName, string telegram, Role role)
    {
        var user = new User
        {
            Name = username,
            UserName = uniqueUserName,
            _roles = [role], 
            Telegram = telegram,
            Balance = 1
        };

        return user;
    }
    
    public void UpdateUsername(string username)
    {
        Name = username;

        var startCodeIndex = UserName!.IndexOf('_');
        
        UserName = username + UserName
            .Substring(startCodeIndex, UserName.Length - startCodeIndex);
    }
    
    public static User CreateParticipant(
        string user,
        string telegram,
        Role role)
    {
        if (role.Name != ParticipantAccount.PARTICIPANT)
            throw new ApplicationException($"Role {role.Name} does not exist");
        
        return Create(user, GenerateUniqueUserName(user), telegram, role);
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
    
    private static string GenerateUniqueUserName(string username)
    {
        var ticks = DateTime.Now.Ticks;
        
        var chars = Enumerable.Range('a', 26).Select(c => (char)c)
            .Concat(Enumerable.Range('A', 26).Select(c => (char)c))
            .Concat(Enumerable.Range('0', 10).Select(c => (char)c))
            .ToArray();

        StringBuilder sb = new StringBuilder();

        while (ticks > 0)
        {
            sb.Append(chars[ticks % chars.Length]);
            ticks /= chars.Length;
        }
        
        username += $"_{sb}{chars[new Random().Next(0, chars.Length)]}";

        return username;
    }
    
    public void VerifyUser() => 
        IsVerified = true;
    
    public UnitResult<Error> SetBalance(int balance)
    {
        if (balance < 0)
            return Error.Validation("invalid.user.balance", "Balance cannot be negative");
        
        Balance = balance;

        return Result.Success<Error>();
    }
}