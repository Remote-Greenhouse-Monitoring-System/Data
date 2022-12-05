using Contracts;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace EFCData; 


//TODO I do not think the dao should implement the greenHouse (?)

public class GreenHouseDAO : IGreenHouseService{
    
    private readonly GreenhouseSystemContext _greenhouseSystemContext;


    public async Task<GreenHouse> CreateGreenHouse(long uid, GreenHouse greenHouse) {
        await _greenhouseSystemContext.GreenHouses!.AddAsync(greenHouse);
        await _greenhouseSystemContext.SaveChangesAsync();
        return await _greenhouseSystemContext.GreenHouses!.FindAsync(uid);
    }

    public async Task RemoveGreenHouse(long gid) {
        GreenHouse greenHouse = await _greenhouseSystemContext.GreenHouses!.FindAsync(gid);
        _greenhouseSystemContext.GreenHouses!.Remove(greenHouse);
        await _greenhouseSystemContext.SaveChangesAsync();
    }

    public async Task<GreenHouse> UpdateGreenHouse(GreenHouse greenHouse) {
        _greenhouseSystemContext.GreenHouses!.Update(greenHouse);
        await _greenhouseSystemContext.SaveChangesAsync();
        return await _greenhouseSystemContext.GreenHouses!.FindAsync(greenHouse.Id);
    }

    public async Task<ICollection<GreenHouse>> GetGreenHouses(long uid) {
        //TODO: Ninja, the User entity souldnt have a list of greenhouses???
        // return await _greenhouseContext.Users!.Include(u=> u.Id).Where(u => u.Id == uid).Select(u => u.GreenHouses).ToListAsync();
        return null;
    }

    public async Task<GreenHouse> AddPlantProfile(long gid, long pid) {
        //TODO: Emi did this?
        return null;
    }

    public async Task<GreenHouse> GetGreenHouseById(long id) {
        return _greenhouseSystemContext.GreenHouses!.Find(id);
    }
}