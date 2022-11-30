using System.Net.WebSockets;
using System.Text;
using Contracts;
using Entities;
using Newtonsoft.Json;
using WebSocketClients.Clients;

namespace WsListenerBackgroundService;

public class BackgroundListener : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<BackgroundListener> _logger;
    private ClientWebSocket _clientWebSocket;
    
    private readonly string _uriAddress = "wss://iotnet.cibicom.dk/app?token=vnoUBwAAABFpb3RuZXQuY2liaWNvbS5ka54Zx4fqYp5yzAQtnGzDDUw=";
    private readonly string _eui = "0004A30B00E8355E";

    public BackgroundListener(IServiceProvider serviceProvider ,ILogger<BackgroundListener> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _clientWebSocket = new ClientWebSocket();
    }
    
    // public override async Task StartAsync(CancellationToken cancellationToken)
    // {
    //     //connect:
    //     try
    //     {
    //         await _clientWebSocket.ConnectAsync(new Uri(_uriAddress), CancellationToken.None);
    //     }catch (Exception e) {
    //         _logger.LogError("Exception: {String}",e.Message);
    //         throw;
    //     }
    // }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        //open connection:
        try
        {
            await _clientWebSocket.ConnectAsync(new Uri(_uriAddress), CancellationToken.None);
            _logger.LogWarning("CONNECTED ... {Time}", DateTime.Now);
        }
        catch (Exception e) {
            _logger.LogError("Exception: {String}",e.Message);
            throw;
        }
        
        while (!stoppingToken.IsCancellationRequested)
        { 
            // keep receiving:
            Byte[] buffer = new byte[1024];
            _logger.LogWarning("waiting for measurements ... {Time}", DateTime.Now);
            // await Task.Delay(10000, stoppingToken);
            var x = await _clientWebSocket.ReceiveAsync(buffer, CancellationToken.None);
            var strResult = Encoding.UTF8.GetString(buffer);
            // string strResult = "{\"cmd\":\"rx\",\"data\":\"00FC016700C8E0\"}";
            // {"cmd":"rx","seqno":1736,"EUI":"0004A30B00E8355E","ts":1669797345523,"fcnt":7,"port":2,"freq":867500000,"rssi":-115,"snr":-6,"toa":0,"dr":"SF12 BW125 4/5","ack":false,"bat":255,"offline":false,"data":"00320019041a00"}
            _logger.LogWarning("received: {String}", strResult);
            
            // Measurement? m = ReceivedDataToMeasurement(strResult);
            // if (m != null)
            // {
            //     _logger.LogWarning("received: {Measurement}", m);
            //     using (IServiceScope scope = _serviceProvider.CreateScope())
            //     {
            //         IMeasurementService measurementService = scope.ServiceProvider.GetRequiredService<IMeasurementService>();
            //         await measurementService.AddMeasurementAsync(m);
            //     }
            // }
            
            //ToDo: when to break the loop, close the connection and kill the thread? never?
        }
        
        //close connection
        await _clientWebSocket.CloseAsync((WebSocketCloseStatus)0, null, CancellationToken.None);
    }
    
    // public override async Task StopAsync(CancellationToken cancellationToken)
    // {
    //     await _clientWebSocket.CloseAsync((WebSocketCloseStatus)0, null, CancellationToken.None);
    // }
    
    
    
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
}