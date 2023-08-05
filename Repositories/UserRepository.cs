using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using web.Interfaces;
using web.Models;
using web;

namespace web.Repositories
{
    public class UserRepository : IUserRepository
    {
        private Database _db;
        public async Task CreateUser(User user)
        {
        await _db.AddAsync(user);
        }
        public async Task<User> GetUser(string username)
        {
        return await _db.Users.Where(u => u.UserName == username).FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<User>> GetAllUsers()
        {
        return await _db.Users.ToListAsync();
        }
        public async Task<User> UpdateUser(User user)
        {
        _db.Update(user);
        return await _db.Users.Where(u => u.UserName == user.UserName).FirstOrDefaultAsync();
        }
        public void DeleteUser(User user)
        {
        _db.Remove(user);
        }
        public async Task SaveAsync()
        {
        await _db.SaveChangesAsync();
        }
        public async Task SendMessage(Message message)
        {
        await _db.AddAsync(message);
        }
        public async Task<IEnumerable<dynamic>> GetInbox(User receiver)
        {
        return await _db.Messages.Where(m => m.Receiver == receiver).Include(m => m.Sender).Select(m => new { m.Id, Sender = m.Sender.UserName, m.Text }).ToListAsync();
        }
        public async Task<IEnumerable<dynamic>> GetSent(User sender)
        {
        return await _db.Messages.Where(m => m.Sender == sender).Include(m => m.Receiver).Select(m => new { m.Id, Receiver = m.Receiver.UserName, m.Text }).ToListAsync();
        }
        public async Task<IEnumerable<dynamic>> GetOneInbox(User sender, User receiver)
        {
        return await _db.Messages.Where(m => m.Receiver == receiver).Where(m => m.Sender == sender).Select(m => new { m.Id, Receiver = m.Receiver.UserName, Sender = m.Sender.UserName, m.Text }).ToListAsync();
        }
        public async Task<IEnumerable<dynamic>> GetOneSent(User sender, User receiver)
        {
        return await _db.Messages.Where(m => m.Sender == sender).Where(m => m.Receiver == receiver).Select(m => new { m.Id, Receiver = m.Receiver.UserName, Sender = m.Sender.UserName, m.Text }).ToListAsync();
        }
        public async Task<dynamic> GetAMessage(Guid id)
        {
        return await _db.Messages.Where(m => m.Id == id).Include(m => m.Sender).Include(m => m.Receiver).Select(m => new { m.Id, Receiver = m.Receiver.UserName, Sender = m.Sender.UserName, m.Text }).FirstOrDefaultAsync();
        }
        public async Task DeleteAMessage(Guid id)
        {
        var message = await _db.Messages.Where(m => m.Id == id).FirstOrDefaultAsync();
        _db.Messages.Remove(message);
        }
        public UserRepository(Database db)
        {
        _db = db;
        }
    }
}