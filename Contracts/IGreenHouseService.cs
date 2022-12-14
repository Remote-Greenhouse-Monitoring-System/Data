using Entities;

namespace Contracts; 

public interface IGreenHouseService {
    
    public Task<GreenHouse> AddGreenHouse(long uid, GreenHouse greenHouse);
    public Task<GreenHouse> RemoveGreenHouse(long gid);
    public Task<GreenHouse> UpdateGreenHouse(GreenHouse greenHouse);
    public Task<ICollection<GreenHouse>> GetGreenHouses(long uid);

    Task<GreenHouse> GetLastMeasurementGreenhouse();
    Task<ICollection<GreenhouseLastMeasurement>> GetGreenhousesWithLastMeasurement(long uId);
    
    Task<long> GetGreenhouseIdByEui(string eui);
}
