﻿using Contracts;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace EFCData;

public class MeasurementDAO : IMeasurementService
{
    private readonly GreenhouseSystemContext _greenhouseSystemContext;

    public MeasurementDAO(GreenhouseSystemContext greenhouseSystemContext)
    {
        _greenhouseSystemContext = greenhouseSystemContext;
    }

    public async Task<ICollection<Measurement>> GetMeasurements(long gId, int amount)
    {
        ICollection<Measurement> measurements = await _greenhouseSystemContext.Measurements!.Take(amount)
            .Where(m => m.GreenhouseId == gId)
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

    
    //upload:
    public async Task AddMeasurementAsync(Measurement measurement)
    {
        await _greenhouseSystemContext.Measurements!.AddAsync(measurement);
        await _greenhouseSystemContext.SaveChangesAsync();
        await Console.Out.WriteLineAsync("MeasurementDAO: " + measurement +" added to DB");
    }
}