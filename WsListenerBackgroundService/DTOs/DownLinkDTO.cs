namespace WebSocketClients.Clients;

public class DownLinkDTO
{
    //do NOT follow C# naming conventions here !!! - must be named exactly this way.
    public string cmd { get; set; }
    public string EUI { get; set; }
    public int port { get; set; }
    public bool confirmed { get; set; }
    public string data { get; set; }
}