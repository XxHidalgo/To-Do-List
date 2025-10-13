using Microsoft.AspNetCore.Mvc;
using ToDoList.Services;
using ToDoList.Interfaces;
using ToDoList.Models.DTOs;
using ToDoListModel = ToDoList.Models.Domain.ToDoList;
using AutoMapper;

namespace ToDoList.Controllers;

[ApiController]
[Route("[controller]")]
public class ToDoListController : ControllerBase
{
    private readonly ILogger<ToDoListController> _logger;
    private readonly IToDoListService _toDoListService;
    private readonly IMapper _mapper;

    public ToDoListController(ILogger<ToDoListController> logger, IToDoListService toDoListService, IMapper mapper)
    {
        _logger = logger;
        _toDoListService = toDoListService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] string? filter = null)
    {
        var domainLists = await _toDoListService.GetListsAsync(filter);

        var dtoLists = _mapper.Map<IEnumerable<ToDoListDto>>(domainLists);

        return Ok(dtoLists);
    }

    [HttpPost]
    public async Task<IActionResult> CreateList([FromBody] CreateOrUpdateToDoListDto newListDto)
    {
        var domain = _mapper.Map<ToDoListModel>(newListDto);

        var created = await _toDoListService.CreateListAsync(domain);

        var createdDto = _mapper.Map<ToDoListDto>(created); 

        return CreatedAtAction(nameof(GetList), new { id = createdDto.id }, createdDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateList(int id, [FromBody] CreateOrUpdateToDoListDto updatedDto)
    {
        var domain = _mapper.Map<ToDoListModel>(updatedDto);

        var result = await _toDoListService.UpdateListAsync(id, domain);

        if (result == null) return BadRequest("Invalid list data.");

        var resultDto = _mapper.Map<ToDoListDto>(result);

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
