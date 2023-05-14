using Geocod.io.Demo.Clients.ResponseObjects.GeocodIo;
using Geocod.io.Demo.Endpoints;
using Newtonsoft.Json;
using RestSharp;

namespace Geocod.io.Demo.Clients;

public class GeocodIoClient : IGeocodIoClient
{
    public async Task<List<GeocodIoResponse>> GeocodeList(List<GeocodIoAddress> geocodioAddresses)
    {
        var stringList = geocodioAddresses.Select(ga => ga.ToString()).ToArray();

        // TODO: Add URL to the appsettings.json file
        using var client = new RestClient("https://api.geocod.io/v1.7/");

        var request = new RestRequest("geocode", Method.Post);

        // TODO: Add the API Key to the appsettings.json file
        request.AddQueryParameter("api_key", "00fe16f0646546060e81540514406e1214f1818");

        request.AddJsonBody(JsonConvert.SerializeObject(stringList));

        var response = await client.ExecuteAsync(request);
        var rootList = JsonConvert.DeserializeObject<Root>(response.Content);

        return rootList.Results.Select(r =>
            r.Response.Results.Select(r2 => new GeocodIoResponse
            {
                FormattedAddress = r2.FormattedAddress,
                Accuracy = r2.Accuracy,
                AccuracyType = r2.AccuracyType,
                Latitude = r2.Location.Latitude,
                Longitude = r2.Location.Longitude
            })
        ).SelectMany(r => r).ToList();
    }
}
