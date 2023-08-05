using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using web.DTOs;

namespace web.Models
{
    public class Message
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public User Receiver { get; set; }
        public User Sender { get; set; }
        public Message()
            {
                Sender = new();
                Receiver = new();
            }
        public Message(MessageDto dto)
        {
        Id = Guid.NewGuid();
        Text = dto.Text;
        Sender = new();
        Receiver = new();
        }
    }
}