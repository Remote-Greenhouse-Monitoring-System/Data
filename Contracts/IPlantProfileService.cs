using Entities;

namespace Contracts;

public interface IPlantProfileService
{
    public Task<PlantProfile> AddPlantProfile(PlantProfile plantP, long userId);
    public Task RemovePlantProfile(long pId);
    public Task<PlantProfile> UpdatePlantProfile(PlantProfile plantP);
    public Task<ICollection<PlantProfile>> GetUserPlantProfiles(long uId);
    public Task<ICollection<PlantProfile>> GetPreMadePlantProfiles();
    public Task<PlantProfile> GetPlantProfileById(long pId);
    public Task ActivatePlantProfile(long pId,long gId);
    public Task DeActivatePlantProfile(long gId);
    Task<PlantProfile> GetActivePlantProfileOnGreenhouse(long gId);
}