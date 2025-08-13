using CSharpFunctionalExtensions;
using UserService.Application.Abstractions;
using UserService.Application.Repositories;
using UserService.Domain.Shared;
using UserService.Domain.User;

namespace UserService.Application.Commands.GetUser;

public class GetUserByEmailHandler : ICommandHandler<string, Result<UserInfo, ErrorList>>
{
    private readonly IAccountRepository _accountRepository;

    public GetUserByEmailHandler(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }
    
    public async Task<Result<UserInfo, ErrorList>> Handle(
        string email, 
        CancellationToken cancellationToken = default)
    {
        var userResult = await _accountRepository
            .GetUserByEmail(email, cancellationToken);

        if (userResult.IsFailure)
            return (ErrorList)userResult.Error;
        
        var result = new UserInfo()
        {
            Id = userResult.Value.Id,

            UserName = userResult.Value.UserName!,
            Email = userResult.Value.Email!
        };

        return result;
    }
}