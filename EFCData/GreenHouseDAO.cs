using Contracts;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace EFCData; 


//TODO I do not think the dao should implement the greenHouse (?)

public class GreenHouseDAO : IGreenHouseService{
    
    private readonly GreenhouseContext _greenhouseContext;


    public async Task<GreenHouse> CreateGreenHouse(long uid, GreenHouse greenHouse) {
        await _greenhouseContext.GreenHouses!.AddAsync(greenHouse);
        await _greenhouseContext.SaveChangesAsync();
        return await _greenhouseContext.GreenHouses!.FindAsync(uid);
    }

    public async Task RemoveGreenHouse(long gid) {
        GreenHouse greenHouse = await _greenhouseContext.GreenHouses!.FindAsync(gid);
        _greenhouseContext.GreenHouses!.Remove(greenHouse);
        await _greenhouseContext.SaveChangesAsync();
    }

    public async Task<GreenHouse> UpdateGreenHouse(GreenHouse greenHouse) {
        _greenhouseContext.GreenHouses!.Update(greenHouse);
        await _greenhouseContext.SaveChangesAsync();
        return await _greenhouseContext.GreenHouses!.FindAsync(greenHouse.GID);
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
        return _greenhouseContext.GreenHouses!.Find(id);
    }
}