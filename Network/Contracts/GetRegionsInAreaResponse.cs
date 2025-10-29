using MemoryPack;

namespace Network.Contracts;

[MemoryPackable]
public partial class GetRegionsInAreaResponse
{
    public RegionInfo[] Regions { get; set; } = [];
}

[MemoryPackable]
public partial class RegionInfo
{
    public uint Id { get; set; }
    public string Name { get; set; } = string.Empty;
}