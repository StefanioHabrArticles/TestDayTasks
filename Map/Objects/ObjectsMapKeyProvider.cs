using Microsoft.Extensions.Options;

namespace Objects;

internal sealed class ObjectsMapKeyProvider(
    IOptions<ObjectsMapKeyOptions> options) : IObjectsMapKeyProvider
{
    public string GetKey(uint id) => $"{options.Value.Prefix}:{id}";

    public string GetGeoKey() => options.Value.GeoPrefix;
}