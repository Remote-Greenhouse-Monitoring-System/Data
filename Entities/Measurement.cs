namespace Entities;

public class Measurement
{
    public long Id { get; set; }
    public long GreenhouseId { get; set; }
    public float Temperature { get; set; }
    public float Humidity { get; set; }
    public float Co2 { get; set; }
    public int Light { get; set; }
    public DateTime Timestamp { get; set; }
}