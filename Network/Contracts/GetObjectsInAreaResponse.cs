using MemoryPack;

namespace Network.Contracts;

[MemoryPackable]
public partial class GetObjectsInAreaResponse
{
    public ObjectInfoDto[] Objects { get; set; } = [];
}

[MemoryPackable]
public partial class ObjectInfoDto
{
    public uint Id { get; set; }
    public int TopLeftX { get; set; }
    public int TopLeftY { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
}