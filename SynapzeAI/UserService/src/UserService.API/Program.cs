using Microsoft.OpenApi.Models;
using Serilog;
using UserService.API.Middlewares;
using UserService.Application;
using UserService.Infrastructure;
using UserService.Infrastructure.Seeding;

DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.Seq(builder.Configuration.GetConnectionString("Seq") ?? throw new ArgumentNullException("Seq"))
    .CreateLogger();

builder.Services.AddSerilog();
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services
    .AddFromApplication()
    .AddFromInfrastructure(builder.Configuration);

var app = builder.Build();

var accountsSeeder = app.Services.GetRequiredService<AccountsSeeder>();
await accountsSeeder.SeedAsync();

app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(config =>
{
    config.WithOrigins("http://localhost:5173")
        .AllowCredentials()
        .AllowAnyHeader()
        .AllowAnyMethod();
});

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();