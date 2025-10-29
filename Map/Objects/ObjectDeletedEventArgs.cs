namespace Objects;

public class ObjectDeletedEventArgs(uint id) : EventArgs
{
    public uint Id { get; } = id;
}
