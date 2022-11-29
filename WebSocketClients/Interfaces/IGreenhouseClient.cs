using Entities;

namespace WebSocketClients.Interfaces;

public interface IGreenhouseClient
{
    Task WsClientTest();
    Task SetThresholdToGreenhouse(long gid, Threshold threshold);
}