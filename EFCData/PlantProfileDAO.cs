using Contracts;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace EFCData;

public class PlantProfileDAO : IPlantProfileService
{
    private readonly GreenhouseSystemContext _greenhouseSystemContext;

    public PlantProfileDAO(GreenhouseSystemContext greenhouseSystemContext)
    {
        _greenhouseSystemContext = greenhouseSystemContext;
    }
    
    public async Task<PlantProfile> CreatePlantProfile(PlantProfile plantP)
    {
        await _greenhouseSystemContext.Thresholds!.AddAsync(plantP.Threshold);
        
        await _greenhouseSystemContext.PlantProfiles!.AddAsync(plantP);
        await _greenhouseSystemContext.SaveChangesAsync();
        return plantP;
    }

    public async Task RemovePlantProfile(long pId)
    {
        _greenhouseSystemContext.PlantProfiles?.Remove(GetPlantProfileById(pId).Result);
        await _greenhouseSystemContext.SaveChangesAsync();
    }

    public async Task<PlantProfile> UpdatePlantProfile(PlantProfile plantP)
    {
        _greenhouseSystemContext.PlantProfiles?.Update(plantP);
        await _greenhouseSystemContext.SaveChangesAsync();
        return plantP;
    }

    // User not implemented yet
    public Task<ICollection<PlantProfile>> GetUserPlantProfile(long uId)
    {
        throw new NotImplementedException();
    }

    public async Task<ICollection<PlantProfile>> GetPremadePlantProfiles()
    {
        return await _greenhouseSystemContext.PlantProfiles!.ToListAsync();
    }

    public async Task<PlantProfile> GetPlantProfileById(long pId)
    {
        PlantProfile plantProfile = await _greenhouseSystemContext.PlantProfiles!.FirstAsync(p => p.Id == pId);
        return plantProfile;
    }

    public async Task ActivatePlantProfile(long pId)
    {
        PlantProfile plantProfileToAct = GetPlantProfileById(pId).Result;
    }
}