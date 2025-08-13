using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using TestsService.Application.Abstractions;
using TestsService.Application.Messaging;
using TestsService.Application.Providers;
using TestsService.Application.Repositories;
using TestsService.Domain.Shared.ValueObjects.Dtos;
using TestsService.Infrastructure.BackgroundServices;
using TestsService.Infrastructure.DbContexts;
using TestsService.Infrastructure.MessageQueue;
using TestsService.Infrastructure.Options;
using TestsService.Infrastructure.Providers;
using TestsService.Infrastructure.Repositories;
using TestsService.Presentation;
using TestsService.Presentation.Options;
using FileInfo = TestsService.Domain.Shared.ValueObjects.FileInfo;

namespace TestsService.Infrastructure;

public static class Inject
{
    public static IServiceCollection AddFromInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddHttpClient();
        
        services.AddScoped<IReadDbContext, ReadDbContext>();
        
        services.AddScoped<AppDbContext>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ITestRepository, TestRepository>();
        
        services.AddScoped<IAIProvider, AIProvider>();
        services.AddScoped<IAccountsProvider, AccountsProvider>();

        services.AddHostedService<FileCleanerBackgroundService>();
        services.AddMinio(configuration);

        services.AddScoped<IFileProvider, MinioProvider>();

        services.AddSingleton<IMessageQueue<IEnumerable<FileInfo>>,
            InMemoryMessageQueue<IEnumerable<FileInfo>>>();
        
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

    private static IServiceCollection AddMinio(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        return services.AddMinio(o =>
        {
            MinioOptions minioOptions = configuration.GetSection("Minio").Get<MinioOptions>()
                                        ?? throw new ApplicationException("Minio options is missing.");

            o.WithEndpoint(minioOptions.Endpoint);
            o.WithCredentials(minioOptions.AccessKey, minioOptions.SecretKey);

            o.WithSSL(minioOptions.WithSsl);
        });
    }
}