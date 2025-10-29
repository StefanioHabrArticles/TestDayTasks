using MagicOnion;
using Network.Contracts;

namespace Network.Service;

public interface INetworkService : IService<INetworkService>
{
    UnaryResult<GetObjectsInAreaResponse> GetObjectsInArea(GetObjectsInAreaRequest request);
    UnaryResult<GetRegionsInAreaResponse> GetRegionsInArea(GetRegionsInAreaRequest request);
}