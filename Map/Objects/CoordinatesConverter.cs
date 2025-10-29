using CoordinateSharp;

namespace Objects;

internal sealed class CoordinatesConverter : ICoordinatesConverter
{
    public double[] ToGeo(int x, int y)
    {
        var coordinate = Cartesian.CartesianToLatLong(x, y, 0);
        return [coordinate.Longitude.ToDouble(), coordinate.Latitude.ToDouble()];
    }

    public int[] ToCartesian(double lat, double lng)
    {
        var cartesian = new Coordinate(lat, lng).Cartesian;
        return [double.ConvertToInteger<int>(cartesian.X), double.ConvertToInteger<int>(cartesian.Y)];
    }
}