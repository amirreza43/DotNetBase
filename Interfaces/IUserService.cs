using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using web;
using web.Models;
public interface IUserService
{
    AuthRes Authenticate(UserForAuth model);
    Task<User> GetByUserName(string username);
    User GetByUserName1(string username);
    string ComputeSha256Hash(string plainText, Guid salt);
}