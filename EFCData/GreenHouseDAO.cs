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

    public async Task<GreenHouse> AddGreenHouse(long uid, GreenHouse greenHouse) {
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

        try
        {
            user.GreenHouses!.Add(greenHouse);
            _greenhouseSystemContext.Users!.Update(user);
            await _greenhouseSystemContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("The greenhouse could not be added." +
                                " It may be because a greenhouse with the same Device EUI already exists.");
        }
         return greenHouse;
    }

    public async Task<GreenHouse> RemoveGreenHouse(long gid)
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

        GreenHouse aux = greenHouse!;
        _greenhouseSystemContext.GreenHouses!.Remove(greenHouse!);
        await _greenhouseSystemContext.SaveChangesAsync();
        return aux;
    }

    public async Task<GreenHouse> UpdateGreenHouse(GreenHouse greenHouse) {
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
        return greenHouse;
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

    public async Task<ICollection<GreenhouseLastMeasurement>> GetGreenhousesWithLastMeasurement(long uId)
    {
        User user;
        ICollection<GreenHouse> greenhouses=new List<GreenHouse>();
        ICollection<GreenhouseLastMeasurement> greenhousesWithLastMeasurements=new List<GreenhouseLastMeasurement>();
        //getting the user with the corresponding ID
        try
        {
            user = await _greenhouseSystemContext.Users!.Include(u=>u.GreenHouses).FirstAsync(u => u.Id == uId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("User not found.");
        }
        
        //iterating through the greenhouses and fetching their measurements and creating the list of custom objects.
        greenhouses = user.GreenHouses!;
        foreach (GreenHouse greenhouse in greenhouses)
        {
            GreenHouse g = await _greenhouseSystemContext.GreenHouses!
                .Include(g => g.Measurements)
                .FirstAsync(g => g.Id == greenhouse.Id);
            greenhouse.Measurements = g.Measurements;
            if (greenhouse.Measurements == null || greenhouse.Measurements!.Count == 0)
            {
                greenhousesWithLastMeasurements.Add(new GreenhouseLastMeasurement(greenhouse.Id,greenhouse.Name, new Measurement(), greenhouse.DeviceEui));
            }
            else
            {
                greenhousesWithLastMeasurements
                    .Add(new GreenhouseLastMeasurement(greenhouse.Id,
                        greenhouse.Name,
                        greenhouse.Measurements!
                            .OrderBy(m=>m.Timestamp)
                            .First(), greenhouse.DeviceEui));
            }
        }
        
       

        return greenhousesWithLastMeasurements;

    }

    public async Task<long> GetGreenhouseIdByEui(string eui)
    {
        long greenHouseId;
        try
        {
            var g = await _greenhouseSystemContext.GreenHouses!.FirstAsync(g=>g.DeviceEui==eui);
            greenHouseId = g.Id;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Greenhouse could not be found.");
        }
        
        return greenHouseId;
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