using Contracts;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace EFCData;

public class MeasurementDAO : IMeasurementService
{
    private readonly GreenhouseContext _greenhouseContext;

    public MeasurementDAO(GreenhouseContext greenhouseContext)
    {
        _greenhouseContext = greenhouseContext;
    }

    public async Task<ICollection<Measurement>> GetMeasurements(long gId, int amount)
    {
        ICollection<Measurement> measurements = await _greenhouseContext.Measurements!.Take(amount)
            .Where(m => m.GreenhouseId == gId)
            .ToListAsync();
        return measurements ;
    }

    public async Task<Measurement> GetLastMeasurement(long gId)
    {
        Measurement measurement = await _greenhouseContext.
            Measurements!
            .Where(m=>m.GreenhouseId==gId)
            .OrderBy(m => m.Timestamp)
            .FirstAsync();
        return measurement;
    }

    public async Task<ICollection<Measurement>> GetAllPerHours(long gId, int hours)
    {

        TimeSpan timeSpan = new TimeSpan(hours, 0, 0);
        ICollection<Measurement> measurements =
            await _greenhouseContext.Measurements!
                .Where(m => m.Timestamp >= DateTime.Now.Subtract(timeSpan) && m.GreenhouseId==gId)
                .ToListAsync();
        return measurements;
    }

    public async Task<ICollection<Measurement>> GetAllPerDays(long gId, int days)
    {
        TimeSpan timeSpan = new TimeSpan(days,0,0,0);
        DateTime temp = DateTime.Now.Subtract(timeSpan);
        ICollection<Measurement> measurements =
            await _greenhouseContext.Measurements!
                .Where(m => m.Timestamp.CompareTo(temp)==1 && m.GreenhouseId==gId)
                .ToListAsync();
        return measurements;
    }

    public async Task<ICollection<Measurement>> GetAllPerMonth(long gId, int month, int year)
    {
        
        ICollection<Measurement> measurements =
            await _greenhouseContext.Measurements!
                .Where(m => m.Timestamp.Month == month && m.Timestamp.Year==year && m.GreenhouseId==gId)
                .ToListAsync();
        return measurements;
    }

    public async Task<ICollection<Measurement>> GetAllPerYear(long gId, int year)
    {
        ICollection<Measurement> measurements =
            await _greenhouseContext.Measurements!
                .Where(m => m.Timestamp.Year == year && m.GreenhouseId==gId)
                .ToListAsync();
        return measurements;
    }

    
    //upload:
    public async Task AddMeasurementAsync(Measurement measurement)
    {
        await Console.Out.WriteLineAsync("measurement - " + measurement +" added to DB");
        // await _greenhouseContext.Measurements!.AddAsync(measurement);
    }
}