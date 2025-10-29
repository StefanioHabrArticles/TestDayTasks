using AwesomeAssertions;

namespace Regions.Tests;

public class RegionsMapFactoryTests
{
    private readonly RegionsMapFactory _factory = new(new RegionsGenerator());

    [Fact]
    public void Create_GeneratedMap_AllRegionsHaveEqualArea()
    {
        // Arrange
        const int width = 100;
        const int height = 100;

        // Act
        var map = _factory.Create(width, height);

        var regionAreas = new Dictionary<uint, int>();
        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                var regionId = map[x, y];
                if (regionId is not > 0) continue;
                regionAreas.TryAdd(regionId.Value, 0);
                regionAreas[regionId.Value]++;
            }
        }

        // Assert
        regionAreas.Values.Should().AllSatisfy(x => x.Should().Be(regionAreas.Values.First()));
    }

    [Fact]
    public void Create_GeneratedMap_RegionsDoNotIntersect()
    {
        // Arrange
        const int width = 100;
        const int height = 100;

        // Act
        var map = _factory.Create(width, height);

        var tilesWithRegions = new HashSet<(int x, int y)>();
        var allRegionIds = new HashSet<uint>();

        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                var regionId = map[x, y];

                if (regionId is not > 0) continue;

                var tileCoordinate = (x, y);
                tilesWithRegions.Contains(tileCoordinate).Should().BeFalse();
                tilesWithRegions.Add(tileCoordinate);

                allRegionIds.Add(regionId.Value);
            }
        }

        // Assert
        allRegionIds.Count.Should().BeGreaterThan(0);
        tilesWithRegions.Count.Should().BeGreaterThan(0);
    }

    [Fact]
    public void Create_GeneratedMap_EachTileBelongsToMaxOneRegion()
    {
        // Arrange
        const int width = 50;
        const int height = 50;
        const int regionsCount = 5;

        // Act
        var map = _factory.Create(width, height, regionsCount);

        // Assert
        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                var regionId = map[x, y];

                if (regionId.HasValue)
                {
                    var secondQuery = map[x, y];
                    secondQuery.Should().Be(regionId);
                }
            }
        }
    }
}