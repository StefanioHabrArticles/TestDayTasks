namespace Objects;

public class ObjectAddedEventArgs(ObjectInfo objectInfo) : EventArgs
{
    public ObjectInfo ObjectInfo { get; } = objectInfo;
}
