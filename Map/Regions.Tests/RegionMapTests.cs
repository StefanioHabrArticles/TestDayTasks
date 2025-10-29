using AwesomeAssertions;

namespace Regions.Tests;

public class RegionMapTests
{
    private static RegionsMap CreateTestMap()
    {
        // Create a 10x10 map with 4 regions in a 2x2 grid
        // Region 1: (0,0) to (4,4) - 5x5
        // Region 2: (5,0) to (9,4) - 5x5
        // Region 3: (0,5) to (4,9) - 5x5
        // Region 4: (5,5) to (9,9) - 5x5
        var tiles = new uint[100];

        // Fill region 1
        for (var y = 0; y < 5; y++)
            for (var x = 0; x < 5; x++)
                tiles[y * 10 + x] = 1;

        // Fill region 2
        for (var y = 0; y < 5; y++)
            for (var x = 5; x < 10; x++)
                tiles[y * 10 + x] = 2;

        // Fill region 3
        for (var y = 5; y < 10; y++)
            for (var x = 0; x < 5; x++)
                tiles[y * 10 + x] = 3;

        // Fill region 4
        for (var y = 5; y < 10; y++)
            for (var x = 5; x < 10; x++)
                tiles[y * 10 + x] = 4;

        List<Region> regions =
        [
            new(1, "Region1"),
            new(2, "Region2"),
            new(3, "Region3"),
            new(4, "Region4")
        ];

        return new RegionsMap(10, 10, tiles, regions);
    }

    [Fact]
    public void IndexerById_ExistingId_ReturnsRegion()
    {
        // Arrange
        var map = CreateTestMap();

        // Act
        var region = map[1];

        // Assert
        region.Should().NotBeNull();
        region.Id.Should().Be(1u);
        region.Name.Should().Be("Region1");
    }

    [Fact]
    public void IndexerById_NonExistingId_ReturnsNull()
    {
        // Arrange
        var map = CreateTestMap();

        // Act
        var region = map[999];

        // Assert
        region.Should().BeNull();
    }

    [Fact]
    public void IndexerByCoordinates_ValidCoordinatesInRegion_ReturnsRegionId()
    {
        // Arrange
        var map = CreateTestMap();

        // Act
        var regionId = map[2, 2];

        // Assert
        regionId.Should().NotBeNull();
        regionId!.Value.Should().Be(1u);
    }

    [Fact]
    public void IndexerByCoordinates_DifferentRegion_ReturnsCorrectRegionId()
    {
        // Arrange
        var map = CreateTestMap();

        // Act
        var regionId = map[7, 7];

        // Assert
        regionId.Should().NotBeNull();
        regionId!.Value.Should().Be(4u);
    }

    [Fact]
    public void IndexerByCoordinates_OutOfBoundsNegative_ReturnsNull()
    {
        // Arrange
        var map = CreateTestMap();

        // Act
        var regionId = map[-1, -1];

        // Assert
        regionId.Should().BeNull();
    }

    [Fact]
    public void IndexerByCoordinates_OutOfBoundsPositive_ReturnsNull()
    {
        // Arrange
        var map = CreateTestMap();

        // Act
        var regionId = map[10, 10];

        // Assert
        regionId.Should().BeNull();
    }

    [Fact]
    public void IsTileInRegion_TileInRegion_ReturnsTrue()
    {
        // Arrange
        var map = CreateTestMap();

        // Act
        var result = map.IsTileInRegion(1, 3, 3);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsTileInRegion_TileOutOfBounds_ReturnsFalse()
    {
        // Arrange
        var map = CreateTestMap();

        // Act
        var result = map.IsTileInRegion(1, 15, 15);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsTileInRegion_TileWithZeroValue_ReturnsFalse()
    {
        // Arrange
        var tiles = new uint[100]; // All zeros
        var map = new RegionsMap(10, 10, tiles, []);

        // Act
        var result = map.IsTileInRegion(1, 5, 5);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void GetIntersectedRegions_AreaInSingleRegion_ReturnsSingleRegion()
    {
        // Arrange
        var map = CreateTestMap();

        // Act
        var regions = map.GetIntersectedRegions(1, 1, 3, 3);

        // Assert
        regions.Count.Should().Be(1);
        regions[0].Id.Should().Be(1u);
    }

    [Fact]
    public void GetIntersectedRegions_AreaSpanningTwoRegions_ReturnsBothRegions()
    {
        // Arrange
        var map = CreateTestMap();

        // Act
        var regions = map.GetIntersectedRegions(3, 3, 7, 7);

        // Assert
        regions.Count.Should().Be(4);
        regions.Any(r => r.Id == 1).Should().BeTrue();
        regions.Any(r => r.Id == 2).Should().BeTrue();
        regions.Any(r => r.Id == 3).Should().BeTrue();
        regions.Any(r => r.Id == 4).Should().BeTrue();
    }

    [Fact]
    public void GetIntersectedRegions_AreaWithNoRegions_ReturnsEmptyList()
    {
        // Arrange
        var tiles = new uint[100]; // All zeros
        var map = new RegionsMap(10, 10, tiles, []);

        // Act
        var result = map.GetIntersectedRegions(0, 0, 5, 5);

        // Assert
        result.Count.Should().Be(0);
    }

    [Fact]
    public void GetIntersectedRegions_AreaPartiallyOutOfBounds_ReturnsIntersectedRegions()
    {
        // Arrange
        var map = CreateTestMap();

        // Act
        var regions = map.GetIntersectedRegions(-5, -5, 2, 2);

        // Assert
        regions.Count.Should().Be(1);
        regions[0].Id.Should().Be(1u);
    }

    [Fact]
    public void GetIntersectedRegions_AreaCompletelyOutOfBounds_ReturnsEmptyList()
    {
        // Arrange
        var map = CreateTestMap();

        // Act
        var regions = map.GetIntersectedRegions(20, 20, 25, 25);

        // Assert
        regions.Count.Should().Be(0);
    }

    [Fact]
    public void GetIntersectedRegions_AreaWithSwappedCoordinates_ReturnsCorrectRegions()
    {
        // Arrange
        var map = CreateTestMap();

        // Act (x2 < x1 and y2 < y1)
        var regions = map.GetIntersectedRegions(7, 7, 6, 6);

        // Assert
        regions.Count.Should().Be(1);
        regions[0].Id.Should().Be(4u);
    }

    [Fact]
    public void GetIntersectedRegions_SingleTileArea_ReturnsSingleRegion()
    {
        // Arrange
        var map = CreateTestMap();

        // Act
        var regions = map.GetIntersectedRegions(8, 8, 8, 8);

        // Assert
        regions.Count.Should().Be(1);
        regions[0].Id.Should().Be(4u);
    }

    [Fact]
    public void GetIntersectedRegions_HorizontalStrip_ReturnsRegionsInStrip()
    {
        // Arrange
        var map = CreateTestMap();

        // Act
        var regions = map.GetIntersectedRegions(0, 2, 9, 2);

        // Assert
        regions.Count.Should().Be(2);
        regions.Any(r => r.Id == 1).Should().BeTrue();
        regions.Any(r => r.Id == 2).Should().BeTrue();
    }

    [Fact]
    public void GetIntersectedRegions_VerticalStrip_ReturnsRegionsInStrip()
    {
        // Arrange
        var map = CreateTestMap();

        // Act
        var regions = map.GetIntersectedRegions(2, 0, 2, 9);

        // Assert
        regions.Count.Should().Be(2);
        regions.Any(r => r.Id == 1).Should().BeTrue();
        regions.Any(r => r.Id == 3).Should().BeTrue();
    }
}