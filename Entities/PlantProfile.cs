namespace Entities;

public class PlantProfile
{
    public long Id { get; set; }
    public string Name { get; set; }
    public Boolean Activated { get; set; }
    public string Description { get; set; }
    public float OptimalTemperature { get; set; }
    public float OptimalHumidity { get; set; }
    public float OptimalCo2 { get; set; }
    public int OptimalLight { get; set; }

    
    public PlantProfile()
    {
    }

    public PlantProfile(string name, string description, float optimalTemperature, float optimalHumidity, float optimalCo2, int optimalLight)
    {
        Name = name;
        Description = description;
        OptimalTemperature = optimalTemperature;
        OptimalHumidity = optimalHumidity;
        OptimalCo2 = optimalCo2;
        OptimalLight = optimalLight;

        Activated = false;
        Id = -1;
    }
}