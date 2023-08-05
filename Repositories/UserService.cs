using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using web;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using NETCore.Encrypt;
using Microsoft.Extensions.Configuration;
using web.Models;
using web.DTOs;
using web.Interfaces;

namespace web.Repositories
{
    public class UserService
    {
        // users hardcoded for simplicity, store in a db with hashed passwords in production applications
        private List<User> _users = new List<User>
            {
                new User { FirstName = "Test", LastName = "User", UserName = "test", Password = "test" }
            };
        private Database _db;

        private readonly AppSettings _appSettings;
        private string cryptKey;
        private string cryptIV;
        private string _salt;

        public UserService(IOptions<AppSettings> appSettings, Database db)
        {
        _db = db;
        _appSettings = appSettings.Value;
        _salt = Environment.GetEnvironmentVariable("SALT_STRING");
        }

        public AuthRes Authenticate(UserForAuth model)
        {
        var usersalt = _db.Users.SingleOrDefault(x => x.UserName == model.Username);
        string hashedpass = ComputeSha256Hash(model.Password, usersalt.Salt);
        var user = _db.Users.SingleOrDefault(x => x.UserName == model.Username && x.Password == hashedpass);

        // return null if user not found
        if (user == null) return null;

        // authentication successful so generate jwt token
        var token = generateJwtToken(user);

        return new AuthRes(user, token);

        }


        public async Task<User> GetByUserName(string username)
        {
        return await _db.Users.Where(u => u.UserName == username).FirstOrDefaultAsync();
        }
        public User GetByUserName1(string username)
        {
        return _db.Users.Where(u => u.UserName == username).FirstOrDefault();
        }
        // helper methods

        private string generateJwtToken(User user)
        {
        // generate token that is valid for 7 days
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim("UserName", user.UserName.ToString()) }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
        }
        public string ComputeSha256Hash(string rawData, Guid salt)
        {
        var password = rawData;
        var saltedData = string.Concat(rawData, salt);
        // Create a SHA256   
        using (SHA256 sha256Hash = SHA256.Create())
        {
            // ComputeHash - returns byte array  
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(saltedData));

            // Convert byte array to a string   
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
            builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }
        }
    }
}