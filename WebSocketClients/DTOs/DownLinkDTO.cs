namespace WebSocketClients.Clients;

public class DownLinkDTO
{
    public string cmd { get; set; }
    public string EUI { get; set; }
    public int port { get; set; }
    public bool confirmed { get; set; }
    public string data { get; set; }
}