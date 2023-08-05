using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace web.DTOs
{
  public class MessageDto
  {
    [Required]
    [MinLength(3)]
    [MaxLength(500)]
    public string Text {get; set;}
  }
}