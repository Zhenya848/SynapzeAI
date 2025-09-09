using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using PaymentService.Abstractions;
using PaymentService.Contracts.Messaging;
using PaymentService.DbContexts;
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