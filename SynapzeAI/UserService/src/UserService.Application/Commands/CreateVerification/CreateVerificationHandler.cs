using System.Text.Json;
using Application.Abstractions;
using Core;
using CSharpFunctionalExtensions;
using UserService.Application.Providers;
using UserService.Application.Repositories;
using UserService.Domain;
using UserService.Domain.Shared;

namespace UserService.Application.Commands.CreateVerification;

public class CreateVerificationHandler : ICommandHandler<JsonElement, Result<Guid, ErrorList>>
{
    private readonly IMessageProvider _messageProvider;
    private readonly IAccountRepository _accountRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    public CreateVerificationHandler(IMessageProvider messageProvider, 
        IAccountRepository accountRepository, 
        IUnitOfWork unitOfWork)
    {
        _messageProvider = messageProvider;
        _accountRepository = accountRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result<Guid, ErrorList>> Handle(
        JsonElement json, 
        CancellationToken cancellationToken = default)
    {
        var chatInfoResult = _messageProvider.GetChatInfo(json);
        
        if (chatInfoResult.IsFailure)
            return (ErrorList)chatInfoResult.Error;

        var chatInfo = chatInfoResult.Value;
        
        var existsVerification = await _accountRepository
            .GetVerificationByUsername(chatInfo.Username, cancellationToken);

        if (existsVerification.IsSuccess)
            return (ErrorList)Errors.User.AlreadyExist();
        
        using var transaction = await _unitOfWork.BeginTransaction(cancellationToken);
        
        var code = GenerateCode();
        var expirationDate = DateTime.UtcNow.AddMinutes(VerificationConstants.ExpiresMinutes);
        
        var verificationResult = Verification.Create(chatInfo.Username, code, expirationDate);
        
        if (verificationResult.IsFailure)
            return (ErrorList)verificationResult.Error;
        
        var verification = verificationResult.Value;

        _accountRepository.CreateVerification(verification);
        await _unitOfWork.SaveChanges(cancellationToken);
        
        var sendCodeResult = await _messageProvider.SendCode(chatInfo, code);

        if (sendCodeResult.IsFailure)
        {
            transaction.Rollback();
            return (ErrorList)sendCodeResult.Error;
        }
        
        transaction.Commit();
        
        return verification.Id;
    }
    
    private string GenerateCode() => new Random().Next(10000, 99999).ToString();
}