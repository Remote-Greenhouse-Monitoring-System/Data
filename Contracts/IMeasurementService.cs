using Entities;

namespace Contracts;

public interface IMeasurementService
{
    public Task<ICollection<Measurement>> GetMeasurements(long gId, int amount);
    public Task<Measurement> GetLastMeasurement(long gId);
    public Task<ICollection<Measurement>> GetAllPerHours(long gId, int hours);
    public Task<ICollection<Measurement>> GetAllPerDays(long gId, int days);
    public Task<ICollection<Measurement>> GetAllPerMonth(long gId, int month, int year);
    public Task<ICollection<Measurement>> GetAllPerYear(long gId, int year);
}