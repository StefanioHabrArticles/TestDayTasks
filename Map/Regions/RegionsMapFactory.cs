namespace Regions;

public class RegionsMapFactory(IRegionsGenerator regionsGenerator)
{
    public RegionsMap Create(int width, int height, int regionsCount = 10)
    {
        var generatorResult = regionsGenerator.Generate(width, height, regionsCount);
        return new RegionsMap(width, height, generatorResult.Tiles, generatorResult.Regions);
    }
}