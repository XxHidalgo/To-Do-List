using ToDoList.Interfaces;
using ToDoListModel = ToDoList.Models.Domain.ToDoList;
using ToDoList.Database;
using Microsoft.EntityFrameworkCore;

namespace ToDoList.Services;

public class ToDoListService : IToDoListService
{
    private readonly ToDoListContext _context;

    public ToDoListService(ToDoListContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ToDoListModel>> GetListsAsync(
        string? filterOn = null,
        string? filterQuery = null,
        string? sortBy = null,
        bool sortDescending = false
    )
    {
        var query = _context.ToDoLists
            .Include(l => l.user)
            .Include(l => l.tasks)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filterOn) && !string.IsNullOrWhiteSpace(filterQuery))
        {
            if (filterOn.Equals("title", StringComparison.OrdinalIgnoreCase))
            {
                query = query.Where(l => l.title != null && EF.Functions.Like(l.title, $"%{filterQuery}%"));
            }
        }

        if (!string.IsNullOrWhiteSpace(sortBy))
        {
            if (sortBy.Equals("title", StringComparison.OrdinalIgnoreCase))
            {
                query = sortDescending ? query.OrderByDescending(l => l.title) : query.OrderBy(l => l.title);
            }
            else if (sortBy.Equals("id", StringComparison.OrdinalIgnoreCase))
            {
                query = sortDescending ? query.OrderByDescending(l => l.id) : query.OrderBy(l => l.id);
            }
        }

        return await query.OrderBy(l => l.title).ToListAsync();
    }

    public async Task<ToDoListModel?> GetListByIdAsync(int id)
    {
        return await _context.ToDoLists
            .Include(l => l.user)
            .Include(l => l.tasks)
            .AsNoTracking()
            .FirstOrDefaultAsync(l => l.id == id);
    }

    public async Task<ToDoListModel> CreateListAsync(ToDoListModel newList)
    {
        _context.ToDoLists.Add(newList);
        await _context.SaveChangesAsync();
        return newList;
    }

    public async Task<ToDoListModel?> UpdateListAsync(int id, ToDoListModel updatedList)
    {
        var existingList = await _context.ToDoLists.FindAsync(id);

        if (existingList == null) return null;

        existingList.title          = updatedList.title;
        existingList.description    = updatedList.description;
        existingList.isCompleted    = updatedList.isCompleted;
        existingList.user_id        = updatedList.user_id;

        await _context.SaveChangesAsync();
        return existingList;
    }

    public async Task<bool> DeleteListAsync(int id)
    {
        var list = await _context.ToDoLists.FindAsync(id);

        if (list == null) return false;

        _context.ToDoLists.Remove(list);
        await _context.SaveChangesAsync();
        return true;
    }
}