using UserService.Application.Abstractions;
using UserService.Application.Repositories;
using UserService.Domain.User.Dtos;

namespace UserService.Application.Commands.GetUsers;

public class GetUsersHandler : ICommandHandler<GetUsersCommand, IEnumerable<UserDto>>
{
    private readonly IAccountRepository _accountRepository;

    public GetUsersHandler(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }
    
    public async Task<IEnumerable<UserDto>> Handle(
        GetUsersCommand command, 
        CancellationToken cancellationToken = default)
    {
        var users = await _accountRepository
            .GetUsers(command.Users, command.Roles, cancellationToken);
        
        var usersDto = users.Select(u => new UserDto()
        {
            Id = u.Id,

            UserName = u.UserName!,
            Email = u.Email!,

            AdminAccount = u.AdminAccount != null
                ? new AdminAccountDto()
                {
                    FirstName = u.AdminAccount.FullName.FirstName,
                    LastName = u.AdminAccount.FullName.LastName,
                    Patronymic = u.AdminAccount.FullName.Patronymic
                }
                : null,

            ParticipantAccount = u.ParticipantAccount != null
                ? new ParticipantAccountDto()
                {
                    Nickname = u.ParticipantAccount.Nickname
                }
                : null
        });
        
        return usersDto;
    }
}