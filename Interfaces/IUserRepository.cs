using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using web.Models;

namespace web.Interfaces
{
    public interface IUserRepository
    {
    Task CreateUser(User user);
    Task<User> GetUser(string username);
    Task<IEnumerable<User>> GetAllUsers();
    Task<User> UpdateUser(User user);
    void DeleteUser(User user);
    Task SendMessage(Message message);
    Task<IEnumerable<dynamic>> GetInbox(User receiver);
    Task<IEnumerable<dynamic>> GetSent(User sender);
    Task<IEnumerable<dynamic>> GetOneInbox(User sender, User receiver);
    Task<IEnumerable<dynamic>> GetOneSent(User sender, User receiver);
    Task<dynamic> GetAMessage(Guid id);
    Task DeleteAMessage(Guid id);
    Task SaveAsync();
    }
}