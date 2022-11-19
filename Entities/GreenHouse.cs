namespace Entities; 

public class GreenHouse {
    // Fields
    public long id { get; set; }  // TODO: define type to use for id
    public string name { get; set; }
    public List<Measurement> measurements { get; set; }
    public List<PlantProfile> plantProfiles { get; set; }

    public GreenHouse(string name) {
        // ID SET IN DATABASE
        this.name = name;
        this.measurements = new List<Measurement>();
        this.plantProfiles = new List<PlantProfile>();
    }
}