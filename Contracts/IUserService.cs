using Entities;

namespace Contracts;

public interface IUserService
{
    public Task<User> GetUserByUsername(string username);
    public Task<User> GetUserById(long id);
    public Task<User> GetUserByEmail(string email);
    public Task<User> AddUser(User user);
    public Task RemoveUser(User user);
    public Task<User> UpdateUser(User user);
    public Task<User> LogUserIn(string username, string password);
}