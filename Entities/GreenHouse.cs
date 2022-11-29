namespace Entities; 

public class GreenHouse {
    // Fields
    public long GID { get; set; } 
    public string name { get; set; }
    public ICollection<Measurement> measurements { get; set; }
    public ICollection<PlantProfile> plantProfiles { get; set; }

    public GreenHouse(string name) {
        // ID SET IN DATABASE
        this.name = name;
        this.measurements = new List<Measurement>();
        this.plantProfiles = new List<PlantProfile>();
    }
}