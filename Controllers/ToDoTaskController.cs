using Microsoft.AspNetCore.Mvc;
using ToDoList.Interfaces;
using ToDoList.Models.Domain;
using ToDoList.Models.DTOs;
using ToDoTaskModel = ToDoList.Models.Domain.ToDoTask;

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

        var dto = tasks.Select(t => new ToDoTaskDto
        {
            id = t.id,
            toDoList_id = t.toDoList_id,
            title = t.title,
            description = t.description,
            dueDate = t.dueDate,
            isCompleted = t.isCompleted
        });

        return Ok(dto);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTask([FromBody] CreateOrUpdateToDoTaskDto newTaskDto)
    {
        var domain = new ToDoTaskModel
        {
            toDoList_id = newTaskDto.toDoList_id,
            title = newTaskDto.title,
            description = newTaskDto.description,
            dueDate = newTaskDto.dueDate,
            isCompleted = newTaskDto.isCompleted
        };

        var createdTask = await _toDoTaskService.CreateTaskAsync(domain);

        var createdDto = new ToDoTaskDto
        {
            id = createdTask.id,
            toDoList_id = createdTask.toDoList_id,
            title = createdTask.title,
            description = createdTask.description,
            dueDate = createdTask.dueDate,
            isCompleted = createdTask.isCompleted
        };

        return CreatedAtAction(nameof(GetTasks), new { id = createdDto.id }, createdDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTask(int id, [FromBody] CreateOrUpdateToDoTaskDto updatedTaskDto)
    {
        var domain = new ToDoTaskModel
        {
            id = id,
            toDoList_id = updatedTaskDto.toDoList_id,
            title = updatedTaskDto.title,
            description = updatedTaskDto.description,
            dueDate = updatedTaskDto.dueDate,
            isCompleted = updatedTaskDto.isCompleted
        };

        var result = await _toDoTaskService.UpdateTaskAsync(id, domain);
        if (result == null)
        {
            return BadRequest("Invalid task data.");
        }

        var resultDto = new ToDoTaskDto
        {
            id = result.id,
            toDoList_id = result.toDoList_id,
            title = result.title,
            description = result.description,
            dueDate = result.dueDate,
            isCompleted = result.isCompleted
        };

        return Ok(resultDto);
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
