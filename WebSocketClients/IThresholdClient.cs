using Entities;

namespace WebSocketClients.Interfaces;

public interface IThresholdClient
{
    Task WsClientTest();
    Task SetThresholdToGreenhouse(long gid, Threshold threshold);
}