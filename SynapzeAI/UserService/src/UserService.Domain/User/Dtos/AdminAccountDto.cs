namespace UserService.Domain.User.Dtos;

public record AdminAccountDto
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Patronymic { get; set; } = default!;
}