using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;

namespace Geocod.io.Demo.Endpoints;

public class GeocodIoAddressMap : ClassMap<GeocodIoAddress>
{
    public GeocodIoAddressMap()
    {
        Map(m => m.Address).Name("address");
        Map(m => m.City).Name("city");
        Map(m => m.State).Name("state");
        Map(m => m.Zip).Name("zip");
    }
}
