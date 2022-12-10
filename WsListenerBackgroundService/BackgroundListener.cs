using System.Net.WebSockets;
using System.Text;
using Contracts;
using Entities;
using Newtonsoft.Json;
using WebSocketClients.Clients;
using WsListenerBackgroundService.DTOs;
namespace WsListenerBackgroundService;

public class BackgroundListener : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ClientWebSocket _clientWebSocket;

    private const string UriAddress = "wss://iotnet.cibicom.dk/app?token=vnoUBwAAABFpb3RuZXQuY2liaWNvbS5ka54Zx4fqYp5yzAQtnGzDDUw=";

    public BackgroundListener(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _clientWebSocket = new ClientWebSocket();
    }
    
    //StartAsync()
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
    
    //ExecuteAsync()
    private string _lastStatusBits = "00000000";
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        //open connection:
        try
        {
            await _clientWebSocket.ConnectAsync(new Uri(UriAddress), CancellationToken.None);
            Console.WriteLine("... background listener CONNECTED");
        }
        catch (Exception e) {
            Console.WriteLine("Exception: " + e.Message);
            throw;
        }
        
        //--------------
        using var scope = _serviceProvider.CreateScope();
        var thresholdService = scope.ServiceProvider.GetRequiredService<IThresholdService>();
        var greenhouseService = scope.ServiceProvider.GetRequiredService<IGreenHouseService>();
        var measurementService = scope.ServiceProvider.GetRequiredService<IMeasurementService>();
        //--------------

        //infinite-listening loop
        var uplinkJson = "";
        while (!stoppingToken.IsCancellationRequested)
        {
            //waiting for message/measurements
            Console.WriteLine("Waiting for measurements ... " + DateTime.Now);
            //receive message
            Byte[] buffer = new byte[256];
            var receiveResult = await _clientWebSocket.ReceiveAsync(buffer, CancellationToken.None);
            uplinkJson += Encoding.UTF8.GetString(buffer);
            
            if (!receiveResult.EndOfMessage) continue;
            Console.WriteLine("received: " + uplinkJson);

            //->deserialize into UplinkDTO-object
            var upLinkDto = JsonConvert.DeserializeObject<UpLinkDTO>(uplinkJson);
            uplinkJson = "";
            //skip to next iteration if uplinkDto is null or 'cmd' is not "rx"
            if (upLinkDto is not { Cmd: "rx" }) continue;

            //send response [DownLink]
            var greenhouseId = greenhouseService.GetGreenhouseIdByEui(upLinkDto.Eui);
            var threshold = await thresholdService.GetThresholdOnActivePlantProfile(greenhouseId);
            await SendDownLinkAsync(upLinkDto.Eui, upLinkDto.Port, threshold);

            //if data=null continue listening
            if (string.IsNullOrEmpty(upLinkDto.Data)) continue;
            
            //extract measurements and send to DB
            var newMeasurement = GetMeasurementFromReceivedData(greenhouseId, upLinkDto.Data);
            await measurementService.AddMeasurement(newMeasurement,greenhouseId);

            //extract status and send notification if changed
            var newStatusBits = GetStatusFromReceivedData(upLinkDto.Data);
            if (!_lastStatusBits.Equals(newStatusBits))
            {
                var whatActionsHappened = GetChangedActions(_lastStatusBits, newStatusBits);
                //ToDo: ->send notification here
            }
            _lastStatusBits = newStatusBits;
        }
        
        //close connection
        await _clientWebSocket.CloseAsync(0, null, CancellationToken.None);
    }

    //StopAsync()
    // public override async Task StopAsync(CancellationToken cancellationToken)
    // {
    //     await _clientWebSocket.CloseAsync((WebSocketCloseStatus)0, null, CancellationToken.None);
    // }

    
    private async Task SendDownLinkAsync(string eui, int port, Threshold threshold)
    {
        //create fake DownLink
        DownLinkDTO downLinkDto = new ()
        {
            cmd = "tx",
            EUI = eui,
            port = port,
            confirmed = true,
            data = GetHexStringFromThreshold(threshold)
            // data = "001b001d"
        };
        //convert to json
        string downLinkJson = JsonConvert.SerializeObject(downLinkDto);
        // const string downLinkJson = "{\"cmd\"  : \"tx\",\"EUI\"  : \"0004A30B00E8355E\",\"port\" : 2,\"confirmed\" : false,\"data\" : \"001b001d\"}";
        await _clientWebSocket.SendAsync(Encoding.UTF8.GetBytes(downLinkJson), WebSocketMessageType.Text, true, CancellationToken.None);
        await Console.Out.WriteLineAsync("DownLink sent: " + downLinkJson);
    }
    


    //------------ convert/retrieve methods -------------
    
    private const int ByteSize = 2;
    public static Measurement GetMeasurementFromReceivedData(long gId, string data)
    {
        var i = 0;
        
        var temperature = Convert.ToInt16(data.Substring(i,ByteSize*2),16) / 10.0;
        i += ByteSize * 2;
        var humidity = Math.Round(Convert.ToInt16(data.Substring(i,ByteSize*2),16) / 10.0, 1);
        i += ByteSize * 2;
        var co2 = Convert.ToInt16(data.Substring(i,ByteSize*2),16);
        // i += ByteSize * 2;
        // var light = Convert.ToInt16(data.Substring(i,ByteSize*4),16);

        // return new Measurement((float)temperature, (float)humidity, co2, light);
        return new Measurement(gId, (float)temperature, (float)humidity, co2, 1);
    }

    public static string GetStatusFromReceivedData(string data)
    {
        var statusInt = Convert.ToInt16(data.Substring(6 * ByteSize, ByteSize), 16);
        var statusBits = Convert.ToString(statusInt, 2).PadLeft(8, '0');
        return statusBits;
    }
    
    public static List<string> GetChangedActions(string lastStatus, string newStatus)
    {
        //0000 light-co2-humidity-window
        var actions = new[] { "Light-action", "Co2-action", "Humidity-action", "Temperature-action"};

        var changedActions = new List<string>();
        for (var i = 0; i < lastStatus.Length; i++)
        {
            //if last- and new- states are the same ->skip
            if (lastStatus[i].Equals(newStatus[i])) continue;
            //else
            switch (newStatus[i])
            {
                case '0':
                    changedActions.Add(actions[i-4]+" turned OFF");
                    break;
                case '1':
                    changedActions.Add(actions[i-4]+" turned ON");
                    break;
            }
        }

        return changedActions;
    }
    
    public static string GetHexStringFromThreshold(Threshold thresholds)
    {
        //ToDo: light missing ...?

        var thresholdHexString = "";
        thresholdHexString += ((int)(thresholds.TemperatureMax *10)).ToString("X4");
        thresholdHexString += ((int)(thresholds.TemperatureMin *10)).ToString("X4");
        thresholdHexString += ((int)(thresholds.HumidityMax *10)).ToString("X4");
        thresholdHexString += ((int)(thresholds.HumidityMin *10)).ToString("X4");
        thresholdHexString += ((int)thresholds.Co2Max).ToString("X4");
        thresholdHexString += ((int)thresholds.Co2Min).ToString("X4");
        return thresholdHexString;
    }
}