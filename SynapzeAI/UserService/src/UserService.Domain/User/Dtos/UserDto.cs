namespace UserService.Domain.User.Dtos;

public record UserDto()
{
    public Guid Id { get; set; }
    
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Telegram { get; set; }

    public ParticipantAccountDto? ParticipantAccount { get; set; }
    public AdminAccountDto? AdminAccount { get; set; }
}