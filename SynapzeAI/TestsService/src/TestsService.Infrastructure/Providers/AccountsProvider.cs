using System.Text;
using CSharpFunctionalExtensions;
using Grpc.Net.Client;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using TestService.Presentation;
using TestsService.Application.Providers;
using TestsService.Domain.Shared;
using TestsService.Domain.Shared.ValueObjects;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace TestsService.Infrastructure.Providers;

public class AccountsProvider : IAccountsProvider
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<AccountsProvider> _logger;
    
    private readonly JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings
    {
        NullValueHandling = NullValueHandling.Ignore,
        ContractResolver = new CamelCasePropertyNamesContractResolver()
    };

    public AccountsProvider(ILogger<AccountsProvider> logger, HttpClient httpClient)
    {
        _logger = logger;
        _httpClient = httpClient;
    }

    public async Task<Result<UserInfo[], Error>> GetUsers(
        IEnumerable<Guid> userIds,
        CancellationToken cancellationToken = default)
    {
        try
        {
            using var channel = GrpcChannel.ForAddress("http://localhost:5275");
            var client = new Greeter.GreeterClient(channel);
            
            var request = new GetUsersRequest();
            request.UserIds.AddRange(userIds.Select(i => i.ToString()));
            
            var response = await client.GetUsersAsync(request);

            var users = response.Users
                .Select(u => new UserInfo() { Id = Guid.Parse(u.Id), UserName = u.UserName, Email = u.Email })
                .ToArray();

            return users;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            
            return Error.Failure("get.users.failure", "Cannot get users, see log errors");
        }
    }

    public async Task<Result<UserInfo, Error>> GetUserByEmail(
        string email, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            using var channel = GrpcChannel.ForAddress("http://localhost:5275");
            var client = new Greeter.GreeterClient(channel);
            
            var request = new GetUserByEmailRequest { Email = email };
            
            var response = await client.GetUserByEmailAsync(request);
            var user = response.User;
            
            var result = new UserInfo() { Id = Guid.Parse(user.Id), UserName = user.UserName, Email = user.Email };

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            
            return Error.Failure("get.user.failure", "Cannot get user, see log errors");
        }
    }
}