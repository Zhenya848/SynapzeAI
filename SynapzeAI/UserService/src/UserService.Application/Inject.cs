using Application.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace UserService.Application;

public static class Inject
{
    public static IServiceCollection AddFromApplication(this IServiceCollection services)
    {
        return services.Scan(scan => scan.FromAssemblies(typeof(Inject).Assembly)
            .AddClasses(classes => classes.AssignableToAny(
                typeof(ICommandHandler<,>),
                typeof(IQueryHandler<,>)))
            .AsSelfWithInterfaces()
            .WithLifetime(ServiceLifetime.Scoped));
    }
}