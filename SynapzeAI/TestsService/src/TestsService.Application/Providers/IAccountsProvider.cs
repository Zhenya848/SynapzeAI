using CSharpFunctionalExtensions;
using TestsService.Domain.Shared;
using TestsService.Domain.Shared.ValueObjects;

namespace TestsService.Application.Providers;

public interface IAccountsProvider
{
    public Task<Result<UserInfo[], Error>> GetUsers(
        IEnumerable<Guid> userIds,
        CancellationToken cancellationToken = default);

    public Task<Result<UserInfo, Error>> GetUserByEmail(
        string email,
        CancellationToken cancellationToken = default);
}