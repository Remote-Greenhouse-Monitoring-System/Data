using Contracts;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace EFCData;

public class UserDAO : IUserService
{
    private readonly GreenhouseSystemContext _systemContext;

    public UserDAO(GreenhouseSystemContext systemContext)
    {
        this._systemContext = systemContext;
    }

    public async Task<User> GetUserByUsername(string username)
    {
       return await _systemContext.Users!.FirstAsync(u => u.Username == username);
    }

    public async Task<User> GetUserById(long id)
    {
        return await _systemContext.Users!.FirstAsync(u => u.Id == id);
    }

    public async Task<User> GetUserByEmail(string email)
    {
        return await _systemContext.Users!.FirstAsync(u => u.Email == email);
    }

    public async Task<User> AddUser(User user)
    {
        await _systemContext.Users!.AddAsync(user);
        await _systemContext.SaveChangesAsync();
        return await _systemContext.Users!.FirstAsync(u => u.Email == user.Email);


    }

    public async Task RemoveUser(long userId)

    {
        User u = await _systemContext.Users.FirstAsync(u => u.Id == userId);
        await Task.Run((() =>
        {
            _systemContext.Users!.Remove(u);
        }));
        await _systemContext.SaveChangesAsync();
        
    }

    public async Task<User> UpdateUser(User user)
    {
        _systemContext.Users!.Update(user);

        await _systemContext.SaveChangesAsync();

        return await _systemContext.Users!.FirstAsync(u => u.Email == user.Email);
    }

    public async Task<User> LogUserIn(string email, string password)
    {
        User user=await _systemContext.Users!.FirstAsync(u => u.Email == email);
        if (user.Password == password)
        {
            return user;
        }
        throw new Exception("The credentials provided are incorrect.");
    }
}