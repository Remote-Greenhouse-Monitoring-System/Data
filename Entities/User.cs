using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Entities;

public class User
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
    
    public string? Email { get; set; }
    [JsonIgnore]
    public ICollection<GreenHouse>? GreenHouses { get; set; } = new List<GreenHouse>();
    public string? NotificationToken { get; set; }
    [JsonIgnore] public ICollection<PlantProfile>? PlantProfiles { get; set; } = new List<PlantProfile>();
    public User()
    {
    }

    [JsonConstructor]
    public User(long id, string username, string email)
    {
        Id = id;
        Username = username;
        Email = email;
    }
}