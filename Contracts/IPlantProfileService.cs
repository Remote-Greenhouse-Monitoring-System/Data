using Entities;

namespace Contracts;

public interface IPlantProfileService
{
    public Task<PlantProfile> CreatePlantProfile(PlantProfile plantP);
    public Task RemovePlantProfile(long pId);
    public Task<PlantProfile> UpdatePlantProfile(PlantProfile plantP);
    public Task<ICollection<PlantProfile>> GetUserPlantProfile(long uId);
    public Task<ICollection<PlantProfile>> GetPremadePlantProfiles();
    public Task<PlantProfile> GetPlantProfileById(long pId);
    public Task ActivatePlantProfile(long pId);
}