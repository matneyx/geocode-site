using Geocod.io.Demo.Endpoints;

namespace Geocod.io.Demo.Clients;

public interface IGeocodIoClient
{
    Task<IEnumerable<GeocodIoResponse>> GeocodeList(List<GeocodIoAddress> geocodioAddresses);
}
