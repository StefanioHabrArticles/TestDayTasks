namespace Regions;

public record RegionsGeneratorResult(
    uint[] Tiles,
    IReadOnlyList<Region> Regions)
{
    public static RegionsGeneratorResult Empty { get; } = new([], []);
};