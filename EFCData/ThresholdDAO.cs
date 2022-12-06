using Contracts;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace EFCData;

public class ThresholdDao : IThresholdService
{
    private GreenhouseSystemContext _context;

    public ThresholdDao(GreenhouseSystemContext context)
    {
        _context = context;
    }

    public async Task<Threshold> GetThresholdForPlantProfile(long plantProfileId)
    {
        PlantProfile profile = await _context.PlantProfiles!.Include(p=>p.Threshold).FirstAsync(p => p.Id == plantProfileId);
        return profile.Threshold;
    }

    public async Task UpdateThresholdOnPlantProfile(Threshold threshold, long pId)
    {
        PlantProfile profile = await _context.PlantProfiles!.Include(p=>p.Threshold).FirstAsync(p => p.Id == pId);
        profile.Threshold = threshold;
        _context.PlantProfiles!.Update(profile);
        await _context.SaveChangesAsync();
    }   
}