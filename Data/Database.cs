using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using web.Models;
using Npgsql.EntityFrameworkCore.PostgreSQL;


namespace web
{
  public class Database : DbContext
  {
    public Database(DbContextOptions<Database> options) : base(options) {
    }
    public DbSet<User> Users { get; set; }
    public DbSet<Message> Messages { get; set; }

  }
}
