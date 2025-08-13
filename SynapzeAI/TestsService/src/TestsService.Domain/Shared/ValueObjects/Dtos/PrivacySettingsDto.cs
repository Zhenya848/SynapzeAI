namespace TestsService.Domain.Shared.ValueObjects.Dtos;

public record PrivacySettingsDto
{
    public bool IsPrivate { get; set; }
    
    public string[] UsersNamesAreAllowed { get; set; }
    public string[] UsersEmailsAreAllowed { get; set; }
}