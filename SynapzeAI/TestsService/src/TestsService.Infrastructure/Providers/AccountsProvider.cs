using System.Text;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
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
            var request = JsonSerializer.Serialize(userIds);
            var content = new StringContent(request, Encoding.UTF8, "application/json");

            var response =
                await _httpClient.PostAsync("http://localhost:5276/api/Account/users", content, cancellationToken);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

            var users = JsonConvert.DeserializeObject<UserInfo[]>(responseBody, _jsonSerializerSettings);

            if (users is null)
                return Error.Failure("parse.user.failure", "Cannot parse user data");

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
            var response = await _httpClient
                .GetAsync($"http://localhost:5276/api/Account/user/{email}", cancellationToken);

            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

            var user = JsonConvert.DeserializeObject<UserInfo>(responseBody, _jsonSerializerSettings);

            if (user is null)
                return Error.Failure("parse.user.failure", "Cannot parse user data");

            return user;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            
            return Error.Failure("get.user.failure", "Cannot get user, see log errors");
        }
    }
}