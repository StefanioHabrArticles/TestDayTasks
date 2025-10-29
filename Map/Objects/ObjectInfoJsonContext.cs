using System.Text.Json.Serialization;

namespace Objects;

[JsonSerializable(typeof(ObjectInfo))]
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
internal partial class ObjectInfoJsonContext : JsonSerializerContext;