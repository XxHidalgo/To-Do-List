using Microsoft.AspNetCore.Mvc;
using ToDoList.Services;
using ToDoList.Interfaces;
using ToDoList.Models.DTOs;
using ToDoListModel = ToDoList.Models.Domain.ToDoList;
using AutoMapper;
using ToDoList.CustomActionFilters;
using Microsoft.AspNetCore.Authorization;
using ToDoList.Pagination;

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
    [Authorize(Roles = "Reader")]
    public async Task<IActionResult> GetList(
        [FromQuery] string? filterOn = null,
        [FromQuery] string? filterQuery = null,
        [FromQuery] string? sortBy = null,
        [FromQuery] bool sortDescending = false,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10
    )
    {
        var domainLists = await _toDoListService.GetListsAsync(
            new PaginationParameters
            {
                filterOn = filterOn,
                filterQuery = filterQuery,
                sortBy = sortBy,
                sortDescending = sortDescending,
                pageNumber = pageNumber,
                pageSize = pageSize
            }
        );

        var dtoLists = _mapper.Map<IEnumerable<ToDoListDto>>(domainLists);

        return Ok(dtoLists);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Reader")]
    public async Task<IActionResult> GetList(int id)
    {
        var domain = await _toDoListService.GetListByIdAsync(id);

        if (domain == null) return NotFound();

        var dto = _mapper.Map<ToDoListDto>(domain);

        return Ok(dto);
    }

    [HttpPost]
    [ValidateModel]
    [Authorize(Roles = "Writer")]
    public async Task<IActionResult> CreateList([FromBody] CreateOrUpdateToDoListDto newListDto)
    {
        var domain = _mapper.Map<ToDoListModel>(newListDto);

        var created = await _toDoListService.CreateListAsync(domain);

        var createdDto = _mapper.Map<ToDoListDto>(created); 

        return CreatedAtAction(nameof(GetList), new { id = createdDto.id }, createdDto);
    }

    [HttpPut("{id}")]
    [ValidateModel]
    [Authorize(Roles = "Writer")]
    public async Task<IActionResult> UpdateList(int id, [FromBody] CreateOrUpdateToDoListDto updatedDto)
    {
        var domain = _mapper.Map<ToDoListModel>(updatedDto);

        var result = await _toDoListService.UpdateListAsync(id, domain);

        if (result == null) return NotFound("Invalid list data.");

        var resultDto = _mapper.Map<ToDoListDto>(result);

        return Ok(resultDto);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Writer")]
    public async Task<IActionResult> DeleteList(int id)
    {
        var success = await _toDoListService.DeleteListAsync(id);

        if (!success) return NotFound("Invalid list ID.");

        return NoContent();
    }
}
