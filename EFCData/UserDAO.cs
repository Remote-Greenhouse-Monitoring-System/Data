using Contracts;
using Entities;
using Microsoft.Data.SqlClient;
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

        ICollection<User> users;
        try
        {
            await _systemContext.Users!.AddAsync(user);
        }
        catch (Exception sqlException)
        {
            Console.WriteLine(sqlException);
            throw new("Something went wrong when adding the user. Please try again. ");
        }
        await _systemContext.SaveChangesAsync();
        return await _systemContext.Users!.FirstAsync(u => u.Email == user.Email);
    }

    public async Task<User> RemoveUser(long userId)

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

        User u = user;
        _systemContext.Users!.Remove(user);
        await _systemContext.SaveChangesAsync();
        return u;
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