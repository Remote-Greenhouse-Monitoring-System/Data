using Contracts;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace EFCData; 


//TODO I do not think the dao should implement the greenHouse (?)

public class GreenHouseDAO : IGreenHouseService{
    
    private readonly GreenhouseSystemContext _greenhouseSystemContext;

    public GreenHouseDAO(GreenhouseSystemContext greenhouseSystemContext)
    {
        _greenhouseSystemContext = greenhouseSystemContext;
    }

    public async Task AddGreenHouse(long uid, GreenHouse greenHouse) {
        await _greenhouseSystemContext!.GreenHouses!.AddAsync(greenHouse);
        User u = await _greenhouseSystemContext.Users!.FirstAsync(u => u.Id == uid);
        u.GreenHouses!.Add(greenHouse);
         _greenhouseSystemContext.Users!.Update(u);
         await _greenhouseSystemContext.SaveChangesAsync();
    }

    public async Task RemoveGreenHouse(long gid) {
        GreenHouse? greenHouse = await _greenhouseSystemContext.GreenHouses!.FindAsync(gid);
        _greenhouseSystemContext.GreenHouses!.Remove(greenHouse!);
        await _greenhouseSystemContext.SaveChangesAsync();
    }

    public async Task UpdateGreenHouse(GreenHouse greenHouse) {
        _greenhouseSystemContext.GreenHouses!.Update(greenHouse);
        await _greenhouseSystemContext.SaveChangesAsync();
        
    }

    public async Task<ICollection<GreenHouse>> GetGreenHouses(long uid) {
        User u= await _greenhouseSystemContext.Users!.Include(u=>u.GreenHouses).FirstAsync(u=>u.Id==uid);
        return u.GreenHouses!;
    }
    
    public async Task<GreenHouse> GetGreenHouseById(long id) {
        return await _greenhouseSystemContext.GreenHouses!.FirstAsync(g=>g.Id==id);
    }
}