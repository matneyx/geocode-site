using Newtonsoft.Json;

namespace Geocod.io.Demo.Clients.ResponseObjects.GeocodIo;

public class Result
{
    public string Query { get; set; }
    public Response Response { get; set; }

    [JsonProperty("address_components")] public AddressComponents AddressComponents { get; set; }

    [JsonProperty("formatted_address")] public string FormattedAddress { get; set; }

    public Location Location { get; set; }
    public double Accuracy { get; set; }

    [JsonProperty("accuracy_type")] public string AccuracyType { get; set; }

    public string Source { get; set; }
}
