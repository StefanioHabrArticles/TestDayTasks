using Microsoft.Extensions.DependencyInjection;

namespace Objects;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddObjectsMap(this IServiceCollection services)
    {
        services.AddSingleton<IObjectsMapKeyProvider, ObjectsMapKeyProvider>();
        services.AddSingleton<ICoordinatesConverter, CoordinatesConverter>();

        services.AddSingleton<ObjectsMap>();
        return services;
    }
}