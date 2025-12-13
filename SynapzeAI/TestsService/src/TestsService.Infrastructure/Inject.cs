using System.Reflection;
using System.Security.Cryptography;
using Application.Abstractions;
using Elastic.CommonSchema.Serilog;
using Elastic.Ingest.Elasticsearch;
using Elastic.Ingest.Elasticsearch.DataStreams;
using Elastic.Serilog.Sinks;
using Framework.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using Serilog;
using Serilog.Events;
using TestsService.Application.Abstractions;
using TestsService.Application.Providers;
using TestsService.Application.Repositories;
using TestsService.Infrastructure.DbContexts;
using TestsService.Infrastructure.Providers;
using TestsService.Infrastructure.Repositories;
using TestsService.Presentation.Authorization;
using File = System.IO.File;
using Log = Serilog.Log;

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
        services.AddSingleton<IAiProvider, AiProvider>();
        
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
            });
        
        string indexFormat =
            $"{Assembly.GetExecutingAssembly().GetName().Name?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM-dd}";

        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.Debug()
            .WriteTo.Elasticsearch(
                [new Uri(configuration.GetConnectionString("Elasticsearch") 
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

        services.AddOpenTelemetry()
            .WithMetrics(c => c
                .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("SynapzeAI.API"))
                .AddMeter("SynapzeAI")
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddRuntimeInstrumentation()
                .AddProcessInstrumentation()
                .AddPrometheusExporter());
        
        return services;
    }
}