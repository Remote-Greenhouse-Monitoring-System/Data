namespace WebSocketClients.Clients;

public class UpLinkDto
{
    public string Cmd { get; set; }
    public string Eui { get; set; }
    public long Ts { get; set; }
    public bool Ack { get; set; }
    public int Fcnt { get; set; }
    public int Port { get; set; }
    public int Seqno { get; set; }
    public string Data { get; set; }
}