namespace Geocod.io.Demo.Clients.ResponseObjects.GeocodIo;

public record Response
{
    public Input Input { get; set; }
    public List<Result> Results { get; set; }
}
