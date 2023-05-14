using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Boa.Constrictor.RestSharp;
using Boa.Constrictor.Screenplay;
using Geocod.io.Demo.E2E.Framework;
using Geocod.io.Demo.E2E.Framework.Logger;
using Geocod.io.Demo.E2E.Framework.ResponseObjects;
using Geocod.io.Demo.E2E.Screenplay.Requests;
using Newtonsoft.Json;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace Geocod.io.Demo.E2E.ApiTests;

public class GeocodeTests : IClassFixture<ApiTestFixture>
{
    private readonly IActor _actor;

    public GeocodeTests(ApiTestFixture fixture, ITestOutputHelper outputHelper)
    {
        var logger = new NunitBoaConstrictorLogger(outputHelper, LogSeverity.Info);
        _actor = new Actor("Api User", logger);
        _actor.Can(CallRestApi.Using(fixture.Client));
    }

    [Fact]
    public void User_gets_an_ok_response_with_list_of_results()
    {
        var request = GeocodeRequests.UploadCsvFile($"{AppDomain.CurrentDomain.BaseDirectory}Files\\good-test.csv");

        var response = _actor.Calls(Rest.Request(request));

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var results = JsonConvert.DeserializeObject<List<GeocodioResponse>>(response.Content);

        results.First().FormattedAddress.ShouldBe("660 Pennsylvania Ave SE, Washington, DC 20003");
        results.First().Accuracy.ShouldBe(1);
        results.First().AccuracyType.ShouldBe("rooftop");
        results.First().Latitude.ShouldBe(38.885172d);
        results.First().Longitude.ShouldBe(-76.996565d);
    }

    [Fact]
    public void User_gets_bad_request_if_file_is_incorrect()
    {
        var request = GeocodeRequests.UploadCsvFile($"{AppDomain.CurrentDomain.BaseDirectory}Files\\bad-test.csv");

        var response = _actor.Calls(Rest.Request(request));

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        response.Content.ShouldStartWith("\"File was not valid CSV, or was not in the expected format.");
        response.Content.ShouldContain("Header with name 'Address'[0] was not found.");
        response.Content.ShouldContain("Header with name 'City'[0] was not found.");
        response.Content.ShouldContain("Header with name 'State'[0] was not found.");
        response.Content.ShouldContain("Header with name 'Zip'[0] was not found.");
    }
}
