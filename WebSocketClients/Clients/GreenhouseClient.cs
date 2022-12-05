using System.Net.WebSockets;
using System.Text;
using Contracts;
using Entities;
using Newtonsoft.Json;
using WebSocketClients.Interfaces;

namespace WebSocketClients.Clients;

public class GreenhouseClient : IGreenhouseClient
{
    private IMeasurementService _measurementService;
    private readonly ClientWebSocket _clientWebSocket;

    private const string UriAddress = "wss://iotnet.cibicom.dk/app?token=vnoUBwAAABFpb3RuZXQuY2liaWNvbS5ka54Zx4fqYp5yzAQtnGzDDUw=";
    private const string Eui = "0004A30B00E8355E";

    public GreenhouseClient(IMeasurementService measurementService)
    {
        _measurementService = measurementService;
        _clientWebSocket = new ClientWebSocket();
    }

    private async Task ConnectClientAsync()
    {
        //connect:
        try
        {
            await _clientWebSocket.ConnectAsync(new Uri(UriAddress), CancellationToken.None);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }
    
    public async Task WsClientTest()
    {

        await ConnectClientAsync();
        
        //create payloadDTO
        DownLinkDTO downLinkDto = new ()
        {
            cmd = "tx",
            EUI = Eui,
            port = 2,
            confirmed = false,
            data = "E803"
        };
        string payloadJson = JsonConvert.SerializeObject(downLinkDto);
        
        //send
        await _clientWebSocket.SendAsync(Encoding.UTF8.GetBytes(payloadJson), WebSocketMessageType.Text, true, CancellationToken.None);
        await Console.Out.WriteLineAsync("sent: " + payloadJson);
        
        //receive
        Byte[] buffer = new byte[256];
        var x = await _clientWebSocket.ReceiveAsync(buffer, CancellationToken.None);
        var strResult = Encoding.UTF8.GetString(buffer);
        await Console.Out.WriteLineAsync("received: " + strResult);
        
        // UpLinkDTO? receivedPayload = JsonConvert.DeserializeObject<UpLinkDTO>(strResult);
    }

    public Task SetThresholdToGreenhouse(long gid, Threshold threshold)
    {
        throw new NotImplementedException();
    }
}