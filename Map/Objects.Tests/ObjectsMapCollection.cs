namespace Objects.Tests;

[CollectionDefinition(nameof(ObjectsMapCollection))]
public class ObjectsMapCollection : ICollectionFixture<RedisContainerFixture>;