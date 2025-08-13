using UserService.Application.Abstractions;
using UserService.Application.Repositories;
using UserService.Domain;
using UserService.Domain.User;

namespace UserService.Application.Commands.GetUsers;

public class GetUsersHandler : ICommandHandler<IEnumerable<Guid>, IEnumerable<UserInfo>>
{
    private readonly IAccountRepository _accountRepository;

    public GetUsersHandler(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }
    
    public async Task<IEnumerable<UserInfo>> Handle(
        IEnumerable<Guid> userIds, 
        CancellationToken cancellationToken = default)
    {
        var users = await _accountRepository
            .GetUsers(userIds, cancellationToken);
        
        var result = users.Select(u => new UserInfo()
        {
            Id = u.Id,

            UserName = u.UserName!,
            Email = u.Email!
        });

        return result;
    }
}