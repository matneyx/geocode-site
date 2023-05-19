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
    [Route("small-batch")]
    public async Task<IActionResult> SmallBatch(IFormFile file)
    {
        try
        {
            var list = GeocodeUtils.GetAddressesFromCsvFile(file);
            if (!list.Any()) return BadRequest("File was empty");

            var response = await _geocodIoClient.GeocodeList(list);

            return Ok(response);
        }
        catch (Exception e)
        {
            return BadRequest(
                $"File was not valid CSV, or was not in the expected format.  Exception message was: {e.Message}");
        }
    }

    [HttpPost]
    [Route("large-batch")]
    public async Task<IActionResult> LargeBatch(IFormFile file)
    {
        try
        {
            var list = GeocodeUtils.GetAddressesFromCsvFile(file);
            if (!list.Any()) return BadRequest("File was empty");

            var response = await _geocodIoClient.UploadGeocodeFile(list);

            return Ok(response);
        }
        catch (Exception e)
        {
            return BadRequest(
                $"File was not valid CSV, or was not in the expected format.  Exception message was: {e.Message}");
        }
    }

    [HttpGet("download-results")]
    public async Task<IActionResult> DownloadResults([FromQuery]double batchId)
    {
        Console.WriteLine("Downloading results");

        var results = await _geocodIoClient.GetGeocodedFile(batchId);

        return Ok(results);
    }
}

public static class GeocodeUtils{
    public static List<GeocodIoAddress> GetAddressesFromCsvFile(IFormFile file)
    {
        using var streamReader = new StreamReader(file.OpenReadStream());
        using var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture);

        csvReader.Context.RegisterClassMap<GeocodIoAddressMap>();
        var list = csvReader.GetRecords<GeocodIoAddress>().ToList();
        return list;
    }
}
