using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Objects.Tests;

public class ObjectsMapFixture(RedisContainerFixture redisContainerFixture) : IAsyncLifetime
{
    public ServiceProvider ServiceProvider { get; private set; } = null!;

    public Task InitializeAsync()
    {
        ServiceProvider = BuildServiceProvider();
        return Task.CompletedTask;
    }

    public async Task DisposeAsync()
    {
        await ServiceProvider.DisposeAsync();
    }

    private ServiceProvider BuildServiceProvider()
    {
        var services = new ServiceCollection().AddObjectsMap();

        services.AddSingleton<IConnectionMultiplexer>(_ =>
            ConnectionMultiplexer.Connect(redisContainerFixture.Container.GetConnectionString()));
        services.AddSingleton(Options.Create(new ObjectsMapKeyOptions()));

        return services.BuildServiceProvider();
    }
}
