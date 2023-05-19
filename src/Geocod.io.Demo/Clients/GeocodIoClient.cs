using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using Geocod.io.Demo.Clients.ResponseObjects.GeocodIo;
using Geocod.io.Demo.Endpoints;
using Geocod.io.Demo.Hubs;
using Newtonsoft.Json;
using RestSharp;

namespace Geocod.io.Demo.Clients;

public class GeocodIoClient : IGeocodIoClient
{
    public GeocodIoClient()
    {
        // Default constructor. Used for production.
    }

    private static readonly Dictionary<string, int> _accuracyTypeMapper = new()
    {
        { "rooftop", 1 },
        { "point", 2 },
        { "range_interpolation", 3 },
        { "nearest_rooftop_match", 4 },
        { "intersection", 5 },
        { "street_center", 6 },
        { "place", 7 },
        { "county", 8 },
        { "state", 9 }
    };

    private static IRestClient _client;

    // Constructor used for tests
    public GeocodIoClient(IRestClient restClient)
    {
        _client = restClient;
    }

    public async Task<IEnumerable<GeocodIoResponse>> GeocodeList(IEnumerable<GeocodIoAddress> geocodioAddresses)
    {
        using var client = GetRestClient();

        var stringList = geocodioAddresses.Select(ga => ga.ToString()).ToArray();

        var request = new RestRequest("geocode", Method.Post);
        request.AddJsonBody(JsonConvert.SerializeObject(stringList));

        var response = await client.ExecuteAsync(request);
        var rootList = JsonConvert.DeserializeObject<ListResponse>(response.Content);

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

    private static IRestClient GetRestClient()
    {
        if(_client != null) return _client;

        // TODO: Add URL to the appsettings.json file
        var client = new RestClient("https://api.geocod.io/v1.7/");

        // TODO: Add the API Key to the appsettings.json file
        client.DefaultParameters.AddParameter(new QueryParameter("api_key", "00fe16f0646546060e81540514406e1214f1818"));
        return client;
    }

    public async Task<GeocodeData> UploadGeocodeFile(List<GeocodIoAddress> formFile)
    {
        try
        {
            using var client = GetRestClient();

            var request = new RestRequest("lists", Method.Post);

            request.AddParameter("filename", "file.csv");
            request.AddParameter("direction", "forward");
            request.AddParameter("format", "{{A}} {{B}} {{C}} {{D}}");

            var formattedAddresses = formFile.Select(ga => ga.ToString()).ToArray();
            var csv = string.Join("\n", formattedAddresses);

            request.AddParameter("file", csv, ParameterType.RequestBody);

            var response = await client.ExecuteAsync(request);
            var batchResponse = JsonConvert.DeserializeObject<BatchResponse>(response.Content);

            return new GeocodeData()
            {
                BatchId = batchResponse.id
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<GeocodeData> CheckGeocodeStatus(int batchId)
    {
        using var client = GetRestClient();

        var request = new RestRequest($"lists/{batchId}", Method.Get);
        var response = await client.ExecuteAsync(request);
        var batchResponse = JsonConvert.DeserializeObject<BatchStatus>(response.Content);

        return new GeocodeData()
        {
            BatchId = batchId,
            Progress = batchResponse.status.progress
        };
    }

    public async Task<IEnumerable<GeocodIoResponse>> GetGeocodedFile(double batchId)
    {
        using var client = GetRestClient();

        var request = new RestRequest($"lists/{batchId}/download", Method.Get);
        var response = await client.ExecuteAsync(request);

        // Despite what the docs say, the response doesn't contain a header row.
        var headerRow =
            "address,city,state_zip,Latitude,Longitude,Accuracy Score,Accuracy Type,Number,Street,Unit Type,Unit Number,City2,State,County,Zip,Country,Source\n";

        using var stringReader = new StringReader(headerRow + response.Content);
        using var csvReader = new CsvReader(stringReader, CultureInfo.InvariantCulture);
        csvReader.Context.RegisterClassMap<LargeBatchResultsClassMap>();
        var list = csvReader.GetRecords<LargeBatchResults>().ToList();

        // Filter out the rows that don't have a valid Accuracy Score, Latitude, or Longitude
        return list
            .Where(a
                => double.TryParse(a.AccuracyScore, out _)
                   && a.AccuracyScore != "0" && a.AccuracyScore != string.Empty
                 && double.TryParse(a.Latitude, out _)
                   && a.Latitude != "0" && a.Latitude != string.Empty
                 && double.TryParse(a.Longitude, out _)
                   && a.Longitude != "0" && a.Longitude != string.Empty
                )
            .Select(a => new GeocodIoResponse()
        {
            Accuracy = double.TryParse(a.AccuracyScore, out var accuracyScoreDouble)
                ? accuracyScoreDouble
                : 0,
            AccuracyType = a.AccuracyType,
            FormattedAddress =
                $"{a.Address}, {(a.City.Trim() == string.Empty ? a.City2 : a.City)}, {a.State} {a.Zip}, {a.Country}",
            Latitude = double.TryParse(a.Latitude, out var latitudeDouble) ? latitudeDouble : 0,
            Longitude = double.TryParse(a.Longitude, out var longitudeDouble) ? longitudeDouble : 0
        }).ToList();
    }
}
