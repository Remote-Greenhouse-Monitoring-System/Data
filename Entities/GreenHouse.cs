using System.ComponentModel.DataAnnotations.Schema;

namespace Entities; 

public class GreenHouse {
    // Fields
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; } 
    public string Name { get; set; }
    public ICollection<Measurement> measurements { get; set; } = new List<Measurement>();
    public ICollection<PlantProfile> plantProfiles { get; set; } = new List<PlantProfile>();

    public GreenHouse(string name) {
        // ID SET IN DATABASE
        this.Name = name;
        
    }
}