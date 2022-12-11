using System.ComponentModel.DataAnnotations.Schema;

namespace Entities;

public class Threshold
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    public float HumidityMin { get; set; } =  -999f ;
    public float HumidityMax { get; set; } = 999f;
    public float Co2Min { get; set; } = -99999f;
    public float Co2Max { get; set; } = 99999f;
    public float TemperatureMin { get; set; } = -99f;
    public float TemperatureMax { get; set; } = 199f;

    public override bool Equals(object? obj)
    {
        if (obj is not Threshold) return false;
    
        var t = (Threshold)obj;
        return HumidityMax.Equals(t.HumidityMax)
               && HumidityMin.Equals(t.HumidityMin)
               && Co2Max.Equals(t.Co2Max)
               && Co2Min.Equals(t.Co2Min)
               && TemperatureMax.Equals(t.TemperatureMax)
               && TemperatureMin.Equals(t.TemperatureMin);
    }

    protected bool Equals(Threshold other)
    {
        return HumidityMin.Equals(other.HumidityMin) && HumidityMax.Equals(other.HumidityMax) && Co2Min.Equals(other.Co2Min) && Co2Max.Equals(other.Co2Max) && TemperatureMin.Equals(other.TemperatureMin) && TemperatureMax.Equals(other.TemperatureMax);
    }
}