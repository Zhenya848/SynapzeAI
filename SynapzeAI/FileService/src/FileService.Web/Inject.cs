using System.Reflection;
using FileService.Web.Abstractions;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace FileService.Web;

public static class Inject
{
    public static IServiceCollection AddEndpoints(this IServiceCollection services)
    {
        services.AddEndpoints(Assembly.GetExecutingAssembly());
        
        return services;
    }
    
    public static IApplicationBuilder MapEndpoints(this WebApplication app, RouteGroupBuilder? routeGroupBuilder = null)
    {
        var endpoints = app.Services.GetRequiredService<IEnumerable<IEndpoint>>();

        IEndpointRouteBuilder builder = routeGroupBuilder is null ? app : routeGroupBuilder;
        
        foreach (var endpoint in endpoints)
        {
            endpoint.MapEndpoint(builder);
        }
        
        return app;
    }

    private static IServiceCollection AddEndpoints(this IServiceCollection services, Assembly assembly)
    {
        var servicesDescriptors = assembly
            .DefinedTypes
            .Where(type => type is { IsAbstract: false, IsInterface: false } && type.IsAssignableTo(typeof(IEndpoint)))
            .Select(type => ServiceDescriptor.Transient(typeof(IEndpoint), type));
        
        services.TryAddEnumerable(servicesDescriptors);
        
        return services;
    }
}