namespace WebSocketClients.Clients;

public class DownLinkDTO
{
    public string Cmd { get; set; }
    public string Eui { get; set; }
    public int Port { get; set; }
    public bool Confirmed { get; set; }
    public string Data { get; set; }
}