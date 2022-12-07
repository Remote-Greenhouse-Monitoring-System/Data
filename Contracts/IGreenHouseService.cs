using Entities;

namespace Contracts; 

public interface IGreenHouseService {
    
    public Task AddGreenHouse(long uid, GreenHouse greenHouse);
    public Task RemoveGreenHouse(long gid);
    public Task UpdateGreenHouse(GreenHouse greenHouse);
    public Task<ICollection<GreenHouse>> GetGreenHouses(long uid);

    Task<GreenHouse> GetLastMeasurementGreenhouse();
    Task<ICollection<GreenhouseLastMeasurement>> GetGreenhousesWithLastMeasurement(long uId);
}