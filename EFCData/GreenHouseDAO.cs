using Contracts;
using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace EFCData; 


//TODO I do not think the dao should implement the greenHouse (?)

public class GreenHouseDao : IGreenHouseService{
    
    private readonly GreenhouseSystemContext _greenhouseSystemContext;

    public GreenHouseDao(GreenhouseSystemContext greenhouseSystemContext)
    {
        _greenhouseSystemContext = greenhouseSystemContext;
    }

    public async Task AddGreenHouse(long uid, GreenHouse greenHouse) {
        await _greenhouseSystemContext!.GreenHouses!.AddAsync(greenHouse);
        User user;
        try
        {
            user = await _greenhouseSystemContext.Users!.FirstAsync(u => u.Id == uid);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("User could not be found.");
        }

        user.GreenHouses!.Add(greenHouse);
         _greenhouseSystemContext.Users!.Update(user);
         await _greenhouseSystemContext.SaveChangesAsync();
    }

    public async Task RemoveGreenHouse(long gid)
    {
        GreenHouse? greenHouse;
        try
        {
             greenHouse= await _greenhouseSystemContext.GreenHouses!.FindAsync(gid);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Greenhouse could not be found.");
        }
        _greenhouseSystemContext.GreenHouses!.Remove(greenHouse!);
        await _greenhouseSystemContext.SaveChangesAsync();
    }

    public async Task UpdateGreenHouse(GreenHouse greenHouse) {
        try
        {
            _greenhouseSystemContext.GreenHouses!.Update(greenHouse);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Greenhouse could not be found.");
        }
        await _greenhouseSystemContext.SaveChangesAsync();
        
    }

    public async Task<ICollection<GreenHouse>> GetGreenHouses(long uid)
    {
        User user;
        try
        {
             user= await _greenhouseSystemContext.Users!
                .Include(u=>u.GreenHouses).FirstAsync(u=>u.Id==uid);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("User could not be found.");
        }
        return user.GreenHouses!;
    }

    public async Task<GreenHouse> GetLastMeasurementGreenhouse()
    {
        Measurement measurement;
        try
        {
            measurement= await _greenhouseSystemContext.Measurements!.OrderBy(m => m.Timestamp).FirstAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("There are no measurements available.");
        }

        GreenHouse greenHouse;

        try
        {
            greenHouse = await _greenhouseSystemContext.GreenHouses!.FirstAsync(g => g.Id == measurement.GreenhouseId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception(
                "Something went wrong. Either there are no measurements available or they are not linked with any greenhouses.");
        }

        return greenHouse;
    }

    public async Task<ICollection<GreenhouseLastMeasurement>> GetGreenhousesWithMeasurement()
    {
        ICollection<GreenHouse> greenhouses=new List<GreenHouse>();
        ICollection<GreenhouseLastMeasurement> greenhousesWithLastMeasurements=new List<GreenhouseLastMeasurement>();

        try
        {
            greenhouses= await _greenhouseSystemContext.GreenHouses!
                .Include(g => g.Measurements)
                .ToListAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("No greenhouses found.");
        }

        foreach (var greenHouse in greenhouses)
        {
            if (greenHouse.Measurements == null || greenHouse.Measurements!.Count == 0)
            {
                greenhousesWithLastMeasurements.Add(new GreenhouseLastMeasurement(greenHouse.Id,greenHouse.Name, new Measurement()));
            }
            else
            {
                greenhousesWithLastMeasurements
                    .Add(new GreenhouseLastMeasurement(greenHouse.Id,
                        greenHouse.Name,
                        greenHouse.Measurements!
                            .OrderBy(m=>m.Timestamp)
                            .First()));
            }
        
        }

        return greenhousesWithLastMeasurements;

    }

    public async Task<GreenHouse> GetGreenHouseById(long id)
    {
        GreenHouse greenHouse;
        try
        {
            greenHouse=await _greenhouseSystemContext.GreenHouses!.FirstAsync(g=>g.Id==id);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Greenhouse could not be found.");
        }
        
        return greenHouse;
    }
}