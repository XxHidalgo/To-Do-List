using Microsoft.AspNetCore.Mvc;
using ToDoList.Interfaces;
using ToDoList.Models.Domain;
using ToDoList.Models.DTOs;
using AutoMapper;

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
    public async Task<IActionResult> GetTasks([FromQuery] string? filter = null)
    {
        var tasks = await _toDoTaskService.GetTasksAsync(filter);

        var dto = _mapper.Map<IEnumerable<ToDoTaskDto>>(tasks);

        return Ok(dto);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTask([FromBody] CreateOrUpdateToDoTaskDto newTaskDto)
    {
        var domain = _mapper.Map<ToDoTask>(newTaskDto);

        var createdTask = await _toDoTaskService.CreateTaskAsync(domain);

        var createdDto = _mapper.Map<ToDoTaskDto>(createdTask);

        return CreatedAtAction(nameof(GetTasks), new { id = createdDto.id }, createdDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTask(int id, [FromBody] CreateOrUpdateToDoTaskDto updatedTaskDto)
    {
        var domain = _mapper.Map<ToDoTask>(updatedTaskDto);

        var result = await _toDoTaskService.UpdateTaskAsync(id, domain);

        if (result == null) return BadRequest("Invalid task data.");

        var resultDto = _mapper.Map<ToDoTaskDto>(result);
        return Ok(resultDto);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(int id)
    {
        var success = await _toDoTaskService.DeleteTaskAsync(id);
        
        if (!success) return BadRequest("Invalid task ID.");

        return NoContent();
    }
    
}
