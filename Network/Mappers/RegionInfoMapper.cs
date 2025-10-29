using Network.Contracts;
using Regions;
using Riok.Mapperly.Abstractions;

namespace Network.Mappers;

[Mapper]
public static partial class RegionInfoMapper
{
    public static partial RegionInfo[] ToRegionInfos(this IReadOnlyList<Region> regions);
}