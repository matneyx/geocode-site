using RestSharp;

namespace Geocod.io.Demo.E2E.Screenplay.Requests;

public static class GeocodeRequests
{
    public static IRestRequest UploadCsvFile(string file) =>
        new RestRequest("geocode/small-batch", Method.POST).AddFile("file", file);
}
