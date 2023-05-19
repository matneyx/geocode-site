using Geocod.io.Demo.Endpoints;
using Geocod.io.Demo.Hubs;

namespace Geocod.io.Demo.Clients;

public interface IGeocodIoClient
{
    Task<IEnumerable<GeocodIoResponse>> GeocodeList(IEnumerable<GeocodIoAddress> geocodioAddresses);
    Task<GeocodeData> UploadGeocodeFile(List<GeocodIoAddress> formFile);
    Task<GeocodeData> CheckGeocodeStatus(int batchId);
    Task<IEnumerable<GeocodIoResponse>> GetGeocodedFile(double batchId);
}
