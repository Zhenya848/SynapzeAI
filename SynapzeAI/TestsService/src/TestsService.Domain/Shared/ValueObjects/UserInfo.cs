namespace TestsService.Domain.Shared.ValueObjects;

public record UserInfo()
{
    public Guid Id { get; set; }
    
    public string UserName { get; set; }
    public string Email { get; set; }
}