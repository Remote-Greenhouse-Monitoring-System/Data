using Contracts;
using Entities;

namespace EFCData;

public class PlantProfileDAO : IPlantProfileService
{
    private readonly GreenhouseContext _greenhouseContext;

    public PlantProfileDAO(GreenhouseContext greenhouseContext)
    {
        _greenhouseContext = greenhouseContext;
    }
    
    public async Task<PlantProfile> CreatePlantProfile(PlantProfile plantP)
    {
        long largestId = -1;
        if (_greenhouseContext.PlantProfiles.Any())
        {
            largestId = _greenhouseContext.PlantProfiles.Max(p => p.Id);
        }

        plantP.Id = ++largestId;
        _greenhouseContext.PlantProfiles.Add(plantP);
        await _greenhouseContext.SaveChangesAsync();
        return plantP;
    }

    public async Task RemovePlantProfile(long pId)
    {
        _greenhouseContext.PlantProfiles?.Remove(GetPlantProfileById(pId).Result);
        await _greenhouseContext.SaveChangesAsync();
    }

    public async Task<PlantProfile> UpdatePlantProfile(PlantProfile plantP)
    {
        _greenhouseContext.PlantProfiles?.Update(plantP);
        await _greenhouseContext.SaveChangesAsync();
        return plantP;
    }

    // User not implemented yet
    public Task<ICollection<PlantProfile>> GetUserPlantProfile(long uId)
    {
        throw new NotImplementedException();
    }

    public Task<ICollection<PlantProfile>> GetPremadePlantProfiles()
    {
        return Task.FromResult<ICollection<PlantProfile>>(_greenhouseContext.PlantProfiles.ToList());
    }

    public async Task<PlantProfile> GetPlantProfileById(long pId)
    {
        PlantProfile plantProfile = _greenhouseContext.PlantProfiles.First(p => p.Id == pId);
        return plantProfile;
    }

    public async Task ActivatePlantProfile(long pId)
    {
        PlantProfile plantProfileToAct = GetPlantProfileById(pId).Result;
        plantProfileToAct.Activated = true;
    }
}