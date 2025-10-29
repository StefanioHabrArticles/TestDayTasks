namespace Regions;

public class RegionsGenerator : IRegionsGenerator
{
    public RegionsGeneratorResult Generate(int width, int height, int regionsCount)
    {
        if (regionsCount <= 0 || width <= 0 || height <= 0)
        {
            return RegionsGeneratorResult.Empty;
        }

        // Calculate grid dimensions for equal-sized regions
        var totalArea = width * height;
        var targetRegionArea = totalArea / regionsCount;
        int regionSize, regionsX, regionsY;
        while (true)
        {
            regionSize = (int)Math.Floor(Math.Sqrt(targetRegionArea));

            regionsX = (int)Math.Floor((double)width / regionSize);
            regionsY = (int)Math.Floor((double)height / regionSize);

            if (regionsX * regionsY > regionsCount)
                break;
            targetRegionArea--;
        }

        // Create tiles array
        var tiles = new uint[width * height];
        var regions = new List<Region>();

        uint regionId = 1;

        // Generate regions in a grid pattern
        for (var ry = 0; ry < regionsY; ry++)
        {
            for (var rx = 0; rx < regionsX; rx++)
            {
                // Calculate region bounds
                var x1 = rx * regionSize;
                var y1 = ry * regionSize;
                var x2 = Math.Min(x1 + regionSize - 1, width - 1);
                var y2 = Math.Min(y1 + regionSize - 1, height - 1);

                // Skip if region is outside map bounds
                if (x1 >= width || y1 >= height)
                    continue;

                // Fill tiles array with region ID
                for (var y = y1; y <= y2; y++)
                {
                    for (var x = x1; x <= x2; x++)
                    {
                        tiles[y * width + x] = regionId;
                    }
                }

                // Create region with GUID name
                regions.Add(new Region(regionId, Name: Guid.NewGuid().ToString()));
                if (regions.Count == regionsCount)
                    return new RegionsGeneratorResult(tiles, regions);

                regionId++;
            }
        }

        return RegionsGeneratorResult.Empty;
    }
}