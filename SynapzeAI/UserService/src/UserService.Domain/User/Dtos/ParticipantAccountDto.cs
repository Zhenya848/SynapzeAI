namespace UserService.Domain.User.Dtos;

public record ParticipantAccountDto
{
    public string Nickname { get; set; } = default!;
}