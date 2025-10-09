using Microsoft.AspNetCore.Mvc;
using ToDoList.Interfaces;
using ToDoList.Models;

namespace ToDoList.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly IUserService _userService;

    public UserController(ILogger<UserController> logger, IUserService userService)
    {
        _logger = logger;
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers([FromQuery] string? filter = null)
    {
        var users = await _userService.GetUsersAsync(filter);
        return Ok(users);
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] User newUser)
    {
        var createdUser = await _userService.CreateUserAsync(newUser);
        return CreatedAtAction(nameof(GetUsers), new { id = createdUser.id }, createdUser);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] User updatedUser)
    {
        var result = await _userService.UpdateUserAsync(id, updatedUser);
        if (result == null)
        {
            return BadRequest("Invalid user data.");
        }
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var success = await _userService.DeleteUserAsync(id);
        if (!success)
        {
            return BadRequest("Invalid user ID.");
        }
        return NoContent();
    }
    
}
