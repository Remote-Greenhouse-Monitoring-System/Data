using Entities;

namespace Contracts; 

public interface GreenHouseService {
    public Task<GreenHouse> CreateGreenHouse(GreenHouse greenHouse);
    public Task RemoveGreenHouse(long id);
    public Task<GreenHouse> UpdateGreenHouse(GreenHouse greenHouse);
    public Task<GreenHouse> AddPlantProfile(long GreenHouseID, long PlantProfileID);
    public Task<ICollection<GreenHouse>> GetGreenHouses();
    public Task<GreenHouse> GetGreenHouseById(long id);
}