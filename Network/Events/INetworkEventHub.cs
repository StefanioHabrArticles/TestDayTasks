using MagicOnion;
using Objects;

namespace Network.Events;

public interface INetworkEventHub : IStreamingHub<INetworkEventHub, INetworkEventReceiver>
{
    Task JoinAsync();
    Task LeaveAsync();
}

public interface INetworkEventReceiver
{
    void ObjectAdded(ObjectAddedEventArgs e);
    void ObjectDeleted(ObjectDeletedEventArgs e);
}