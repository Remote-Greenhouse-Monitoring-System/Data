namespace Entities;

public class Measurement
{
    public long Id { get; set; }
    public long GreenhouseId { get; set; }
    public double Temperature { get; set; }
    public double Humidity { get; set; }
    public double Co2 { get; set; }
    public int Light { get; set; }
    public DateTime Timestamp { get; set; }

    public Measurement()
    {
    }

    public Measurement(double temperature, double humidity, double co2, int light)
    {
        Temperature = temperature;
        Humidity = humidity;
        Co2 = co2;
        Light = light;
        Timestamp = DateTime.Now;
    }

    public override string ToString()
    {
        return "M-"+Id+" : temperature="+Temperature+"C, humidity="+Humidity+"%, Co2="+Co2;
    }
}