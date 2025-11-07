using ToDoList.Interfaces;
using ToDoListModel = ToDoList.Models.Domain.ToDoList;
using ToDoList.Database;
using Microsoft.EntityFrameworkCore;

namespace ToDoList.Services;

public class ToDoListService : BaseService<ToDoListModel>, IToDoListService
{
    public ToDoListService(ToDoListContext context) : base(context)
    {
    }

    public async Task<IEnumerable<ToDoListModel>> GetListsAsync(
        string? filterOn = null,
        string? filterQuery = null,
        string? sortBy = null,
        bool sortDescending = false,
        int pageNumber = 1,
        int pageSize = 100
    )
    {
        var query = Context.ToDoLists
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

        var skipResult = (pageNumber - 1) * pageSize;

        return await query.Skip(skipResult).Take(pageSize).ToListAsync();
    }

    public async Task<ToDoListModel?> GetListByIdAsync(int id)
    {
        return await Context.ToDoLists
            .Include(l => l.user)
            .Include(l => l.tasks)
            .AsNoTracking()
            .FirstOrDefaultAsync(l => l.id == id);
    }

    public async Task<ToDoListModel> CreateListAsync(ToDoListModel newList)
    {
        return await CreateAsync(newList);
    }

    public async Task<ToDoListModel?> UpdateListAsync(int id, ToDoListModel updatedList)
    {
        return await UpdateAsync(id, updatedList);
    }

    public async Task<bool> DeleteListAsync(int id)
    {
        return await DeleteAsync(id);
    }
}