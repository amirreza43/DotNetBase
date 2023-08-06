using Microsoft.EntityFrameworkCore;
using web;
using web.Interfaces;
using web.Repositories;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);




// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<Database>( opt => {
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});
//Adding a freeing Cors policy
builder.Services.AddCors(opt =>
      {
        opt.AddPolicy(name: builder.Configuration.GetConnectionString("DefaultPolicy"), builder =>
           {
             builder.AllowAnyOrigin()
                     .AllowAnyHeader()
                     .AllowAnyMethod();
           });
      });
//Mapping AppSettings to the AppSettings section of appsettings.json
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

//Adding Context Accessor to allow for User tracking in Controllers
builder.Services.AddHttpContextAccessor();

//Mapping the IUserRepository to the UserRepository
builder.Services.AddScoped<IUserRepository, UserRepository>();
//Mapping the IUserService to the UserService
builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Adding Cors policy to the app
app.UseCors(builder.Configuration.GetConnectionString("DefaultPolicy"));

app.UseAuthorization();

app.MapControllers();

app.Run();
