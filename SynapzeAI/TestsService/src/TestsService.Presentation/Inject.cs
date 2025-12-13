using Grpc.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TestsService.Application.Abstractions;
using TestsService.Presentation.Ai;
using TestsService.Presentation.Authorization;
using TestsService.Presentation.Grpc.Interceptors;
using TestsService.Presentation.Grpc.Services;
using UserService.Presentation;

namespace TestsService.Presentation;

public static class Inject
{
    public static IServiceCollection AddFromPresentation(
        this IServiceCollection services,
        IConfiguration config)
    {
        services.Configure<AuthOptions>(config.GetSection(AuthOptions.Auth));
        services.Configure<AiOptions>(config.GetSection(AiOptions.Ai));
        
        services.AddOptions<AuthOptions>();
        services.AddOptions<AiOptions>();
        
        services.AddSingleton<ProvideSecretKeyInterceptor>();
        
        services.AddGrpcClient<Greeter.GreeterClient>(options =>
        {
            options.Address = new Uri("http://userservice-api:8081");
        })
        .AddInterceptor<ProvideSecretKeyInterceptor>();
        
        services.AddScoped<IGreeterService, GreeterService>();

        return services;
    }
}