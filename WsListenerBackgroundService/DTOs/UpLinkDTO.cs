﻿namespace WebSocketClients.Clients;

public class UpLinkDTO
{
    public string cmd { get; set; }
    public string EUI { get; set; }
    public long ts { get; set; }
    public bool ack { get; set; }
    public int fcnt { get; set; }
    public int port { get; set; }
    public int seqno { get; set; }
    public string data { get; set; }
}