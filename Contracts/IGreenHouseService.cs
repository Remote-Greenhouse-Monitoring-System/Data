using Entities;

namespace Contracts; 

public interface IGreenHouseService {
    //public long uid { get; set; }
    public Task<GreenHouse> CreateGreenHouse(GreenHouse greenHouse);
    public Task RemoveGreenHouse(long gid);
    public Task<GreenHouse> UpdateGreenHouse(GreenHouse greenHouse);
    public Task<GreenHouse> AddPlantProfile(long gid, long pid);
    public Task<ICollection<GreenHouse>> GetGreenHouses();
    public Task<GreenHouse> GetGreenHouseById(long id);
}