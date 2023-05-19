using System.Text.Json;
using Geocod.io.Demo.Clients;
using Geocod.io.Demo.Endpoints;
using Microsoft.AspNetCore.SignalR;

namespace Geocod.io.Demo.Hubs;

public interface IGeocodeHub
{
    Task GeocodeStart(GeocodeData data);
    Task GeocodeUpdate(GeocodeData data);
    Task GeocodeComplete(GeocodeData data);
}

public class GeocodeHub : Hub<IGeocodeHub>
{
    private IGeocodIoClient _geocodIoClient;

    public GeocodeHub(IGeocodIoClient geocodIoClient)
    {
        _geocodIoClient = geocodIoClient;
    }

    public async Task SendHandshake(Handshake handshake)
    {
        _ = Task.Run(() =>  MonitorGeocodeProgress(Context.ConnectionId, handshake.BatchId));
        await Clients.All.GeocodeStart(new GeocodeData()
        {
            BatchId = handshake.BatchId,
            Progress = 0,
        });
    }

    private async Task MonitorGeocodeProgress(string connectionId, int responseBatchId)
    {
        double progress = 0;
        GeocodeData statusUpdate = null;

        while(progress < 100)
        {
            Console.WriteLine($"Progress = {progress}%");
            statusUpdate = await _geocodIoClient.CheckGeocodeStatus(responseBatchId);

            Clients.Client(connectionId).GeocodeUpdate(statusUpdate);

            progress = statusUpdate.Progress;
            Thread.Sleep(1000);
        }

        await Clients.Client(connectionId).GeocodeComplete(statusUpdate);
    }
}
