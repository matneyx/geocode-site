namespace Geocod.io.Demo.Clients.ResponseObjects.GeocodIo;

public class BatchStatus
{
    public int id { get; set; }
    public List<object> fields { get; set; }
    public File file { get; set; }
    public Status status { get; set; }
    public object download_url { get; set; }
    public DateTime expires_at { get; set; }
}
