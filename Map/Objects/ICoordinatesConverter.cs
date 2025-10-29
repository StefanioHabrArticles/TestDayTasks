namespace Objects;

public interface ICoordinatesConverter
{
    double[] ToGeo(int x, int y);

    int[] ToCartesian(double lat, double lng);
}