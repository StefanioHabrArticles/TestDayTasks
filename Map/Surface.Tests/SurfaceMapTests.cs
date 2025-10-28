using AwesomeAssertions;

namespace Surface.Tests;

public class SurfaceMapTests
{
    [Fact]
    public void Indexer_GetValidCoordinates_ReturnsCorrectValue()
    {
        // Arrange
        var map = new SurfaceMap(3, 3);
        map[1, 1] = SurfaceType.Mountain;

        // Act
        var result = map[1, 1];

        // Assert
        result.Should().Be(SurfaceType.Mountain);
    }

    [Fact]
    public void Indexer_GetOutOfBoundsCoordinates_ReturnsUnknown()
    {
        // Arrange
        var map = new SurfaceMap(3, 3);

        // Act
        var result = map[5, 5];

        // Assert
        result.Should().Be(SurfaceType.Unknown);
    }

    [Fact]
    public void Indexer_GetNegativeCoordinates_ReturnsUnknown()
    {
        // Arrange
        var map = new SurfaceMap(3, 3);

        // Act
        var result = map[-1, -1];

        // Assert
        result.Should().Be(SurfaceType.Unknown);
    }

    [Fact]
    public void Indexer_SetValidCoordinates_ValueIsSet()
    {
        // Arrange
        var map = new SurfaceMap(3, 3);

        // Act
        map[2, 2] = SurfaceType.Mountain;

        // Assert
        map[2, 2].Should().Be(SurfaceType.Mountain);
    }

    [Fact]
    public void Indexer_SetOutOfBoundsCoordinates_DoesNotThrowException()
    {
        // Arrange
        var map = new SurfaceMap(3, 3);

        // Act & Assert
        var exception = Record.Exception(() => map[10, 10] = SurfaceType.Mountain);
        exception.Should().BeNull();
    }

    [Fact]
    public void CanPlace_FlatTile_ReturnsTrue()
    {
        // Arrange
        var map = new SurfaceMap(3, 3);
        map[1, 1] = SurfaceType.Flat;

        // Act
        var result = map.CanPlace(1, 1);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void CanPlace_MountainTile_ReturnsFalse()
    {
        // Arrange
        var map = new SurfaceMap(3, 3);
        map[1, 1] = SurfaceType.Mountain;

        // Act
        var result = map.CanPlace(1, 1);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void CanPlace_UnknownTile_ReturnsFalse()
    {
        // Arrange
        var map = new SurfaceMap(3, 3);

        // Act
        var result = map.CanPlace(1, 1);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void CanPlace_OutOfBoundsCoordinates_ReturnsFalse()
    {
        // Arrange
        var map = new SurfaceMap(3, 3);

        // Act
        var result = map.CanPlace(10, 10);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void FillArea_ValidRectangle_FillsAllTilesInArea()
    {
        // Arrange
        var map = new SurfaceMap(5, 5);

        // Act
        map.FillArea(1, 1, 3, 3, SurfaceType.Mountain);

        // Assert
        for (var y = 1; y <= 3; y++)
        {
            for (var x = 1; x <= 3; x++)
            {
                map[x, y].Should().Be(SurfaceType.Mountain);
            }
        }
    }

    [Fact]
    public void FillArea_ValidRectangle_DoesNotAffectTilesOutsideArea()
    {
        // Arrange
        var map = new SurfaceMap(5, 5);
        map[0, 0] = SurfaceType.Flat;
        map[4, 4] = SurfaceType.Flat;

        // Act
        map.FillArea(1, 1, 3, 3, SurfaceType.Mountain);

        // Assert
        map[0, 0].Should().Be(SurfaceType.Flat);
        map[4, 4].Should().Be(SurfaceType.Flat);
    }

    [Fact]
    public void FillArea_SingleTile_FillsOnlyOneTile()
    {
        // Arrange
        var map = new SurfaceMap(3, 3);

        // Act
        map.FillArea(1, 1, 1, 1, SurfaceType.Mountain);

        // Assert
        map[1, 1].Should().Be(SurfaceType.Mountain);
        map[0, 0].Should().Be(SurfaceType.Unknown);
        map[2, 2].Should().Be(SurfaceType.Unknown);
    }

    [Fact]
    public void FromCollection_ValidCollection_PopulatesMapCorrectly()
    {
        // Arrange
        List<SurfaceType> tiles =
        [
            SurfaceType.Flat, SurfaceType.Mountain, SurfaceType.Flat,
            SurfaceType.Mountain, SurfaceType.Flat, SurfaceType.Mountain
        ];

        // Act
        var map = SurfaceMap.FromCollection(3, 2, tiles);

        // Assert
        map.Width.Should().Be(3);
        map.Height.Should().Be(2);
        map[0, 0].Should().Be(SurfaceType.Flat);
        map[1, 0].Should().Be(SurfaceType.Mountain);
        map[2, 0].Should().Be(SurfaceType.Flat);
        map[0, 1].Should().Be(SurfaceType.Mountain);
        map[1, 1].Should().Be(SurfaceType.Flat);
        map[2, 1].Should().Be(SurfaceType.Mountain);
    }

    [Fact]
    public void FromCollection_CollectionSmallerThanMapSize_CreatesMapWithUnknownTiles()
    {
        // Arrange
        List<SurfaceType> tiles = [SurfaceType.Flat, SurfaceType.Mountain];

        // Act
        var map = SurfaceMap.FromCollection(3, 3, tiles);

        // Assert
        map[0, 0].Should().Be(SurfaceType.Unknown);
        map[1, 0].Should().Be(SurfaceType.Unknown);
        map[2, 0].Should().Be(SurfaceType.Unknown);
    }

    [Fact]
    public void FromCollection_EmptyCollection_CreatesMapWithUnknownTiles()
    {
        // Arrange
        // Act
        var map = SurfaceMap.FromCollection(2, 2, []);

        // Assert
        map[0, 0].Should().Be(SurfaceType.Unknown);
        map[1, 1].Should().Be(SurfaceType.Unknown);
    }
}
