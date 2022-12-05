using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Entities;

public class PlantProfile
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]    
    public long Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public float OptimalTemperature { get; set; }
    public float OptimalHumidity { get; set; }
    public float OptimalCo2 { get; set; }
    public int OptimalLight { get; set; }
    [JsonIgnore]
    public Threshold Threshold { get; set; }
    [JsonIgnore]
    public ICollection<Measurement>? Measurements { get; set; } = new List<Measurement>();
    public PlantProfile()
    {
    }
    [JsonConstructor]
    public PlantProfile(string name, string description, float optimalTemperature, float optimalHumidity, float optimalCo2, int optimalLight)
    {
        Name = name;
        Description = description;
        OptimalTemperature = optimalTemperature;
        OptimalHumidity = optimalHumidity;
        OptimalCo2 = optimalCo2;
        OptimalLight = optimalLight;
        Threshold = new Threshold();
    }
}