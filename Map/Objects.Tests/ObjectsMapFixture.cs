using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using Testcontainers.Redis;

namespace Objects.Tests;

public class ObjectsMapFixture : IAsyncLifetime
{
    private readonly RedisContainer _redisContainer = new RedisBuilder()
        .WithImage("redis:7.0")
        .Build();

    public ServiceProvider ServiceProvider { get; private set; } = null!;

    public async Task InitializeAsync()
    {
        await _redisContainer.StartAsync();
        ServiceProvider = BuildServiceProvider();
    }

    public async Task DisposeAsync()
    {
        await ServiceProvider.DisposeAsync();
        await _redisContainer.StopAsync();
        await _redisContainer.DisposeAsync();
    }

    private ServiceProvider BuildServiceProvider()
    {
        var services = new ServiceCollection().AddObjectsMap();

        services.AddSingleton<IConnectionMultiplexer>(_ =>
            ConnectionMultiplexer.Connect(_redisContainer.GetConnectionString()));
        services.AddSingleton(Options.Create(new ObjectsMapKeyOptions()));

        return services.BuildServiceProvider();
    }
}
