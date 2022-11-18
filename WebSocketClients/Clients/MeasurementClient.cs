using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;
using Newtonsoft.Json;

namespace WebSocketClients.Clients;

using Contracts;
using Entities;
using WebSocketClients.Interfaces;


public class MeasurementClient : IMeasurementClient
{

    private IMeasurementService? _measurementService;
    private ClientWebSocket _clientWebSocket;
    
    public MeasurementClient(IMeasurementService measurementService)
    {
        _measurementService = measurementService;
        _clientWebSocket = new ClientWebSocket();
    }

    
    
    //managing a plant profile of a device/=greenhouse
    public void SetPlantProfile(PlantProfile plantProfile)
    {
        Console.Out.WriteLine("MOCK-plant profile "+plantProfile.Id+" -SET");
    }
    public void UpdatePlantProfile(PlantProfile plantProfile)
    {
        Console.Out.WriteLine("MOCK-plant profile "+plantProfile.Id+" -UPDATED");
    }
    public void DeletePlantProfile(PlantProfile plantProfile)
    {
        Console.Out.WriteLine("MOCK-plant profile "+plantProfile.Id+" -DELETED");
    }

    public async Task<PlantProfile> ClientTestPlantProfile()
    {
        PlantProfile plantProfile = new PlantProfile("name","dscrpt",1,2,3, 4);
        
        string json = JsonConvert.SerializeObject(plantProfile);
        await _clientWebSocket.ConnectAsync(new Uri("wss://localhost:1234/ws"), CancellationToken.None);
        await _clientWebSocket.SendAsync(Encoding.UTF8.GetBytes(json), WebSocketMessageType.Text, true, CancellationToken.None);
        Console.Out.WriteLine("sent: " + json);

        Byte[] buffer = new byte[256];
        var x = await _clientWebSocket.ReceiveAsync(buffer, CancellationToken.None);
        var strResult = System.Text.Encoding.UTF8.GetString(buffer);
        Console.Out.WriteLine("received: " + strResult);
        return JsonConvert.DeserializeObject<PlantProfile>(strResult);
    }

    public Task<Measurement> ClientTestMeasurements()
    {
        throw new NotImplementedException();
    }
}