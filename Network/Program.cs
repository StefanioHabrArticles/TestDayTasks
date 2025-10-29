using Objects;
using Regions;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddObjectsMap();
builder.Services.Configure<ObjectsMapKeyOptions>(
    builder.Configuration.GetSection(nameof(ObjectsMapKeyOptions)));
builder.Services.AddSingleton<IConnectionMultiplexer>(_ =>
    ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis")!));

builder.Services.AddSingleton(new RegionsMapFactory(new RegionsGenerator()).Create(100, 100));

builder.Services.AddMagicOnion();

var app = builder.Build();

app.MapMagicOnionService();

app.Run();