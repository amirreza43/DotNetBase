using System.ComponentModel.DataAnnotations;

namespace web
{
  public class UserForAuth
  {
    [Required]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }

  }
}