namespace Geocod.io.Demo.Endpoints;

public record GeocodIoResponse
{
    public string FormattedAddress { get; set; }
    public double Accuracy { get; set; }
    public string AccuracyType { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}
