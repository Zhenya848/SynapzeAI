using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserService.Application;
using UserService.Application.Abstractions;
using UserService.Application.Repositories;
using UserService.Domain.User;
using UserService.Infrastructure.Options;
using UserService.Infrastructure.Repositories;
using UserService.Infrastructure.Seeding;
using UserService.Presentation;
using UserService.Presentation.Options;

namespace UserService.Infrastructure;

public static class Inject
{
    public static IServiceCollection AddFromInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<JwtOptions>(
            config.GetSection(JwtOptions.JWT));
        
        services.Configure<AdminOptions>(
            config.GetSection(AdminOptions.ADMIN));

        services.Configure<RefreshSessionOptions>(
            config.GetSection(RefreshSessionOptions.RefreshSession));

        services.AddOptions<JwtOptions>();
        services.AddOptions<AdminOptions>();
        services.AddOptions<RefreshSession>();
        
        services
            .AddIdentity<User, Role>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = 8;
            })
            .AddEntityFrameworkStores<AccountsDbContext>();

        services.AddScoped<AccountsDbContext>();
        
        services.AddSingleton<AccountsSeeder>();
        services.AddScoped<AccountsSeederService>();
        
        services.AddTransient<ITokenProvider, JwtTokenProvider>();
        
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        services.AddScoped<IAccountRepository, AccountRepository>();
        
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                var jwtOptions = config.GetSection(JwtOptions.JWT).Get<JwtOptions>()
                                 ?? throw new ApplicationException("Missing JWT configuration");

                options.TokenValidationParameters = TokenValidationParametersFactory
                    .CreateWithLifeTime(jwtOptions);
            });

        services.AddAuthorization();

        return services;
    }
}