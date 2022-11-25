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
        string payload =
            "{\"cmd\"  : \"tx\",\"EUI\"  : \"0004A30B00E8355E\",\"port\" : 2,\"confirmed\" : false,\"data\" : \"E803\"}";
        PlantProfile plantProfile = new PlantProfile("name","dscrpt",1,2,3, 4);
        try
        {
            await _clientWebSocket.ConnectAsync(new Uri("wss://iotnet.cibicom.dk/app?token=vnoUBwAAABFpb3RuZXQuY2liaWNvbS5ka54Zx4fqYp5yzAQtnGzDDUw="), CancellationToken.None);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        await _clientWebSocket.SendAsync(Encoding.UTF8.GetBytes(payload), WebSocketMessageType.Text, true, CancellationToken.None);
        Console.Out.WriteLine("sent: " + payload);

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