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
    private readonly ClientWebSocket _clientWebSocket;

    private const string UriAddress = "wss://iotnet.cibicom.dk/app?token=vnoUBwAAABFpb3RuZXQuY2liaWNvbS5ka54Zx4fqYp5yzAQtnGzDDUw=";

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
            await _clientWebSocket.ConnectAsync(new Uri(UriAddress), CancellationToken.None);
            _logger.LogInformation("... background listener CONNECTED");
        }
        catch (Exception e) {
            _logger.LogError("Exception: {String}",e.Message);
            throw;
        }
        
        while (!stoppingToken.IsCancellationRequested)
        { 
            // keep receiving:
            Byte[] buffer = new byte[1024*2];
            _logger.LogInformation("Waiting for measurements ...");
            var x = await _clientWebSocket.ReceiveAsync(buffer, CancellationToken.None);
            if (x.Count <=0) continue;
            var strResult = Encoding.UTF8.GetString(buffer);
            // string strResult = "{\"cmd\":\"rx\",\"seqno\":1736,\"EUI\":\"0004A30B00E8355E\",\"ts\":1669797345523,\"fcnt\":7,\"port\":2,\"freq\":867500000,\"rssi\":-115,\"snr\":-6,\"toa\":0,\"dr\":\"SF12 BW125 4/5\",\"ack\":false,\"bat\":255,\"offline\":false,\"data\":\"00320019041a00\"}";
            // {"cmd":"gw","seqno":1742,"EUI":"0004A30B00E8355E","ts":1670259961585,"fcnt":0,"port":2,"freq":868300000,"toa":0,"dr":"SF12 BW125 4/5","ack":false,"gws":[{"rssi":-92,"snr":-13,"ts":1670259961614,"tmms":52000,"time":"2022-12-05T17:06:01.459672112Z","gweui":"7076FFFFFF019BF7","ant":1,"lat":56.0990389,"lon":10.2172163},{"rssi":-93,"snr":-12,"ts":1670259961614,"tmms":52000,"time":"2022-12-05T17:06:01.459672112Z","gweui":"7076FFFFFF019BF7","ant":0,"lat":56.0990389,"lon":10.2172163},{"rssi":-94,"snr":-11,"ts":1670259961606,"tmms":52000,"time":"2022-12-05T17:06:01.459670122Z","gweui":"7076FFFFFF019BFA","ant":1,"lat":56.0990389,"lon":10.2172163},{"rssi":-97,"snr":-11,"ts":1670259961606,"tmms":52000,"time":"2022-12-05T17:06:01.459670122Z","gweui":"7076FFFFFF019BFA","ant":0,"lat":56.0990389,"lon":10.2172163},{"rssi":-100,"snr":-22,"ts":1670259961585,"tmms":52000,"time":"2022-12-05T17:06:01.459722577Z","gweui":"7076FFFFFF019E83","ant":0,"lat":56.3038732,"lon":9.9761113},{"rssi":-101,"snr":-15,"ts":1670259961606,"tmms":52000,"time":"2022-12-05T17:06:01.459727149Z","gweui":"7076FFFFFF019DBF","ant":1,"lat":56.3038732,"lon":9.9761113},{"rssi":-101,"snr":-16,"ts":1670259961606,"tmms":52000,"time":"2022-12-05T17:06:01.459727149Z","gweui":"7076FFFFFF019DBF","ant":0,"lat":56.3038732,"lon":9.9761113},{"rssi":-108,"snr":-22,"ts":1670259961585,"tmms":52000,"time":"2022-12-05T17:06:01.459722577Z","gweui":"7076FFFFFF019E83","ant":1,"lat":56.3038732,"lon":9.9761113}],"bat":255,"data":"00320019041a00","_id":"638e24f9653c41d5dccd887c"}
            Console.WriteLine(strResult);
            var upLinkDto = JsonConvert.DeserializeObject<UpLinkDTO>(strResult);
            if (upLinkDto is not { cmd: "rx" }) continue;   //skip to next iteration if uplinkDto is null or 'cmd' is not "rx"
            Measurement? m = ReceivedDataToMeasurement(upLinkDto);
            if (m != null)
            {
                using (IServiceScope scope = _serviceProvider.CreateScope())
                {
                    IMeasurementService measurementService = scope.ServiceProvider.GetRequiredService<IMeasurementService>();
                    await measurementService.AddMeasurement(m,1,1);
                }
            }


            //ToDo: when to break the loop, close the connection and kill the thread? never?
            //it will close itself when the cancellation token is fired, meaning when the process is killed, aka the server in our case,
            //yeah it listens forever, because it has to
        }
        
        //close connection
        await _clientWebSocket.CloseAsync((WebSocketCloseStatus)0, null, CancellationToken.None);
    }
    
    // public override async Task StopAsync(CancellationToken cancellationToken)
    // {
    //     await _clientWebSocket.CloseAsync((WebSocketCloseStatus)0, null, CancellationToken.None);
    // }
    
    
    
    private static Measurement? ReceivedDataToMeasurement(UpLinkDTO upLinkDto)
    {
        Measurement? m = null;
        if (!String.IsNullOrEmpty(upLinkDto.data))
        {
            int i = 0;
            int l = 4;
            double temperature = Math.Round(Convert.ToInt16(upLinkDto.data.Substring(i,l),16) / 10.0, 1);
            double humidity = Math.Round(Convert.ToInt16(upLinkDto.data.Substring(i+1*l,l),16) / 10.0, 1);
            double co2 = Convert.ToInt16(upLinkDto.data.Substring(i+2*l,l),16);
            byte status = Convert.ToByte(upLinkDto.data.Substring(i+3*l,2),16);
            
            m = new Measurement((float)temperature, (float)humidity, (float)co2, 1, upLinkDto.ts);
        }
        
        return m;
    }
}