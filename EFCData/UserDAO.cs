using Contracts;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace EFCData;

public class UserDao : IUserService
{
    private readonly GreenhouseSystemContext _systemContext;

    public UserDao(GreenhouseSystemContext systemContext)
    {
        this._systemContext = systemContext;
    }

    public async Task<User> GetUserByUsername(string username)
    {
        try
        {
            return await _systemContext.Users!.FirstAsync(u => u.Username == username);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("There is no user with that username.");
        }
    }

    public async Task<User> GetUserById(long id)
    {
        try
        {
            return await _systemContext.Users!.FirstAsync(u => u.Id == id);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("There is no user with that id.");
        }
    }

    public async Task<User> GetUserByEmail(string email)
    {
        try
        {
            return await _systemContext.Users!.FirstAsync(u => u.Email == email);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("There is no user with that email.");
        }
    }

    public async Task<User> AddUser(User user)
    {
        await _systemContext.Users!.AddAsync(user);
        await _systemContext.SaveChangesAsync();
        return await _systemContext.Users!.FirstAsync(u => u.Email == user.Email);


    }

    public async Task RemoveUser(long userId)

    {
        User user;
        try
        {
            user= await _systemContext.Users!.FirstAsync(u => u.Id == userId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("There is no user with that id.");
        }
        _systemContext.Users!.Remove(user);
        await _systemContext.SaveChangesAsync();
    }

    public async Task<User> UpdateUser(User user)
    {
        try
        {
            _systemContext.Users!.Update(user);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Something wrong happened while updating the user.");
        }

        await _systemContext.SaveChangesAsync();

        return await _systemContext.Users!.FirstAsync(u => u.Email == user.Email);
    }

    //TODO Add some kind of encryption to the password 
    //Use PasswordHasher
    public async Task<User> LogUserIn(string email, string password)
    {
        User user;
        try
        {
            user=await _systemContext.Users!.FirstAsync(u => u.Email == email);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("User with the provided email not found");
        }
        if (user.Password == password)
        {
            return user;
        }
        throw new Exception("The credentials provided are incorrect.");
    }
}