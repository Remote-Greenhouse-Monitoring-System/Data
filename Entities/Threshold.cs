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
}