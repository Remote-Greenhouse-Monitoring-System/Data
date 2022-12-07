namespace Entities;

public class GreenhouseLastMeasurement
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public Measurement? LastMeasurement { get; set; } 

    public GreenhouseLastMeasurement(long id, string? name, Measurement? lastMeasurement)
    {
        Id = id;
        Name = name;
        LastMeasurement = lastMeasurement;
    }
}