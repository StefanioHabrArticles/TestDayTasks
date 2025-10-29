namespace Objects;

public interface IObjectsMapKeyProvider
{
    string GetKey(uint id);
    
    string GetGeoKey();
}