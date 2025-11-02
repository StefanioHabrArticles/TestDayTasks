using AwesomeAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace Objects.Tests;

[Collection(nameof(ObjectsMapCollection))]
public class ObjectsMapTests(ObjectsMapFixture fixture) : IClassFixture<ObjectsMapFixture>
{
    private readonly ServiceProvider _serviceProvider = fixture.ServiceProvider;

    [Fact]
    public async Task AddAsync_ValidObjectInfo_ObjectIsStored()
    {
        // Arrange
        var objectsMap = _serviceProvider.GetRequiredService<ObjectsMap>();
        var objectInfo = new ObjectInfo(1, 10, 20, 5, 5);

        // Act
        await objectsMap.Add(objectInfo);
        var result = await objectsMap.Get(1);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(1u);
        result.TopLeftX.Should().Be(10);
        result.TopLeftY.Should().Be(20);
        result.Width.Should().Be(5);
        result.Height.Should().Be(5);
    }

    [Fact]
    public async Task Get_ExistingId_ReturnsObjectInfo()
    {
        // Arrange
        var objectsMap = _serviceProvider.GetRequiredService<ObjectsMap>();
        var objectInfo = new ObjectInfo(2, 15, 25, 10, 10);
        await objectsMap.Add(objectInfo);

        // Act
        var result = await objectsMap.Get(2);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(2u);
        result.TopLeftX.Should().Be(15);
        result.TopLeftY.Should().Be(25);
    }

    [Fact]
    public async Task Get_NonExistingId_ReturnsNull()
    {
        // Arrange
        var objectsMap = _serviceProvider.GetRequiredService<ObjectsMap>();

        // Act
        var result = await objectsMap.Get(999);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task Get_ByCoordinates_ReturnsNearbyObject()
    {
        // Arrange
        var objectsMap = _serviceProvider.GetRequiredService<ObjectsMap>();
        var objectInfo = new ObjectInfo(3, 30, 40, 8, 8);
        await objectsMap.Add(objectInfo);

        // Act
        var result = await objectsMap.Get(33, 44);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(3u);
        result.TopLeftX.Should().Be(30);
        result.TopLeftY.Should().Be(40);
    }

    [Fact]
    public async Task Get_ByCoordinatesNoMatch_ReturnsNull()
    {
        // Arrange
        var objectsMap = _serviceProvider.GetRequiredService<ObjectsMap>();

        // Act
        var result = await objectsMap.Get(1000, 1000);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task Get_ByArea_ReturnsObjectsInRectangle()
    {
        // Arrange
        var objectsMap = _serviceProvider.GetRequiredService<ObjectsMap>();
        var objectInfo1 = new ObjectInfo(4, 50, 50, 5, 5);
        var objectInfo2 = new ObjectInfo(5, 60, 60, 5, 5);
        var objectInfo3 = new ObjectInfo(6, 100, 100, 5, 5);

        await objectsMap.Add(objectInfo1);
        await objectsMap.Add(objectInfo2);
        await objectsMap.Add(objectInfo3);

        // Act
        var results = await objectsMap.Get(45, 45, 70, 70);

        // Assert
        results.Should().NotBeNull();
        results.Count.Should().Be(2);
        results.Any(o => o.Id == 4).Should().BeTrue();
        results.Any(o => o.Id == 5).Should().BeTrue();
        results.Any(o => o.Id == 6).Should().BeFalse();
    }

    [Fact]
    public async Task Get_ByEmptyArea_ReturnsEmptyList()
    {
        // Arrange
        var objectsMap = _serviceProvider.GetRequiredService<ObjectsMap>();

        // Act
        var results = await objectsMap.Get(500, 500, 600, 600);

        // Assert
        results.Should().NotBeNull();
        results.Count.Should().Be(0);
    }

    [Fact]
    public async Task Delete_ExistingObject_RemovesObject()
    {
        // Arrange
        var objectsMap = _serviceProvider.GetRequiredService<ObjectsMap>();
        var objectInfo = new ObjectInfo(7, 70, 80, 6, 6);
        await objectsMap.Add(objectInfo);

        // Act
        await objectsMap.Delete(objectInfo.Id);
        var result = await objectsMap.Get(7);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task Delete_ExistingObject_RemovesGeoData()
    {
        // Arrange
        var objectsMap = _serviceProvider.GetRequiredService<ObjectsMap>();
        var objectInfo = new ObjectInfo(8, 90, 100, 7, 7);
        await objectsMap.Add(objectInfo);

        // Act
        await objectsMap.Delete(objectInfo.Id);
        var result = await objectsMap.Get(90, 100);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task AddAsync_MultipleObjects_AllObjectsAreStored()
    {
        // Arrange
        var objectsMap = _serviceProvider.GetRequiredService<ObjectsMap>();
        var objectInfo1 = new ObjectInfo(9, 110, 120, 4, 4);
        var objectInfo2 = new ObjectInfo(10, 130, 140, 4, 4);

        // Act
        await objectsMap.Add(objectInfo1);
        await objectsMap.Add(objectInfo2);

        var result1 = await objectsMap.Get(9);
        var result2 = await objectsMap.Get(10);

        // Assert
        result1.Should().NotBeNull();
        result1.Id.Should().Be(9u);
        result2.Should().NotBeNull();
        result2.Id.Should().Be(10u);
    }
}