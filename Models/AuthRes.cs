using web.Models;

namespace web
{
  public class AuthRes
  {
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
    public string Token { get; set; }

    public bool Admin { get; set; }

    public AuthRes(User user, string token)
    {
      FirstName = user.FirstName;
      LastName = user.LastName;
      Username = user.UserName;
      Token = token;

    }
  }
}