using System.Globalization;
using System.Net.WebSockets;
using System.Text;
using Contracts;
using Entities;
using FirebaseNotificationClient;
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

    //ExecuteAsync()
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        //open connection:
        try
        {
            await _clientWebSocket.ConnectAsync(new Uri(UriAddress), CancellationToken.None);
        }
        catch (Exception e) {
            Console.WriteLine("Exception: " + e.Message);
            throw;
        }
        
        //instantiate services:
        using var scope = _serviceProvider.CreateScope();
        var thresholdService = scope.ServiceProvider.GetRequiredService<IThresholdService>();
        var greenhouseService = scope.ServiceProvider.GetRequiredService<IGreenHouseService>();
        var measurementService = scope.ServiceProvider.GetRequiredService<IMeasurementService>();
        var notificationClient = scope.ServiceProvider.GetRequiredService<INotificationClient>();
        var userService = scope.ServiceProvider.GetRequiredService<IUserService>();

        //infinite-listening loop
        var uplinkJson = "";
        var lastStatusBits = "00000000";
        while (!stoppingToken.IsCancellationRequested)
        {
            var buffer = new byte[256];
            var receiveResult = await _clientWebSocket.ReceiveAsync(buffer, CancellationToken.None);
            uplinkJson += Encoding.UTF8.GetString(buffer);
            //if not endOfMsg -> keep receiving ...
            if (!receiveResult.EndOfMessage) continue;
            //when endOfMsg->deserialize into UplinkDTO-object
            var upLinkDto = JsonConvert.DeserializeObject<UpLinkDTO>(uplinkJson);
            uplinkJson = "";
            //if uplinkDto is null or 'cmd' is not "rx" or data are nullOrEmpty continue listening
            if (upLinkDto is not { Cmd: "rx" } || string.IsNullOrEmpty(upLinkDto.Data)) continue;
            
            var greenhouseId = await greenhouseService.GetGreenhouseIdByEui(upLinkDto.Eui);
            //extract measurements and send to DB
            var newMeasurement = GetMeasurementFromReceivedData(upLinkDto.Data);
            await measurementService.AddMeasurement(newMeasurement, greenhouseId);

            //send response/=thresholds [DownLink]
            var threshold = await thresholdService.GetThresholdOnActivePlantProfile(greenhouseId);
            await SendDownLinkAsync(upLinkDto.Eui, upLinkDto.Port, threshold);
            
            //extract status and send notification if changed
            var newStatusBits = GetStatusFromReceivedData(upLinkDto.Data);
            if (!lastStatusBits.Equals(newStatusBits))
            {
                var whatActionsHappened = GetChangedActions(lastStatusBits, newStatusBits);
                var user =await userService.GetGreenhouseUser(greenhouseId);
                if (user.Token != null)
                {
                    await notificationClient.SendNotificationToUser(user.Token!,
                        "Following action(s) have been triggered in your greenhouse-"+greenhouseId, whatActionsHappened.ToString()!);
                }
            }
            lastStatusBits = newStatusBits;
        }
        
        //close connection
        await _clientWebSocket.CloseAsync(0, null, CancellationToken.None);
    }

    private async Task SendDownLinkAsync(string eui, int port, Threshold threshold)
    {
        //create DownLink-obj
        DownLinkDTO downLinkDto = new ()
        {
            cmd = "tx",
            EUI = eui,
            port = port,
            confirmed = true,
            data = GetHexStringFromThreshold(threshold)
        };
        //serialize to json
        var downLinkJson = JsonConvert.SerializeObject(downLinkDto);
        //send serialized DownLink
        await _clientWebSocket.SendAsync(Encoding.UTF8.GetBytes(downLinkJson), WebSocketMessageType.Text, true, CancellationToken.None);
    }
    
    
    //------------ convert/retrieve methods -------------
    private const int OneByte = 2;
    private const int TwoBytes = 4;
    private const int FourBytes = 8;
    public static Measurement GetMeasurementFromReceivedData(string data)
    {
        var i = 0;
        
        var temperature = Convert.ToInt16(data.Substring(i,TwoBytes),16) / 10.0;
        i += TwoBytes;
        var humidity = Math.Round(Convert.ToInt16(data.Substring(i,TwoBytes),16) / 10.0, 1);
        i += TwoBytes;
        var co2 = Convert.ToInt16(data.Substring(i,TwoBytes),16);
        i += TwoBytes;
        var intRep = BitConverter.GetBytes(Int32.Parse(data.Substring(i,FourBytes), NumberStyles.AllowHexSpecifier));
        if (BitConverter.IsLittleEndian)
            Array.Reverse(intRep);
        var light = BitConverter.ToSingle(intRep, 0);

        return new Measurement((float)temperature, (float)humidity, co2, (int)light);
    }

    public static string GetStatusFromReceivedData(string data)
    {
        const int statusStartIndex = 3*TwoBytes + 1*FourBytes;
        var statusInt = Convert.ToInt16(data.Substring(statusStartIndex, OneByte), 16);
        var statusBits = Convert.ToString(statusInt, 2).PadLeft(8, '0');
        return statusBits;
    }
    
    //0000 light-co2-humidity-window
    private static readonly Dictionary<int, string> Actions = new()
    {
        {0, ""}
        ,{1, ""}
        ,{2, ""}
        ,{3, ""}
        ,{4,"Light-action"}
        ,{5,"Co2-action"}
        ,{6,"Humidity-action"}
        ,{7,"Temperature-action"}
    };
    
    public static List<string> GetChangedActions(string lastStatus, string newStatus)
    {
        var changedActions = new List<string>();
        for (var i = 0; i < lastStatus.Length; i++)
        {
            //if last- and new- states are the same ->skip
            if (lastStatus[i].Equals(newStatus[i])) continue;
            //else
            switch (newStatus[i])
            {
                case '0':
                    if (Actions[i].Equals("")) break;
                    changedActions.Add(Actions[i]+" turned OFF");
                    break;
                case '1':
                    if (Actions[i].Equals("")) break;
                    changedActions.Add(Actions[i]+" turned ON");
                    break;
            }
        }
        return changedActions;
    }
    
    public static string GetHexStringFromThreshold(Threshold? thresholds)
    {
        //if threshold equals new/=empty threshold ->return zeros
        if (thresholds==null || thresholds.Equals(new Threshold()))
            return "000000000000000000000000";

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