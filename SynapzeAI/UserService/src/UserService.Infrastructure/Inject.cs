using System.Reflection;
using Application.Abstractions;
using Elastic.CommonSchema.Serilog;
using Elastic.Ingest.Elasticsearch;
using Elastic.Ingest.Elasticsearch.DataStreams;
using Elastic.Serilog.Sinks;
using Framework.Authorization;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Events;
using Telegram.Bot;
using UserService.Application;
using UserService.Application.Abstractions;
using UserService.Application.Providers;
using UserService.Application.Repositories;
using UserService.Domain.User;
using UserService.Infrastructure.Consumers;
using UserService.Infrastructure.Options;
using UserService.Infrastructure.Providers;
using UserService.Infrastructure.Providers.Authorization;
using UserService.Infrastructure.Repositories;
using UserService.Infrastructure.Seeding;
using UserService.Presentation.Options;

namespace UserService.Infrastructure;

public static class Inject
{
    public static IServiceCollection AddFromInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddHttpClient();
        
        services.AddSingleton<TelegramBotClient>(provider =>
        {
            var telegramOptions = config.GetSection(TelegramBotOptions.BotOptions).Get<TelegramBotOptions>();
            
            return new TelegramBotClient(telegramOptions?.Token 
                                         ?? throw new ArgumentNullException("Telegram token is null"));
        });

        services.Configure<AdminOptions>(
            config.GetSection(AdminOptions.ADMIN));

        services.Configure<RefreshSessionOptions>(
            config.GetSection(RefreshSessionOptions.RefreshSession));

        services.Configure<TelegramBotOptions>(
            config.GetSection(TelegramBotOptions.BotOptions));
        
        services.AddOptions<AdminOptions>();
        services.AddOptions<RefreshSession>();
        services.AddOptions<TelegramBotOptions>();
        
        services
            .AddIdentity<User, Role>(options =>
            {
                options.User.RequireUniqueEmail = false;
                options.Password.RequiredLength = 8;
            })
            .AddEntityFrameworkStores<AccountsDbContext>();

        services.AddScoped<AccountsDbContext>();
        
        services.AddSingleton<AccountsSeeder>();
        services.AddScoped<AccountsSeederService>();
        
        services.AddTransient<ITokenProvider, JwtTokenProvider>();
        
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        services.AddScoped<IAccountRepository, AccountRepository>();

        services.AddScoped<IMessageProvider, TelegramProvider>();
        
        var authOptions = config.GetSection(AuthOptions.Auth).Get<AuthOptions>()
                          ?? throw new ApplicationException("Auth options not found");
    
        var rsaKeyProvider = new RsaKeyProvider(authOptions);
        services.AddSingleton<IKeyProvider>(rsaKeyProvider);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            var rsaKey = rsaKeyProvider.GetPublicRsa();
            var key = new RsaSecurityKey(rsaKey);

            options.TokenValidationParameters = TokenValidationParametersFactory
                .CreateWithLifeTime(key);
        });
        
        services.AddScoped<UserBoughtTheProductEventConsumer>();

        services.AddMassTransit(x =>
        {
            var options = config.GetSection(RabbitMQOptions.RabbitMQ).Get<RabbitMQOptions>()
                          ?? throw new ApplicationException("Missing RabbitMQ configuration");
            
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(new Uri(options.Host),h =>
                {
                    h.Username(options.Username);
                    h.Password(options.Password);
                });
                
                cfg.ReceiveEndpoint("queue", ep =>
                {
                    ep.PrefetchCount = 16;
                    ep.UseMessageRetry(r => r.Interval(2, 100));
                    ep.Consumer<UserBoughtTheProductEventConsumer>(context);
                });
            });
        });
        
        string indexFormat =
            $"{Assembly.GetExecutingAssembly().GetName().Name?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM-dd}";

        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.Debug()
            .WriteTo.Elasticsearch(
                [new Uri(config.GetConnectionString("Elasticsearch") 
                         ?? throw new ApplicationException("Elasticsearch connection string not found."))],
                options =>
                {
                    options.DataStream = new DataStreamName(indexFormat);
                    options.TextFormatting = new EcsTextFormatterConfiguration<LogEventEcsDocument>();
                    options.BootstrapMethod = BootstrapMethod.Silent;
                })
            .MinimumLevel.Override("Microsoft.AspNetCore.Hosting", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore.Mvc", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore.Routing", LogEventLevel.Warning)
            .CreateLogger();
        
        services.AddSerilog();
        
        services.AddControllers();

        services.AddAuthorization();

        return services;
    }
}