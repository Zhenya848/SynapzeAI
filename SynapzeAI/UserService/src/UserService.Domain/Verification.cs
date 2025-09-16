using CSharpFunctionalExtensions;
using UserService.Domain.Shared;
using DomainUser = UserService.Domain.User.User;
using Core;

namespace UserService.Domain;

public class Verification : Core.Entity<Guid>
{
    public Guid UserId { get; private set; }
    public DomainUser User { get; init; }
    
    public string Code { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    public int Attempts { get; private set; }
    
    private Verification(Guid id) : base(id)
    {
        
    }

    private Verification(Guid id, Guid userId, string code, DateTime expiresAt) : base(id)
    {
        UserId = userId;
        Code = code;
        ExpiresAt = expiresAt;
    }
    
    public static Result<Verification, Error> Create(Guid userId, string code, DateTime expiresAt)
    {
        if (string.IsNullOrWhiteSpace(code))
            return Errors.General.ValueIsRequired(nameof(code));
        
        if (expiresAt <= DateTime.UtcNow)
            return Errors.General.ValueIsInvalid(nameof(expiresAt));
        
        return new Verification(Guid.NewGuid(), userId, code, expiresAt);
    }
    
    public Result<string, Error> Verify(string code)
    {
        if (DateTime.UtcNow > ExpiresAt)
            return Error.Failure("verification.expired", "Код истек");
        
        Attempts++;
        
        if (Code != code)
        {
            if (Attempts >= VerificationConstants.MAX_ATTEMPTS)
                return Error.Failure("verification.blocked", "Слишком много попыток. Запросите новый код.");
            
            return Error.Failure("verification.invalid", 
                $"Неверный код. Осталось попыток: {VerificationConstants.MAX_ATTEMPTS - Attempts}");
        }
        
        return Code;
    }
}