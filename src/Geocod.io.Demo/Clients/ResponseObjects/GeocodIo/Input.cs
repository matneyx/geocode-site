using Newtonsoft.Json;

namespace Geocod.io.Demo.Clients.ResponseObjects.GeocodIo;

public record Input
{
    [JsonProperty("address_components")] public AddressComponents AddressComponents { get; set; }

    [JsonProperty("formatted_address")] public string FormattedAddress { get; set; }
}
