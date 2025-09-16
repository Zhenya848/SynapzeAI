using System.Security.Cryptography;
using Framework.Authorization;
using Framework.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using TestsService.Application.Abstractions;
using TestsService.Application.Repositories;
using TestsService.Domain.Shared;
using TestsService.Infrastructure.DbContexts;
using TestsService.Infrastructure.Repositories;
using TestsService.Presentation.Authorization;

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
        
        var authOptions = configuration.GetSection(AuthOptions.Auth).Get<AuthOptions>()
                          ?? throw new ApplicationException("Auth options not found");
        
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            var rsa = RSA.Create();
        
            byte[] publicKeyBytes = File.ReadAllBytes(authOptions.PublicKeyPath);
            rsa.ImportRSAPublicKey(publicKeyBytes, out _);
            
            var key = new RsaSecurityKey(rsa);
            
            options.TokenValidationParameters = TokenValidationParametersFactory
                .CreateWithLifeTime(key);
        })
        .AddScheme<SecretKeyAuthenticationOptions, SecretKeyAuthenticationHandler>(
            SecretKeyDefaults.AuthenticationScheme, 
            options =>
        {
            options.ExpectedKey = authOptions.SecretKey;
        });
        
        return services;
    }
}