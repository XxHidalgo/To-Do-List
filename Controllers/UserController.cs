using Microsoft.AspNetCore.Mvc;
using ToDoList.Interfaces;
using ToDoList.Models.Domain;
using ToDoList.Models.DTOs;
using UserModel = ToDoList.Models.Domain.User;

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

        var dto = users.Select(u => new UserDto
        {
            id = u.id,
            username = u.username,
            email = u.email
        });

        return Ok(dto);
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateOrUpdateUserDto newUserDto)
    {
        var domain = new UserModel
        {
            username = newUserDto.username,
            email = newUserDto.email,
            password = newUserDto.password
        };

        var createdUser = await _userService.CreateUserAsync(domain);

        var createdDto = new UserDto
        {
            id = createdUser.id,
            username = createdUser.username,
            email = createdUser.email
        };

        return CreatedAtAction(nameof(GetUsers), new { id = createdDto.id }, createdDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] CreateOrUpdateUserDto updatedUserDto)
    {
        var domain = new UserModel
        {
            id = id,
            username = updatedUserDto.username,
            email = updatedUserDto.email,
            password = updatedUserDto.password
        };

        var result = await _userService.UpdateUserAsync(id, domain);
        if (result == null)
        {
            return BadRequest("Invalid user data.");
        }

        var resultDto = new UserDto
        {
            id = result.id,
            username = result.username,
            email = result.email
        };

        return Ok(resultDto);
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
