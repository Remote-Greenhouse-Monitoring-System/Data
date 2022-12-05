using Entities;

namespace Contracts; 

public interface IGreenHouseService {
    
    public Task<GreenHouse> CreateGreenHouse(long uid, GreenHouse greenHouse);
    public Task RemoveGreenHouse(long gid);
    public Task<GreenHouse> UpdateGreenHouse(GreenHouse greenHouse);
    public Task<ICollection<GreenHouse>> GetGreenHouses(long uid);
    public Task<GreenHouse> AddPlantProfile(long gid, long pid);
    public Task<GreenHouse> GetGreenHouseById(long id);
}