using Contracts;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace EFCData;

public class UserDAO : IUserService
{
    private readonly GreenhouseContext _context;

    public UserDAO(GreenhouseContext _context)
    {
        this._context = _context;
    }

    public async Task<User> GetUserByUsername(string username)
    {
       return await _context.Users!.FirstAsync(u => u.Username == username);
    }

    public async Task<User> GetUserById(long id)
    {
        return await _context.Users!.FirstAsync(u => u.Id == id);
    }

    public async Task<User> GetUserByEmail(string email)
    {
        return await _context.Users!.FirstAsync(u => u.Email == email);
    }

    public async Task<User> AddUser(User user)
    {
        await _context.Users!.AddAsync(user);
        await _context.SaveChangesAsync();
        return await _context.Users!.FirstAsync(u => u.Email == user.Email);


    }

    public async Task RemoveUser(long userId)

    {
        User u = await _context.Users.FirstAsync(u => u.Id == userId);
        await Task.Run((() =>
        {
            _context.Users!.Remove(u);
        }));
        await _context.SaveChangesAsync();
        
    }

    public async Task<User> UpdateUser(User user)
    {
        _context.Users!.Update(user);

        await _context.SaveChangesAsync();

        return await _context.Users!.FirstAsync(u => u.Email == user.Email);
    }

    public async Task<User> LogUserIn(string username, string password)
    {
        User user=await _context.Users!.FirstAsync(u => u.Username == username);
        if (user.Password == password)
        {
            return user;
        }
        throw new Exception("The password provided does not match");
    }
}