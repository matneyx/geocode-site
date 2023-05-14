using Geocod.io.Demo.Clients;
using Geocod.io.Demo.Endpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shouldly;
using Xunit;

public class GeocodingControllerTests
{
    [Fact]
    public void Uploading_valid_csv_file_returns_ok()
    {
        // arrange
        var stream = File.OpenRead("Files\\good-test.csv");
        var file = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name))
        {
            Headers = new HeaderDictionary(),
            ContentType = "text/csv"
        };

        var controller = new GeocodeController(Mock.Of<IGeocodIoClient>());

        // act
        var response = controller.FromFile(file);
        var okResult = response.Result as OkObjectResult;

        // assert
        okResult.ShouldNotBeNull();
        okResult.StatusCode.ShouldBe(200);
    }

    [Fact]
    public void Uploading_invalid_csv_file_returns_bad_request()
    {
        // arrange
        var stream = File.OpenRead("Files\\bad-test.csv");
        var file = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name))
        {
            Headers = new HeaderDictionary(),
            ContentType = "text/csv"
        };

        var controller = new GeocodeController(Mock.Of<IGeocodIoClient>());

        // act
        var response = controller.FromFile(file);
        var badRequestResult = response.Result as BadRequestObjectResult;

        // assert
        badRequestResult.ShouldNotBeNull();
        badRequestResult.StatusCode.ShouldBe(400);

        var badRequestValue = badRequestResult.Value as string;

        badRequestValue.ShouldStartWith("File was not valid CSV, or was not in the expected format.");
        badRequestValue.ShouldContain("Header with name 'Address'[0] was not found.");
        badRequestValue.ShouldContain("Header with name 'City'[0] was not found.");
        badRequestValue.ShouldContain("Header with name 'State'[0] was not found.");
        badRequestValue.ShouldContain("Header with name 'Zip'[0] was not found.");
    }
}
