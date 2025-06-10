using CSharpFunctionalExtensions;
using UserService.Application.Abstractions;
using UserService.Application.Repositories;
using UserService.Domain.Shared;
using UserService.Domain.User.Dtos;

namespace UserService.Application.Commands.GetInfoAboutUser;

public class GetInfoAboutUserHandler : ICommandHandler<Guid, Result<UserDto, ErrorList>>
{
    private readonly IAccountRepository _accountRepository;

    public GetInfoAboutUserHandler(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }
    
    public async Task<Result<UserDto, ErrorList>> Handle(
        Guid userId, 
        CancellationToken cancellationToken = default)
    {
        var userResult = await _accountRepository.GetInfoAboutUser(userId, cancellationToken);
        
        if (userResult.IsFailure)
            return (ErrorList)userResult.Error;

        var participant = userResult.Value.ParticipantAccount;
        var admin = userResult.Value.AdminAccount;
        
        var participantAccount = participant is null ? null : new ParticipantAccountDto()
        { 
            Nickname = participant.Nickname
        };
        
        var adminAccount = admin is null ? null : new AdminAccountDto()
        { 
            FirstName = admin.FullName.FirstName, 
            LastName = admin.FullName.LastName,
            Patronymic = admin.FullName.Patronymic,
        };
        
        var user = new UserDto()
        {
            Id = userResult.Value.Id,
            
            UserName = userResult.Value.UserName!, 
            Email = userResult.Value.Email!,
            Telegram = userResult.Value.Telegram,
            
            ParticipantAccount = participantAccount,
            AdminAccount = adminAccount
        };

        return user;
    }
}