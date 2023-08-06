using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using web.Models;
using web.DTOs;
using web.Interfaces;
using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using web.Dtos;

namespace web
{

  public record brandModel(string brand, List<string> models);
  public class UserController : ControllerBase
  {
    private User _user;
    private IUserRepository _repository;
    private IUserService _userService;


    public UserController(IUserRepository repository, IUserService userService, IHttpContextAccessor httpContextAccessor)
    {
      _user = (User)httpContextAccessor.HttpContext.Items["User"];
      _repository = repository;
      _userService = userService;

    }

    [HttpPost("users")]
    public async Task<IActionResult> CreateUser(UserDto dto)
    {
      User newUser = new User(dto);
      newUser.Password = _userService.ComputeSha256Hash(newUser.Password, newUser.Salt);
      var users = await _repository.GetAllUsers();
      foreach (var user in users)
      {
        if (user.UserName == newUser.UserName)
        {
          return BadRequest(new { message = "Username is Taken." });
        }
      }
      await _repository.CreateUser(newUser);
      await _repository.SaveAsync();
      return CreatedAtAction("GetUser", new { newUser.UserName }, newUser);
    }
    [Authorize]
    [HttpGet("users/{UserName}")]
    public async Task<IActionResult> GetUser(string userName)
    {
      var user = await _repository.GetUser(userName);
      if (user is null) return NotFound();
      return Ok(user);
    }
    [Authorize]
    [HttpGet("users/all")]
    public async Task<IActionResult> GetAllUsers()
    {
      var users = await _repository.GetAllUsers();
      return Ok(users);
    }
    [Authorize]
    [HttpDelete("users/{UserName}")]
    public async Task<IActionResult> DeleteUser(string userName)
    {
      var user = await _repository.GetUser(userName);
      if (user is null) return NotFound();
      if (_user.UserName != user.UserName)
      {
        return BadRequest("Not Authorized");
      }
      _repository.DeleteUser(user);
      await _repository.SaveAsync();
      return Ok("User was deleted.");
    }
    [Authorize]
    [HttpPatch("users/{UserName}")]
    public async Task<IActionResult> UpdateUser(string userName, UpdateUserDto userDto)
    {
      var user = await _repository.GetUser(userName);
      if (user is null) return NotFound();
      if (_user.UserName != user.UserName)
      {
        return BadRequest("Not Authorized");
      }
      userDto.Password = _userService.ComputeSha256Hash(userDto.Password, user.Salt);
      user.FirstName = userDto.FirstName;
      user.LastName = userDto.LastName;
      user.Email = userDto.Email;
      user.Password = userDto.Password;
      var newUser = await _repository.UpdateUser(user);
      await _repository.SaveAsync();
      return Ok(newUser);
    }
    [Authorize]
    [HttpPost("messages/{senderId}/{receiverId}")]
    public async Task<IActionResult> SendMessage(string senderId, string receiverId, MessageDto dto)
    {
      var sender = await _repository.GetUser(senderId);
      if (sender is null) return NotFound();
      var receiver = await _repository.GetUser(receiverId);
      if (receiver is null) return NotFound();
      Message message = new Message(dto) { Sender = sender, Receiver = receiver };
      await _repository.SendMessage(message);
      await _repository.SaveAsync();
      return CreatedAtAction("GetAMessage", new { message.Id }, message);
    }
    [Authorize]
    [HttpGet("messages/inbox/{senderId}/{receiverId}")]
    public async Task<IActionResult> GetOneInbox(string senderId, string receiverId)
    {
      if (_user.UserName != receiverId)
      {
        return BadRequest("Not Authorized");
      }
      var sender = await _repository.GetUser(senderId);
      if (sender is null) return NotFound();
      var receiver = await _repository.GetUser(receiverId);
      if (receiver is null) return NotFound();

      var messages = await _repository.GetOneInbox(sender, receiver);
      return Ok(messages);
    }
    [Authorize]
    [HttpGet("messages/sent/{senderId}/{receiverId}")]
    public async Task<IActionResult> GetOneSent(string senderId, string receiverId)
    {
      if (_user.UserName != senderId)
      {
        return BadRequest("Not Authorized");
      }
      var sender = await _repository.GetUser(senderId);
      if (sender is null) return NotFound();
      var receiver = await _repository.GetUser(receiverId);
      if (receiver is null) return NotFound();

      var messages = await _repository.GetOneSent(sender, receiver);
      return Ok(messages);
    }
    [Authorize]
    [HttpGet("users/{UserName}/inbox")]
    public async Task<IActionResult> GetInbox(string userName)
    {
      var user = await _repository.GetUser(userName);
      if (user is null) return NotFound();
      var inbox = await _repository.GetInbox(user);
      return Ok(inbox);
    }
    [Authorize]
    [HttpGet("users/{UserName}/sent")]
    public async Task<IActionResult> GetSent(string userName)
    {
      var user = await _repository.GetUser(userName);
      if (user is null) return NotFound();
      var sent = await _repository.GetSent(user);
      return Ok(sent);
    }
    [Authorize]
    [HttpGet("messages/{id}")]
    public async Task<IActionResult> GetAMessage(Guid id)
    {
      var message = await _repository.GetAMessage(id);
      if (message is null) return NotFound();
      Console.WriteLine($"Inside our controller {message.Text}");
      return Ok(message);
    }
    [Authorize]
    [HttpDelete("messages/{id}")]
    public async Task<IActionResult> DeleteMessage(Guid id)
    {
      var message = await _repository.GetAMessage(id);
      if (message is null) return NotFound();
      await _repository.DeleteAMessage(id);
      await _repository.SaveAsync();
      return Ok("Message was deleted.");
    }
    [HttpPost("authenticate")]
    public IActionResult Authenticate(UserForAuth model)
    {

      var response = _userService.Authenticate(model);

      if (response == null)
        return BadRequest(new { message = "Username or password is incorrect" });

      return Ok(response);
    }

    [Authorize]
    [HttpGet]
    public IActionResult GetAll()
    {
      return Ok("Authorized");
    }

  }
}
