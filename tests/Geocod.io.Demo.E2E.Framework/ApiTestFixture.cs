using Ocaramba;
using RestSharp;

namespace Geocod.io.Demo.E2E.Framework;

public class ApiTestFixture
{
    public readonly AppSettings AppSettings;
    public readonly RestClient Client;

    public ApiTestFixture()
    {
        AppSettings = new AppSettings
        {
            Url = BaseConfiguration.Url
        };

        Client = new RestClient(BaseConfiguration.Url + "api/");
    }
}
