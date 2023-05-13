using CsvHelper.Configuration;

namespace Geocod.io.Demo.Endpoints;

public class GeocodioAddressMap : ClassMap<GeocodioAddress>
{
    public GeocodioAddressMap()
    {
        Map(m => m.Address).Name("Address");
        Map(m => m.City).Name("City");
        Map(m => m.State).Name("State");
        Map(m => m.Zip).Name("Zip");
    }
}
