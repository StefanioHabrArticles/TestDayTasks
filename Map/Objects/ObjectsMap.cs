using System.Text.Json;
using StackExchange.Redis;

namespace Objects;

public class ObjectsMap(
    IObjectsMapKeyProvider keyProvider,
    ICoordinatesConverter coordinatesConverter,
    IConnectionMultiplexer redis)
{
    private readonly IDatabase _db = redis.GetDatabase();

    public event EventHandler<ObjectAddedEventArgs>? ObjectAdded;
    public event EventHandler<ObjectDeletedEventArgs>? ObjectDeleted;

    protected virtual void OnObjectAdded(ObjectInfo objectInfo)
    {
        ObjectAdded?.Invoke(this, new ObjectAddedEventArgs(objectInfo));
    }

    protected virtual void OnObjectDeleted(uint id)
    {
        ObjectDeleted?.Invoke(this, new ObjectDeletedEventArgs(id));
    }

    public async Task<ObjectInfo?> Get(uint id)
    {
        var key = keyProvider.GetKey(id);
        var json = await _db.StringGetAsync(key);

        return json.IsNullOrEmpty
            ? null
            : JsonSerializer.Deserialize(json.ToString(), ObjectInfoJsonContext.Default.ObjectInfo);
    }

    public async Task<ObjectInfo?> Get(int x, int y)
    {
        var geoKey = keyProvider.GetGeoKey();

        var geoCoordinates = coordinatesConverter.ToGeo(x, y);
        var longitude = geoCoordinates[0];
        var latitude = geoCoordinates[1];

        var results = await _db.GeoSearchAsync(
            geoKey,
            longitude,
            latitude,
            new GeoSearchCircle(0.001),
            count: 1,
            order: Order.Ascending);

        if (results.Length == 0)
            return null;

        var objectId = uint.Parse(results.Single().Member.ToString());
        return await Get(objectId);
    }

    public async Task<List<ObjectInfo>> Get(int x1, int y1, int x2, int y2)
    {
        var geoKey = keyProvider.GetGeoKey();

        var centerX = (x1 + x2) / 2;
        var centerY = (y1 + y2) / 2;
        var center = coordinatesConverter.ToGeo(centerX, centerY);

        var results = await _db.GeoSearchAsync(
            geoKey,
            center[0],
            center[1],
            new GeoSearchBox(x2 - x1, y2 - y1),
            order: Order.Ascending);

        List<ObjectInfo> objectInfos = [];
        foreach (var result in results)
        {
            var objectId = uint.Parse(result.Member.ToString());
            var objectInfo = await Get(objectId);
            if (objectInfo is null)
                continue;
            objectInfos.Add(objectInfo);
        }
        return objectInfos;
    }

    public async Task Add(ObjectInfo objectInfo)
    {
        var key = keyProvider.GetKey(objectInfo.Id);
        var geoKey = keyProvider.GetGeoKey();
        var json = JsonSerializer.Serialize(objectInfo, ObjectInfoJsonContext.Default.ObjectInfo);

        var geoCoordinates = coordinatesConverter.ToGeo(objectInfo.TopLeftX, objectInfo.TopLeftY);
        var longitude = geoCoordinates[0];
        var latitude = geoCoordinates[1];

        var transaction = _db.CreateTransaction();
        await transaction.StringSetAsync(key, json);
        await transaction.GeoAddAsync(geoKey, longitude, latitude, objectInfo.Id.ToString());
        var result = await transaction.ExecuteAsync();

        if (result)
        {
            OnObjectAdded(objectInfo);
        }
    }

    public async Task Delete(uint id)
    {
        var key = keyProvider.GetKey(id);
        var geoKey = keyProvider.GetGeoKey();

        var transaction = _db.CreateTransaction();
        await transaction.KeyDeleteAsync(key);
        await transaction.GeoRemoveAsync(geoKey, id.ToString());
        var result = await transaction.ExecuteAsync();

        if (result)
        {
            OnObjectDeleted(id);
        }
    }
}