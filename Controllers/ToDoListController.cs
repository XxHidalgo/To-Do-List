using Microsoft.AspNetCore.Mvc;
using ToDoList.Services;
using ToDoList.Interfaces;
using ToDoListModel = ToDoList.Models.ToDoList;

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
        var toDoLists = await _toDoListService.GetListsAsync(filter);
        return Ok(toDoLists);
    }

    [HttpPost]
    public async Task<IActionResult> CreateList([FromBody] ToDoListModel newList)
    {
        var createdList = await _toDoListService.CreateListAsync(newList);
        return CreatedAtAction(nameof(GetList), new { id = createdList.id }, createdList);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateList(int id, [FromBody] ToDoListModel updatedList)
    {
        var result = await _toDoListService.UpdateListAsync(id, updatedList);
        if (result == null)
        {
            return BadRequest("Invalid list data.");
        }
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteList(int id)
    {
        var success = await _toDoListService.DeleteListAsync(id);
        if (!success)
        {
            return BadRequest("Invalid list ID.");
        }
        return NoContent();
    }
}
