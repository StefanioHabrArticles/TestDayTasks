using System.Collections.Frozen;

namespace Regions;

public class RegionsMap(
    int width,
    int height,
    uint[] tiles,
    IReadOnlyList<Region> regions)
{
    private readonly FrozenDictionary<uint, Region> _regions =
        regions.ToFrozenDictionary(x => x.Id);

    public Region? this[uint id] => _regions.GetValueOrDefault(id);

    public uint? this[int x, int y]
    {
        get
        {
            if (!IsInBounds(x, y))
                return null;

            var index = y * width + x;
            return tiles[index];
        }
    }

    public bool IsTileInRegion(uint regionId, int x, int y) => this[x, y] == regionId;

    public IReadOnlyList<Region> GetIntersectedRegions(int x1, int y1, int x2, int y2)
    {
        var intersectedRegionIds = new HashSet<uint>();

        var minX = Math.Max(0, Math.Min(x1, x2));
        var maxX = Math.Min(width - 1, Math.Max(x1, x2));
        var minY = Math.Max(0, Math.Min(y1, y2));
        var maxY = Math.Min(height - 1, Math.Max(y1, y2));

        for (var y = minY; y <= maxY; y++)
        {
            for (var x = minX; x <= maxX; x++)
            {
                var regionId = this[x, y];
                if (regionId.HasValue)
                {
                    intersectedRegionIds.Add(regionId.Value);
                }
            }
        }

        return intersectedRegionIds.Contains(0)
            ? []
            : intersectedRegionIds
                .Select(id => this[id]!)
                .ToList();
    }

    private bool IsInBounds(int x, int y)
    {
        return x >= 0 && x < width && y >= 0 && y < height;
    }
}