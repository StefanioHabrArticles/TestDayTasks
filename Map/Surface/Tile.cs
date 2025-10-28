namespace Surface;

public readonly ref struct Tile(SurfaceType surfaceType)
{
    public bool CanPlace => surfaceType == SurfaceType.Flat;
}
