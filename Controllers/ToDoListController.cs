using Microsoft.AspNetCore.Mvc;
using ToDoList.Services;
using ToDoList.Interfaces;
using ToDoList.Models.DTOs;
using ToDoListModel = ToDoList.Models.Domain.ToDoList;

namespace ToDoList.Controllers;

[ApiController]
[Route("[controller]")]
public class ToDoListController : ControllerBase
{
    private readonly ILogger<ToDoListController> _logger;
    private readonly IToDoListService _toDoListService;
    public ToDoListController(ILogger<ToDoListController> logger, IToDoListService toDoListService)
    {
        _logger = logger;
        _toDoListService = toDoListService;
    }

    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] string? filter = null)
    {
        var domainLists = await _toDoListService.GetListsAsync(filter);

        var dtoLists = domainLists.Select(d => new ToDoListDto
        {
            id = d.id,
            user_id = d.user_id,
            title = d.title,
            description = d.description,
            isCompleted = d.isCompleted
        });

        return Ok(dtoLists);
    }

    [HttpPost]
    public async Task<IActionResult> CreateList([FromBody] CreateOrUpdateToDoListDto newListDto)
    {
        var domain = new ToDoListModel
        {
            user_id = newListDto.user_id,
            title = newListDto.title,
            description = newListDto.description,
            isCompleted = newListDto.isCompleted
        };

        var created = await _toDoListService.CreateListAsync(domain);

        var createdDto = new ToDoListDto
        {
            id = created.id,
            user_id = created.user_id,
            title = created.title,
            description = created.description,
            isCompleted = created.isCompleted
        };

        return CreatedAtAction(nameof(GetList), new { id = createdDto.id }, createdDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateList(int id, [FromBody] CreateOrUpdateToDoListDto updatedDto)
    {
        var domain = new ToDoListModel
        {
            id = id,
            user_id = updatedDto.user_id,
            title = updatedDto.title,
            description = updatedDto.description,
            isCompleted = updatedDto.isCompleted
        };
        
        var result = await _toDoListService.UpdateListAsync(id, domain);

        if (result == null) return BadRequest("Invalid list data.");

        var resultDto = new ToDoListDto 
        {
            id = result.id,
            user_id = result.user_id,
            title = result.title,
            description = result.description,
            isCompleted = result.isCompleted
        };

        return Ok(resultDto);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteList(int id)
    {
        var success = await _toDoListService.DeleteListAsync(id);

        if (!success) return BadRequest("Invalid list ID.");

        return NoContent();
    }
}
