//import the necessary packages
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using web.Dtos;



namespace web.Models
{
    public class User
    {
        [Key]
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        [JsonIgnore]
        public string Password { get; set; }
        [JsonIgnore]
        public Guid Salt {get; set;}
        [InverseProperty("Receiver")]
        [JsonIgnore]
        public List<Message> Inbox { get; set; }
        [InverseProperty("Sender")]
        [JsonIgnore]
        public List<Message> Sent { get; set; }
        public User()
        {
            Inbox = new();
            Sent = new();
            Salt = Guid.NewGuid();
        }
        public User(UserDto dto)
        {
            UserName = dto.UserName;
            FirstName = dto.FirstName;
            LastName = dto.LastName;
            Email = dto.Email;
            Password = dto.Password;
            Salt = Guid.NewGuid();
            Inbox = new();
            Sent = new();
        }
    }
}