using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace web.DTOs
{
    public class UpdateUserDto
    {
        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string FirstName { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string LastName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [MinLength(6)]
        [MaxLength(25)]
        public string Password { get; set; }

    }
}