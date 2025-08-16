using AIService.API.Middleware;
using AIService.Application;
using AIService.Infrastructure;
using AIService.Presentation.Grpc.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();

builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddFromInfrastructure(builder.Configuration)
    .AddFromApplication();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();
app.MapGrpcService<GreeterService>();
app.Run();