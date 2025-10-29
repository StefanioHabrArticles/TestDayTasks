using MagicOnion;
using MagicOnion.Server;
using Network.Contracts;
using Network.Mappers;
using Objects;
using Regions;

namespace Network.Service;

internal sealed class NetworkService(
    ObjectsMap objectsMap,
    RegionsMap regionsMap) : ServiceBase<INetworkService>, INetworkService
{
    public async UnaryResult<GetObjectsInAreaResponse> GetObjectsInArea(GetObjectsInAreaRequest request)
    {
        var objects = await objectsMap.Get(
            request.X1,
            request.Y1,
            request.X2,
            request.Y2);

        return new GetObjectsInAreaResponse { Objects = objects.ToObjectInfoDtos() };
    }

    public UnaryResult<GetRegionsInAreaResponse> GetRegionsInArea(GetRegionsInAreaRequest request)
    {
        var regions = regionsMap.GetIntersectedRegions(
            request.X1,
            request.Y1,
            request.X2,
            request.Y2);

        return UnaryResult.FromResult(new GetRegionsInAreaResponse { Regions = regions.ToRegionInfos() });
    }
}