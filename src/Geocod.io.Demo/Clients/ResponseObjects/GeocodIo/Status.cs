namespace Geocod.io.Demo.Clients.ResponseObjects.GeocodIo;

public class Status
{
    public string state { get; set; }
    public int progress { get; set; }
    public string message { get; set; }
    public string time_left_description { get; set; }
    public object time_left_seconds { get; set; }
}
