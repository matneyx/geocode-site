namespace Geocod.io.Demo.Endpoints;

public class GeocodIoAddress
{
    public string Address { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Zip { get; set; }

    public override string ToString()
    {
        return $"{Address}, {City}, {State} {Zip}";
    }
}
