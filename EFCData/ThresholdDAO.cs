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
        PlantProfile profile;
        try
        {
            profile= await _context.PlantProfiles!.Include(p=>p.Threshold).FirstAsync(p => p.Id == plantProfileId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Plant profile not found.");
        }
        return profile.Threshold;
    }

    public async Task UpdateThresholdOnPlantProfile(Threshold threshold, long pId)
    {
        PlantProfile profile;
        try
        {
            profile= await _context.PlantProfiles!.Include(p=>p.Threshold).FirstAsync(p => p.Id == pId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Plant profile not found.");
        }
        profile.Threshold = threshold;
        _context.PlantProfiles!.Update(profile);
        await _context.SaveChangesAsync();
    }

    public async Task<Threshold> GetThresholdOnActivePlantProfile(long gId)
    {
        long pId;
        GreenHouse greenHouse;
        try
        {
            greenHouse = await _context.GreenHouses!.Include(g => g.ActivePlantProfile).FirstAsync(g => g.Id == gId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Greenhouse not found.");
        }

        pId = greenHouse.ActivePlantProfile!.Id;
        PlantProfile profile;
        try
        {
            profile = await _context.PlantProfiles!.Include(p => p.Threshold).FirstAsync(p=>p.Id==pId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Plant profile not found.");
        }

        return profile.Threshold;
    }
}