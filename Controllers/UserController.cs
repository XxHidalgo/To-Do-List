using Microsoft.AspNetCore.Mvc;
using ToDoList.Interfaces;
using ToDoList.Models.Domain;
using ToDoList.Models.DTOs;
using AutoMapper;
using ToDoList.CustomActionFilters;

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
    public async Task<IActionResult> GetUsers(
        [FromQuery] string? filterOn = null,
        [FromQuery] string? filterQuery = null,
        [FromQuery] string? sortBy = null,
        [FromQuery] bool sortDescending = false
    )
    {
        var users = await _userService.GetUsersAsync(filterOn, filterQuery, sortBy, sortDescending);

        var dto = _mapper.Map<IEnumerable<UserDto>>(users);

        return Ok(dto);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(int id)
    {
        var user = await _userService.GetUserByIdAsync(id);

        if (user == null) return NotFound();

        var dto = _mapper.Map<UserDto>(user);

        return Ok(dto);
    }

    [HttpPost]
    [ValidateModel]
    public async Task<IActionResult> CreateUser([FromBody] CreateOrUpdateUserDto newUserDto)
    {
        var domain = _mapper.Map<User>(newUserDto);

        var createdUser = await _userService.CreateUserAsync(domain);

        var createdDto = _mapper.Map<UserDto>(createdUser);

        return CreatedAtAction(nameof(GetUsers), new { id = createdDto.id }, createdDto);
    }

    [HttpPut("{id}")]
    [ValidateModel]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] CreateOrUpdateUserDto updatedUserDto)
    {
        var domain = _mapper.Map<User>(updatedUserDto);

        var result = await _userService.UpdateUserAsync(id, domain);

        if (result == null) return NotFound("Invalid user data.");

        var resultDto = _mapper.Map<UserDto>(result);
        
        return Ok(resultDto);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var success = await _userService.DeleteUserAsync(id);

        if (!success) return NotFound("Invalid user ID.");

        return NoContent();
    }
    
}
