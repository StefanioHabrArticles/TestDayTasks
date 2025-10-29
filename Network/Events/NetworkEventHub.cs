using MagicOnion.Server.Hubs;
using Objects;

namespace Network.Events;

public sealed class NetworkEventHub(ObjectsMap objectsMap) : StreamingHubBase<INetworkEventHub, INetworkEventReceiver>, INetworkEventHub
{
    private IGroup<INetworkEventReceiver>? _group;

    public async Task JoinAsync()
    {
        _group = await Group.AddAsync("map_events");
        objectsMap.ObjectAdded += (_, e) => _group?.All.ObjectAdded(e);
        objectsMap.ObjectDeleted += (_, e) => _group?.All.ObjectDeleted(e);
    }

    public async Task LeaveAsync()
    {
        if (_group != null)
        {
            await _group.RemoveAsync(Context);
        }
    }

    protected override ValueTask OnDisconnected()
    {
        return ValueTask.CompletedTask;
    }
}