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
    private ClientWebSocket _clientWebSocket;
    
    private readonly string _uriAddress = "wss://iotnet.cibicom.dk/app?token=vnoUBwAAABFpb3RuZXQuY2liaWNvbS5ka54Zx4fqYp5yzAQtnGzDDUw=";
    private readonly string _eui = "0004A30B00E8355E";

    public GreenhouseClient(IMeasurementService measurementService)
    {
        _measurementService = measurementService;
        _clientWebSocket = new ClientWebSocket();

        Thread t = new Thread(async o =>
        {
            // await ConnectClientAsync();
            while (true)
            {
                //receive
                // Byte[] buffer = new byte[255];
                // Console.Out.WriteLine("waiting for measurements ...");
                // var x = await _clientWebSocket.ReceiveAsync(buffer, CancellationToken.None);
                // var strResult = Encoding.UTF8.GetString(buffer);
                string strResult = "{\"cmd\":\"rx\",\"data\":\"00FC016700C8E0\"}";
                await Console.Out.WriteLineAsync("received: " + strResult);
                
                Measurement? m = ReceivedDataToMeasurement(strResult);
                if (m != null)
                {
                    await _measurementService.AddMeasurementAsync(m);
                }

                await Task.Delay(10000);
                //ToDo: when to break the loop, close the connection and kill the thread? never?
            }
            await _clientWebSocket.CloseAsync((WebSocketCloseStatus)0, null, CancellationToken.None);
        });
        t.Start();
    }

    private Measurement? ReceivedDataToMeasurement(string receivedJson)
    {
        UpLinkDTO? receivedPayload = JsonConvert.DeserializeObject<UpLinkDTO>(receivedJson);

        Measurement? m = null;
        if (!String.IsNullOrEmpty(receivedPayload!.data))
        {
            int i = 0;
            int l = 4;
            double temperature = Math.Round(Convert.ToInt16(receivedPayload.data.Substring(i,l),16) / 10.0, 1);
            double humidity = Math.Round(Convert.ToInt16(receivedPayload.data.Substring(i+1*l,l),16) / 10.0, 1);
            double co2 = Convert.ToInt16(receivedPayload.data.Substring(i+2*l,l),16);
            byte status = Convert.ToByte(receivedPayload.data.Substring(i+3*l,2),16);
            m = new Measurement((float)temperature, (float)humidity, (float)co2, 1);
        }
        
        return m;
    }

    private async Task ConnectClientAsync()
    {
        //connect:
        try
        {
            await _clientWebSocket.ConnectAsync(new Uri(_uriAddress), CancellationToken.None);
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
            EUI = _eui,
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