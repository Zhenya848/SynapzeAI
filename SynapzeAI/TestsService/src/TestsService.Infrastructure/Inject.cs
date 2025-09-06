using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TestsService.Application.Abstractions;
using TestsService.Application.Repositories;
using TestsService.Infrastructure.DbContexts;
using TestsService.Infrastructure.Options;
using TestsService.Infrastructure.Repositories;
using TestsService.Presentation;
using TestsService.Presentation.Options;

namespace TestsService.Infrastructure;

public static class Inject
{
    public static IServiceCollection AddFromInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<IReadDbContext, ReadDbContext>();
        
        services.AddScoped<AppDbContext>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ITestRepository, TestRepository>();

        services.Configure<YandexKassaOptions>(
            configuration.GetSection(YandexKassaOptions.YANDEX));

        services.AddOptions<YandexKassaOptions>();
        
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            var jwtOptions = configuration.GetSection(JwtOptions.JWT).Get<JwtOptions>()
                             ?? throw new ApplicationException("Missing JWT configuration");

            options.TokenValidationParameters = TokenValidationParametersFactory
                .CreateWithLifeTime(jwtOptions);
        });
        
        return services;
    }
}