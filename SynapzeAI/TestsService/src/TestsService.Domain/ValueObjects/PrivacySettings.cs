using CSharpFunctionalExtensions;
using TestsService.Domain.Shared;

namespace TestsService.Domain.ValueObjects;

public record PrivacySettings
{
    public bool IsPrivate { get; } = true;
    
    public List<string> UsersNamesAreAllowed { get; } = new List<string>();
    public List<string> UsersEmailsAreAllowed { get; } = new List<string>();

    private PrivacySettings(
        bool isPrivate,
        List<string> usersNamesAreAllowed, 
        List<string> usersEmailsAreAllowed)
    {
        IsPrivate = isPrivate;
        
        UsersNamesAreAllowed = usersNamesAreAllowed;
        UsersEmailsAreAllowed = usersEmailsAreAllowed;
    }

    public static Result<PrivacySettings, Error> Create(
        bool isPrivate, 
        IEnumerable<string> usersNamesAreAllowed, 
        IEnumerable<string> usersEmailsAreAllowed)
    {
        if (usersNamesAreAllowed.Any(string.IsNullOrWhiteSpace))
            return Errors.General.ValueIsInvalid(nameof(usersNamesAreAllowed));
        
        if (usersEmailsAreAllowed.Any(u => string.IsNullOrWhiteSpace(u) || EmailValidator.IsVaild(u) == false))
            return Errors.General.ValueIsInvalid(nameof(usersEmailsAreAllowed));
        
        return new PrivacySettings(isPrivate, usersNamesAreAllowed.ToList(), usersEmailsAreAllowed.ToList());
    }

    public static PrivacySettings AddDefault() =>
        new PrivacySettings(true, [], []);
}