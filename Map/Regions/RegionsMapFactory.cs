namespace Regions;

public class RegionsMapFactory(IRegionsGenerator regionsGenerator)
{
    public RegionsMap Create(int width, int height)
    {
        var generatorResult = regionsGenerator.Generate(width, height);
        return new RegionsMap(width, height, generatorResult.Tiles, generatorResult.Regions);
    }
}