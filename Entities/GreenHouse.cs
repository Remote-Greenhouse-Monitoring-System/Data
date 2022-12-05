using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Entities; 

public class GreenHouse {
    // Fields
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }  
    public string Name { get; set; }
    [JsonIgnore] 
    public List<Measurement> Measurements { get; set; } = new List<Measurement>();
    [JsonIgnore]
    public PlantProfile? ActivePlantProfile{ get; set; } 
    
    [JsonConstructor]
    public GreenHouse(string name) {
        Name = name;
    
    }
}