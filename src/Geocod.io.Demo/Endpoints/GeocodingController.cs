using System.Globalization;
using CsvHelper;
using Microsoft.AspNetCore.Mvc;

namespace Geocod.io.Demo.Endpoints;

[ApiController]
[Route("api/geocode")]
public class GeocodeController : ControllerBase
{
    [HttpPost, Route("from-file")]
    public IActionResult FromFile(IFormFile file)
    {
        Console.WriteLine($"File uploaded: {file.FileName}");



        try
        {
            using var streamReader = new StreamReader(file.OpenReadStream());
            using var csv = new CsvReader(streamReader, CultureInfo.InvariantCulture);

            csv.Context.RegisterClassMap<GeocodioAddressMap>();
            csv.GetRecords<GeocodioAddress>().ToList();

            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(
                $"File was not valid CSV, or was not in the expected format.  Exception message was: {e.Message}");
        }
    }
}
