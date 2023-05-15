using Geocod.io.Demo.Clients.ResponseObjects.GeocodIo;
using Geocod.io.Demo.Endpoints;
using Newtonsoft.Json;
using RestSharp;

namespace Geocod.io.Demo.Clients;

public class GeocodIoClient : IGeocodIoClient
{
    private readonly IRestClient _client;

    public GeocodIoClient(IRestClient client)
    {
        _client = client;
    }

    private static readonly Dictionary<string, int> _accuracyTypeMapper = new()
    {
        {"rooftop", 1},
        {"point", 2},
        {"range_interpolation", 3},
        {"nearest_rooftop_match", 4},
        {"intersection", 5},
        {"street_center", 6},
        {"place", 7},
        {"county", 8},
        {"state", 9},
    };

    public async Task<IEnumerable<GeocodIoResponse>> GeocodeList(List<GeocodIoAddress> geocodioAddresses)
    {

        var stringList = geocodioAddresses.Select(ga => ga.ToString()).ToArray();

        var request = new RestRequest("geocode", Method.Post);
        request.AddJsonBody(JsonConvert.SerializeObject(stringList));

        var response = await _client.ExecuteAsync(request);
        var rootList = JsonConvert.DeserializeObject<Root>(response.Content);

        return rootList.Results
            .Select(r =>
            r.Response.Results
                .OrderByDescending(r2 => r2.Accuracy)
                .ThenBy(r2 => _accuracyTypeMapper[r2.AccuracyType])
                .Select(r2 => new GeocodIoResponse
                {
                    FormattedAddress = r2.FormattedAddress,
                    Accuracy = r2.Accuracy,
                    AccuracyType = r2.AccuracyType,
                    Latitude = r2.Location.Latitude,
                    Longitude = r2.Location.Longitude
                }).First()
        );
    }
}
