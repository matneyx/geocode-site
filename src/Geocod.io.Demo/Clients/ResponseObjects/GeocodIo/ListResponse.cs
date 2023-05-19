namespace Geocod.io.Demo.Clients.ResponseObjects.GeocodIo;

public record ListResponse
{
    public List<Result> Results { get; set; }
}
