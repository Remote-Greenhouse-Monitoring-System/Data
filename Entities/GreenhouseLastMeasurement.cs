namespace Entities;

public class GreenhouseLastMeasurement
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public Measurement? LastMeasurement { get; set; } 
    public string? EUI { get; set; }

    public GreenhouseLastMeasurement(long id, string? name, Measurement? lastMeasurement, string? EUI)
    {
        Id = id;
        Name = name;
        LastMeasurement = lastMeasurement;
        this.EUI = EUI;
    }
}