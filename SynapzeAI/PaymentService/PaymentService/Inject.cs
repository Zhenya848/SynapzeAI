using System.Security.Cryptography;
using Framework.Authorization;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using PaymentService.Abstractions;
using PaymentService.DbContexts;
using PaymentService.Models.Shared;
using PaymentService.Options;
using PaymentService.Outbox;
using PaymentService.Seeding;
using Quartz;

namespace PaymentService;

public static class Inject
{
    public static IServiceCollection AddServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<RabbitMQOptions>(
            configuration.GetSection(RabbitMQOptions.RabbitMQ));

        services.AddOptions<RabbitMQOptions>();
        
        services.AddHttpContextAccessor();
        
        services.AddScoped<AppDbContext>();
        services.AddScoped<ProcessOutboxMessagesService>();
        
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.Configure<YandexKassaOptions>(
            configuration.GetSection(YandexKassaOptions.YANDEX));

        services.AddOptions<YandexKassaOptions>();
        
        services.AddScoped<ProductsSeeder>();
        
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

        services.AddMassTransit(configure =>
        {
            var options = configuration.GetSection(RabbitMQOptions.RabbitMQ).Get<RabbitMQOptions>()
                          ?? throw new ApplicationException("Missing RabbitMQ configuration");

            configure.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(new Uri(options.Host), h =>
                {
                    h.Username(options.Username);
                    h.Password(options.Password);
                });
            });
            
            services.AddControllers();
        });

        services.AddQuartz(c =>
        {
            var jobKey = new JobKey(nameof(ProcessOutboxMessagesJob));

            c.AddJob<ProcessOutboxMessagesJob>(jobKey)
                .AddTrigger(t => t.ForJob(jobKey)
                    .WithSimpleSchedule(s => s.WithIntervalInSeconds(3).RepeatForever()));
        });

        services.AddQuartzHostedService(o => { o.WaitForJobsToComplete = true; });
        
        return services;
    }
}