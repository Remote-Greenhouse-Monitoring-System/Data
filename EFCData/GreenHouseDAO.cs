using Contracts;
using Entities;

namespace EFCData; 


//TODO I do not think the dao should implement the greenHouse (?)

public class GreenHouseDAO : IGreenHouseService{
    
    // public long uid { get; set; }
    //
    // public GreenHouseDAO(long uid) {
    //     this.uid = uid;
    // }

    public Task<GreenHouse> CreateGreenHouse(GreenHouse greenHouse) {
        throw new NotImplementedException();
    }

    public Task RemoveGreenHouse(long id) {
        throw new NotImplementedException();
    }

    public Task<GreenHouse> UpdateGreenHouse(GreenHouse greenHouse) {
        throw new NotImplementedException();
    }

    public Task<GreenHouse> AddPlantProfile(long GreenHouseID, long PlantProfileID) {
        throw new NotImplementedException();
    }

    public Task<ICollection<GreenHouse>> GetGreenHouses() {
        throw new NotImplementedException();
    }

    public Task<GreenHouse> GetGreenHouseById(long id) {
        throw new NotImplementedException();
    }
}