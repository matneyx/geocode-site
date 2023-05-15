using Newtonsoft.Json;

namespace Geocod.io.Demo.Clients.ResponseObjects.GeocodIo;

public record AddressComponents
{
    public string Number { get; set; }

    public string Street { get; set; }

    public string Suffix { get; set; }

    public string PostDirectional { get; set; }

    [JsonProperty("formatted_street")] public string FormattedStreet { get; set; }

    public string City { get; set; }

    public string State { get; set; }
    public string Zip { get; set; }
    public string Country { get; set; }
    public string PreDirectional { get; set; }
    public string County { get; set; }
}
