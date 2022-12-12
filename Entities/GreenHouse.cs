using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Entities; 

public class GreenHouse {
    
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }  
    public string Name { get; set; }
    [JsonIgnore]
    public long UserId { get; set; }
    public string? DeviceEui { get; set; }
    
    [JsonIgnore] 
    public List<Measurement>? Measurements { get; set; } = new List<Measurement>();
    
    [JsonIgnore]
    public PlantProfile? ActivePlantProfile{ get; set; } 
    
    [JsonConstructor]
    public GreenHouse(string name) {
        Name = name;
    
    }
}