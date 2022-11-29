using System.Net.WebSockets;
using Contracts;

namespace GreenhouseDataAPI;

public class WsListenerBackgroundService : BackgroundService
{
    private IMeasurementService _measurementService;
    private ClientWebSocket _listenerSocket;

    public WsListenerBackgroundService(IMeasurementService measurementService)
    {
        _listenerSocket = new ClientWebSocket();
        _measurementService = measurementService;
    }

    private readonly string _uriAddress = "wss://iotnet.cibicom.dk/app?token=vnoUBwAAABFpb3RuZXQuY2liaWNvbS5ka54Zx4fqYp5yzAQtnGzDDUw=";
    private readonly string _eui = "0004A30B00E8355E";
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Console.Out.WriteAsync("hello " +DateTime.Now);
            await Task.Delay(1000, stoppingToken);
        }
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        //connect
        await _listenerSocket.ConnectAsync(new Uri(_uriAddress), cancellationToken);    }
    
    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        //close connection
        await _listenerSocket.CloseAsync((WebSocketCloseStatus)0, null, cancellationToken);
    }
}