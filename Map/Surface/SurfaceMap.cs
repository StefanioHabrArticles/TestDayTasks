namespace Surface;

public class SurfaceMap(int width, int height)
{
    public int Width { get; } = width;
    public int Height { get; } = height;
    private readonly SurfaceType[] _map = new SurfaceType[width * height];

    public SurfaceType this[int x, int y]
    {
        get
        {
            if (!IsInBounds(x, y))
                return SurfaceType.Unknown;
            return _map[y * Width + x];
        }
        set
        {
            if (!IsInBounds(x, y))
                return;
            _map[y * Width + x] = value;
        }
    }

    private bool IsInBounds(int x, int y) => x >= 0 && x < Width && y >= 0 && y < Height;

    public bool CanPlace(int x, int y)
    {
        var tile = new Tile(this[x, y]);
        return tile.CanPlace;
    }

    public void FillArea(int x1, int y1, int x2, int y2, SurfaceType surfaceType)
    {
        for (var y = y1; y <= y2; y++)
        {
            for (var x = x1; x <= x2; x++)
            {
                this[x, y] = surfaceType;
            }
        }
    }

    public static SurfaceMap FromCollection(int width, int height, IReadOnlyList<SurfaceType> tiles)
    {
        if (tiles.Count != width * height)
            return new SurfaceMap(width: 0, height: 0);

        var map = new SurfaceMap(width, height);
        for (var i = 0; i < tiles.Count; i++)
        {
            map._map[i] = tiles[i];
        }

        return map;
    }
}