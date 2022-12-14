using Contracts;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace EFCData;

public class MeasurementDao : IMeasurementService
{
    private readonly GreenhouseSystemContext _greenhouseSystemContext;

    public MeasurementDao(GreenhouseSystemContext greenhouseSystemContext)
    {
        _greenhouseSystemContext = greenhouseSystemContext;
    }

    public async Task<ICollection<Measurement>> GetMeasurements(long gId, int amount)
    {
        ICollection<Measurement> measurements = await _greenhouseSystemContext.Measurements!.Take(amount)
            .Where(m => m.GreenhouseId == gId)
            .OrderBy(m=>m.Timestamp)
            .ToListAsync();
        return measurements ;
    }

    public async Task<Measurement> GetLastMeasurement(long gId)
    {
        Measurement measurement = await _greenhouseSystemContext.
            Measurements!
            .Where(m=>m.GreenhouseId==gId)
            .OrderBy(m => m.Timestamp)
            .FirstAsync();
        return measurement;
    }

    public async Task<ICollection<Measurement>> GetAllPerHours(long gId, int hours)
    {

        TimeSpan timeSpan = new TimeSpan(hours, 0, 0);
        DateTime temp = DateTime.Now.Subtract(timeSpan);
        ICollection<Measurement> measurements =
            await _greenhouseSystemContext.Measurements!
                .Where(m => m.GreenhouseId == gId && m.Timestamp > temp)
                .ToListAsync();
        return measurements;
    }

    public async Task<ICollection<Measurement>> GetAllPerDays(long gId, int days)
    {
        TimeSpan timeSpan = new TimeSpan(days,0,0,0);
        DateTime temp = DateTime.Now.Subtract(timeSpan);
        ICollection<Measurement> measurements =
            await _greenhouseSystemContext.Measurements!
                .Where(m => m.Timestamp > temp && m.GreenhouseId==gId)
                .ToListAsync();
        return measurements;
    }

    public async Task<ICollection<Measurement>> GetAllPerMonth(long gId, int month, int year)
    {
            
        ICollection<Measurement> measurements =
            await _greenhouseSystemContext.Measurements!
                .Where(m => m.Timestamp.Month == month && m.Timestamp.Year==year && m.GreenhouseId==gId)
                .ToListAsync();
        return measurements;
    }

    public async Task<ICollection<Measurement>> GetAllPerYear(long gId, int year)
    {
        ICollection<Measurement> measurements =
            await _greenhouseSystemContext.Measurements!
                .Where(m => m.Timestamp.Year == year && m.GreenhouseId==gId)
                .ToListAsync();
        return measurements;
    }

    
    //upload:0004A30B00E8355E
    public async Task AddMeasurement(Measurement measurement, long gId)
    {
        GreenHouse greenHouse;
        try
        {
            greenHouse=await _greenhouseSystemContext.GreenHouses!.Include(g=>g.ActivePlantProfile)
                .Include(g=>g.Measurements)
                .FirstAsync(g=>g.Id==gId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Greenhouse not found.");
        }
        
        greenHouse.Measurements!.Add(measurement);
         _greenhouseSystemContext.Update(greenHouse);
         if (greenHouse.ActivePlantProfile != null)
         {
             PlantProfile profile;
             try
             {
                 profile = await _greenhouseSystemContext.PlantProfiles!.FirstAsync(p =>
                     p.Id == greenHouse.ActivePlantProfile!.Id);
             }
             catch (Exception e)
             {
                 Console.WriteLine(e);
                 throw new Exception("Plant profile not found.");
             }

             profile.Measurements!.Add(measurement);
             _greenhouseSystemContext.Update(profile);
         }

        await _greenhouseSystemContext.Measurements!.AddAsync(measurement);
        await _greenhouseSystemContext.SaveChangesAsync();
        await Console.Out.WriteLineAsync("MeasurementDAO: " + measurement +" added to DB");
    }
    //used to add measurement to all greenhouses with the EUI, just for testing purposes
    public async Task AddMeasurementWithEUI(Measurement measurement, string eui)
    {
        ICollection<GreenHouse> greenHouses;
        try
        {
            greenHouses=await _greenhouseSystemContext.GreenHouses!
                .Include(g=>g.ActivePlantProfile)
                .Include(g=>g.Measurements)
                .Where(g=>g.DeviceEui==eui)
                .ToListAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Greenhouses not found.");
        }

        try
        {
            foreach (var greenHouse in greenHouses)
            {
                Measurement m = new Measurement(greenHouse.Id,measurement.Temperature,measurement.Humidity,measurement.Co2, measurement.Light);
                greenHouse.Measurements!.Add(m);
                await _greenhouseSystemContext.Measurements!.AddAsync(m);
                _greenhouseSystemContext.GreenHouses!.Update(greenHouse);
                await _greenhouseSystemContext.SaveChangesAsync();
            }
        }
        catch (Exception e)
        {
            throw new Exception("Something went wrong when updating the database.");
        }
        await Console.Out.WriteLineAsync("MeasurementDAO: " + measurement +" added to DB");
    }
}