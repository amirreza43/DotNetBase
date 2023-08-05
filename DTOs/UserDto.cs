//import the necessary packages
using System;
using System.ComponentModel.DataAnnotations;    
using System.ComponentModel.DataAnnotations.Schema;
using web.Dtos;

namespace web.Dtos
{
    public class UserDto
    {
        //create attributes per User model
        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string UserName { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string FirstName { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string LastName { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime Dob { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MinLength(8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        
    }
}