using Newtonsoft.Json;

namespace Geocod.io.Demo.Clients.ResponseObjects.GeocodIo;

public class Location
{
    [JsonProperty("lat")] public double Latitude { get; set; }

    [JsonProperty("lng")] public double Longitude { get; set; }
}
