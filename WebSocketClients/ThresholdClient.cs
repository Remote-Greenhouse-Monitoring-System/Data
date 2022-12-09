using System.Net.WebSockets;
using System.Text;
using Contracts;
using Entities;
using Newtonsoft.Json;
using WebSocketClients.Interfaces;

namespace WebSocketClients.Clients;

public class ThresholdClient : IThresholdClient
{
    private readonly ClientWebSocket _clientWebSocket;
    private IGreenHouseService _greenHouseService;

    private const string UriAddress = "wss://iotnet.cibicom.dk/app?token=vnoUBwAAABFpb3RuZXQuY2liaWNvbS5ka54Zx4fqYp5yzAQtnGzDDUw=";
    private const string Eui = "0004A30B00E8355E";

    public ThresholdClient(IGreenHouseService greenHouseService)
    {
        _greenHouseService = greenHouseService;
        _clientWebSocket = new ClientWebSocket();
    }

    private async Task ConnectClientAsync()
    {
        if (_clientWebSocket.State != WebSocketState.Open)
        {
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
    }
    
    public async Task WsClientTest()
    {
        //connect
        await ConnectClientAsync();
        
        //create payloadDTO
        DownLinkDTO downLinkDto = new ()
        {
            Cmd = "tx",
            Eui = Eui,
            Port = 2,
            Confirmed = false,
            Data = "E803"
        };
        string payloadJson = JsonConvert.SerializeObject(downLinkDto);
        
        //send
        await _clientWebSocket.SendAsync(Encoding.UTF8.GetBytes(payloadJson), WebSocketMessageType.Text, true, CancellationToken.None);
        await Console.Out.WriteLineAsync("sent: " + payloadJson);
        
        // receive
         // Byte[] buffer = new byte[256];
         // var x = await _clientWebSocket.ReceiveAsync(buffer, CancellationToken.None);
        // if (x.Count <= 0)
        // {
        //     throw new Exception("downlink not confirmed!");
        // }
        // var strResult = Encoding.UTF8.GetString(buffer);
        // await Console.Out.WriteLineAsync("received: " + strResult);
        
        // var response = JsonConvert.DeserializeObject<DownLinkResponse>(strResult);
        // if (response is null or { DataEnqueued: false })
        // {
        //     throw new Exception("downlink not confirmed!");
        // }
    }

    public async Task SetThresholdToGreenhouse(long gid, Threshold threshold)
    {
        //connect
        await ConnectClientAsync();
        
        //create payloadDTO
        DownLinkDTO downLinkDto = new ()
        {
            Cmd = "tx",
            Eui = Eui,
            //ToDo: EUI by Id, not hardcoded
            // Eui = await _greenHouseService.getEUIById(gid);
            Port = 2,
            Confirmed = false,
            Data = ThresholdsToByteString(threshold)
        };
        var downLinkJson = JsonConvert.SerializeObject(downLinkDto);
        
        //send
        await _clientWebSocket.SendAsync(Encoding.UTF8.GetBytes(downLinkJson), 
            WebSocketMessageType.Text, true, CancellationToken.None);
    }

    private static string ThresholdsToByteString(Threshold threshold)
    {
        var thresholdByteString = BitConverter.GetBytes(threshold.TemperatureMin).ToString()
            + BitConverter.GetBytes(threshold.TemperatureMax)
            + BitConverter.GetBytes(threshold.HumidityMin)
            + BitConverter.GetBytes(threshold.HumidityMax)
            + BitConverter.GetBytes(threshold.Co2Min)
            + BitConverter.GetBytes(threshold.Co2Max);
        return thresholdByteString;
    }
}