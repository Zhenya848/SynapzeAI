using AIService.Application.Provideers;
using AIService.Domain.Shared;
using AIService.Infrastructure.Providers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AIService.Infrastructure;

public static class Inject
{
    public static IServiceCollection AddFromInfrastructure(
        this IServiceCollection services, IConfiguration config)
    {
        services.AddHttpClient();
        
        services.Configure<APIKeys>(
            config.GetSection(APIKeys.API_KEYS));
        
        services.AddOptions<APIKeys>();
        
        services.AddScoped<IAIProvider, AIProvider>();
        
        return services;
    }
}