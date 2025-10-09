using Microsoft.AspNetCore.Mvc;
using ToDoList.Interfaces;
using ToDoList.Models;

namespace ToDoList.Controllers;

[ApiController]
[Route("[controller]")]
public class ToDoTaskController : ControllerBase
{
    private readonly ILogger<ToDoTaskController> _logger;
    private readonly IToDoTaskService _toDoTaskService;

    public ToDoTaskController(ILogger<ToDoTaskController> logger, IToDoTaskService toDoTaskService)
    {
        _logger = logger;
        _toDoTaskService = toDoTaskService;
    }

    [HttpGet]
    public async Task<IActionResult> GetTasks([FromQuery] string? filter = null)
    {
        var tasks = await _toDoTaskService.GetTasksAsync(filter);
        return Ok(tasks);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTask([FromBody] ToDoTask newTask)
    {
        var createdTask = await _toDoTaskService.CreateTaskAsync(newTask);
        return CreatedAtAction(nameof(GetTasks), new { id = createdTask.id }, createdTask);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTask(int id, [FromBody] ToDoTask updatedTask)
    {
        var result = await _toDoTaskService.UpdateTaskAsync(id, updatedTask);
        if (result == null)
        {
            return BadRequest("Invalid task data.");
        }
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(int id)
    {
        var success = await _toDoTaskService.DeleteTaskAsync(id);
        if (!success)
        {
            return BadRequest("Invalid task ID.");
        }
        return NoContent();
    }
    
}
