using Microsoft.AspNetCore.Mvc;
using ToDoList.Interfaces;
using ToDoList.Models.Domain;
using ToDoList.Models.DTOs;
using AutoMapper;
using ToDoList.CustomActionFilters;

namespace ToDoList.Controllers;

[ApiController]
[Route("[controller]")]
public class ToDoTaskController : ControllerBase
{
    private readonly ILogger<ToDoTaskController> _logger;
    private readonly IToDoTaskService _toDoTaskService;
    private readonly IMapper _mapper;

    public ToDoTaskController(ILogger<ToDoTaskController> logger, IToDoTaskService toDoTaskService, IMapper mapper)
    {
        _logger = logger;
        _toDoTaskService = toDoTaskService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetTasks(
        [FromQuery] string? filterOn = null,
        [FromQuery] string? filterQuery = null,
        [FromQuery] string? sortBy = null,
        [FromQuery] bool sortDescending = false
    )
    {
        var tasks = await _toDoTaskService.GetTasksAsync(filterOn, filterQuery, sortBy, sortDescending);

        var dto = _mapper.Map<IEnumerable<ToDoTaskDto>>(tasks);

        return Ok(dto);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTask(int id)
    {
        var task = await _toDoTaskService.GetTaskByIdAsync(id);

        if (task == null) return NotFound();

        var dto = _mapper.Map<ToDoTaskDto>(task);

        return Ok(dto);
    }

    [HttpPost]
    [ValidateModel]
    public async Task<IActionResult> CreateTask([FromBody] CreateOrUpdateToDoTaskDto newTaskDto)
    {
        var domain = _mapper.Map<ToDoTask>(newTaskDto);

        var createdTask = await _toDoTaskService.CreateTaskAsync(domain);

        var createdDto = _mapper.Map<ToDoTaskDto>(createdTask);

        return CreatedAtAction(nameof(GetTasks), new { id = createdDto.id }, createdDto);
    }

    [HttpPut("{id}")]
    [ValidateModel]
    public async Task<IActionResult> UpdateTask(int id, [FromBody] CreateOrUpdateToDoTaskDto updatedTaskDto)
    {        
        var domain = _mapper.Map<ToDoTask>(updatedTaskDto);

        var result = await _toDoTaskService.UpdateTaskAsync(id, domain);

        if (result == null) return NotFound("Invalid task data.");

        var resultDto = _mapper.Map<ToDoTaskDto>(result);
        return Ok(resultDto);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(int id)
    {
        var success = await _toDoTaskService.DeleteTaskAsync(id);
        
        if (!success) return NotFound("Invalid task ID.");

        return NoContent();
    }
    
}
