using System.ComponentModel.DataAnnotations.Schema;

namespace Entities;

public class Measurement
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    public long GreenhouseId { get; set; }
    public float Temperature { get; set; }
    public float Humidity { get; set; }
    public float Co2 { get; set; }
    public int Light { get; set; }
    public DateTime Timestamp { get; set; }

    public Measurement()
    {
    }

    public Measurement(float temperature, float humidity, float co2, int light)
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