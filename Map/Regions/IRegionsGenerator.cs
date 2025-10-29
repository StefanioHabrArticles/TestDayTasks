namespace Regions;

public interface IRegionsGenerator
{
    RegionsGeneratorResult Generate(int width, int height, int regionsCount = 10);
}