namespace Objects;

public record ObjectInfo(uint Id, int TopLeftX, int TopLeftY, int Width, int Height)
{
    public bool IsInArea(int x1, int x2, int y1, int y2)
    {
        var bottomRightX = TopLeftX + Width;
        var bottomRightY = TopLeftY + Height;

        return TopLeftX >= x1 && TopLeftY >= y1 && bottomRightX <= x2 && bottomRightY <= y2;
    }
}