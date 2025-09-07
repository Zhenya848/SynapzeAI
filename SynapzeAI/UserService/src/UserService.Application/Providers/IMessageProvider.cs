using CSharpFunctionalExtensions;
using UserService.Domain.Shared;

namespace UserService.Application.Providers;

public interface IMessageProvider
{
    public Task<UnitResult<Error>> SendCode(string code, string userMessageName);
}