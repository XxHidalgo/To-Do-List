using Microsoft.AspNetCore.Mvc;
using ToDoList.Interfaces;
using ToDoList.Models.Domain;
using ToDoList.Models.DTOs;
using AutoMapper;

namespace ToDoList.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public UserController(ILogger<UserController> logger, IUserService userService, IMapper mapper)
    {
        _logger = logger;
        _userService = userService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers([FromQuery] string? filter = null)
    {
        var users = await _userService.GetUsersAsync(filter);

        var dto = _mapper.Map<IEnumerable<UserDto>>(users);

        return Ok(dto);
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateOrUpdateUserDto newUserDto)
    {
        var domain = _mapper.Map<User>(newUserDto);

        var createdUser = await _userService.CreateUserAsync(domain);

        var createdDto = _mapper.Map<UserDto>(createdUser);

        return CreatedAtAction(nameof(GetUsers), new { id = createdDto.id }, createdDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] CreateOrUpdateUserDto updatedUserDto)
    {
        var domain = _mapper.Map<User>(updatedUserDto);

        var result = await _userService.UpdateUserAsync(id, domain);

        if (result == null) return BadRequest("Invalid user data.");

        var resultDto = _mapper.Map<UserDto>(result);
        
        return Ok(resultDto);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var success = await _userService.DeleteUserAsync(id);

        if (!success) return BadRequest("Invalid user ID.");

        return NoContent();
    }
    
}
