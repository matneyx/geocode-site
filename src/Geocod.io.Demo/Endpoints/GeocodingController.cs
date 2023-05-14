using System.Globalization;
using CsvHelper;
using Geocod.io.Demo.Clients;
using Microsoft.AspNetCore.Mvc;

namespace Geocod.io.Demo.Endpoints;

[ApiController]
[Route("api/geocode")]
public class GeocodeController : ControllerBase
{
    private readonly IGeocodIoClient _geocodIoClient;

    public GeocodeController(IGeocodIoClient geocodIoClient)
    {
        _geocodIoClient = geocodIoClient;
    }

    [HttpPost]
    [Route("from-file")]
    public async Task<IActionResult> FromFile(IFormFile file)
    {
        Console.WriteLine($"File uploaded: {file.FileName}");

        try
        {
            using var streamReader = new StreamReader(file.OpenReadStream());
            using var csv = new CsvReader(streamReader, CultureInfo.InvariantCulture);

            csv.Context.RegisterClassMap<GeocodIoAddressMap>();
            var list = csv.GetRecords<GeocodIoAddress>().ToList();

            var response = await _geocodIoClient.GeocodeList(list);

            return Ok(response);
        }
        catch (Exception e)
        {
            return BadRequest(
                $"File was not valid CSV, or was not in the expected format.  Exception message was: {e.Message}");
        }
    }
}
