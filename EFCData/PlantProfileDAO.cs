using Contracts;
using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EFCData;

public class PlantProfileDao : IPlantProfileService
{
    private readonly GreenhouseSystemContext _greenhouseSystemContext;

    public PlantProfileDao(GreenhouseSystemContext greenhouseSystemContext)
    {
        _greenhouseSystemContext = greenhouseSystemContext;
    }
    
    public async Task<PlantProfile> AddPlantProfile(PlantProfile plantP, long userId)
    {
        await _greenhouseSystemContext.Thresholds!.AddAsync(plantP.Threshold);
        await _greenhouseSystemContext.PlantProfiles!.AddAsync(plantP);
        User u  ;
        try
        {
            u=await _greenhouseSystemContext.Users!.Include(u=>u.PlantProfiles).FirstAsync(u => u.Id == userId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("User not found.");
        }
        await _greenhouseSystemContext.SaveChangesAsync();
        
        u.PlantProfiles!.Add(plantP);
        _greenhouseSystemContext.Users!.Update(u);
        
        await _greenhouseSystemContext.SaveChangesAsync();
        return plantP;
    }
    
    public async Task RemovePlantProfile(long pId)
    {
        PlantProfile plantProfile;
        try
        {
            plantProfile=await _greenhouseSystemContext.PlantProfiles!.FirstAsync(p => p.Id == pId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Plant profile not found.");
        }
        _greenhouseSystemContext.PlantProfiles!.Remove(plantProfile);
        await _greenhouseSystemContext.SaveChangesAsync();
    }

    public async Task<PlantProfile> UpdatePlantProfile(PlantProfile plantP)
    {
        try
        {
            _greenhouseSystemContext.PlantProfiles!.Update(plantP);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("A problem occured while updating the plant profile.");
        }
        await _greenhouseSystemContext.SaveChangesAsync();
        return plantP;
    }

    // User not implemented yet
    public async Task<ICollection<PlantProfile>> GetUserPlantProfiles(long uId)
    {
        User u;
        try
        {
            u=await _greenhouseSystemContext.Users!.Include(u=>u.PlantProfiles).FirstAsync(u => u.Id == uId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("User not found.");
        }
        return u.PlantProfiles!;
    }

    public async Task<ICollection<PlantProfile>> GetPreMadePlantProfiles()
    {
        return await _greenhouseSystemContext.PlantProfiles!.ToListAsync();
    }

    public async Task<PlantProfile> GetPlantProfileById(long pId)
    {
        PlantProfile plantProfile;
        try
        {
            plantProfile= await _greenhouseSystemContext.PlantProfiles!.FirstAsync(p => p.Id == pId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Plant profile not found.");
        }
        return plantProfile;
    }

    public async Task ActivatePlantProfile(long pId, long gId)
    {
        PlantProfile plantProfile;
        try
        {
            plantProfile= await _greenhouseSystemContext.PlantProfiles!.FirstAsync(p => p.Id == pId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Plant profile not found.");
        }
        
        
        GreenHouse greenHouse;
        try
        {
            greenHouse= await _greenhouseSystemContext.GreenHouses!.FirstAsync(g => g.Id == gId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Greenhouse not found.");
        }
        
        greenHouse.ActivePlantProfile = plantProfile;
        _greenhouseSystemContext.GreenHouses!.Update(greenHouse);
        await _greenhouseSystemContext.SaveChangesAsync();
    }

    public async Task<PlantProfile> GetActivePlantProfileOnGreenhouse(long gId)
    {
        GreenHouse greenHouse;
        try
        {
            greenHouse= await _greenhouseSystemContext.GreenHouses!.Include(g=>g.ActivePlantProfile).FirstAsync(g => g.Id == gId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Greenhouse not found.");
        }
        if (greenHouse.ActivePlantProfile != null)
        {
            return greenHouse.ActivePlantProfile!;
        }

        throw new Exception($"There is no active plant profile on this greenhouse: {gId} ");
    }
}