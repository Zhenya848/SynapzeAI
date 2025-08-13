using AIService.Application.Commands.Generate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace AIService.Application;

public static class Inject
{
    public static IServiceCollection AddFromApplication(
        this IServiceCollection services)
    {
        services.AddScoped<GenerateContentHandler>();
        
        return services;
    }
}