using Grpc.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TestsService.Application.Abstractions;
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
        services.Configure<AuthOptions>(
            config.GetSection(AuthOptions.Auth));
        
        services.AddOptions<AuthOptions>();
        
        services.AddSingleton<ProvideSecretKeyInterceptor>();
        
        services.AddGrpcClient<Greeter.GreeterClient>(options =>
        {
            options.Address = new Uri("http://localhost:5275");
        })
        .AddInterceptor<ProvideSecretKeyInterceptor>();
        
        services.AddScoped<IGreeterService, GreeterService>();

        return services;
    }
}