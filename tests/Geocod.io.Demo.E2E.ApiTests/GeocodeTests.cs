using System;
using System.Net;
using Boa.Constrictor.RestSharp;
using Boa.Constrictor.Screenplay;
using Geocod.io.Demo.E2E.Framework;
using Geocod.io.Demo.E2E.Framework.Logger;
using Geocod.io.Demo.E2E.Screenplay.Requests;
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
    public void User_can_upload_a_file()
    {
        var request = GeocodeRequests.UploadCsvFile($"{AppDomain.CurrentDomain.BaseDirectory}Files\\good-test.csv");

        var response = _actor.Calls(Rest.Request(request));

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
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
