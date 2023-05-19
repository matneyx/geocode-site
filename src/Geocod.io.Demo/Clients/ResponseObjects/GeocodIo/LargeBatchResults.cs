using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;

namespace Geocod.io.Demo.Clients.ResponseObjects.GeocodIo;

[TrimOptions(TrimOptions.Trim)]
public class LargeBatchResults
{
    public string Address { get; set; }
    public string City { get; set; }
    public string StateZip { get; set; } // I think this is a bug in Geocod.io's API
    public string Latitude { get; set; }
    public string Longitude { get; set; }
    public string AccuracyScore { get; set; }
    public string AccuracyType { get; set; }
    public string Number { get; set; }
    public string Street { get; set; }
    public string UnitType { get; set; }
    public string UnitNumber { get; set; }
    public string City2 { get; set; }
    public string State { get; set; }
    public string County { get; set; }
    public string Zip { get; set; }
    public string Country { get; set; }
    public string Source { get; set; }
}

[TrimOptions(TrimOptions.Trim)]
public class LargeBatchResultsClassMap : ClassMap<LargeBatchResults>
{
    public LargeBatchResultsClassMap()
    {
        Map(m => m.Address).Name("address");
        Map(m => m.City).Name("city");
        Map(m => m.StateZip).Name("state_zip");
        Map(m => m.Latitude).Name("Latitude");
        Map(m => m.Longitude).Name("Longitude");
        Map(m => m.AccuracyScore).Name("Accuracy Score");
        Map(m => m.AccuracyType).Name("Accuracy Type");
        Map(m => m.Number).Name("Number");
        Map(m => m.Street).Name("Street");
        Map(m => m.UnitType).Name("Unit Type");
        Map(m => m.UnitNumber).Name("Unit Number");
        Map(m => m.City2).Name("City2");
        Map(m => m.State).Name("State");
        Map(m => m.County).Name("County");
        Map(m => m.Zip).Name("Zip");
        Map(m => m.Country).Name("Country");
        Map(m => m.Source).Name("Source");
    }
}
