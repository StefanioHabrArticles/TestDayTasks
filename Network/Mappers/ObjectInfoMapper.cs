using Network.Contracts;
using Objects;
using Riok.Mapperly.Abstractions;

namespace Network.Mappers;

[Mapper]
public static partial class ObjectInfoMapper
{
    public static partial ObjectInfoDto[] ToObjectInfoDtos(this IReadOnlyList<ObjectInfo> objectInfos);
}