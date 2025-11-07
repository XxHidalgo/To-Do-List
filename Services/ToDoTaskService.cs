using Microsoft.EntityFrameworkCore;
using ToDoList.Database;
using ToDoList.Interfaces;
using ToDoList.Models.Domain;

namespace ToDoList.Services;

public class ToDoTaskService : BaseService<ToDoTask>, IToDoTaskService
{
    public ToDoTaskService(ToDoListContext context) : base(context)
    {
    }

    public async Task<IEnumerable<ToDoTask>> GetTasksAsync(
        string? filterOn = null,
        string? filterQuery = null,
        string? sortBy = null,
        bool sortDescending = false,
        int pageNumber = 1,
        int pageSize = 100
    )
    {
    var query = Context.ToDoTasks
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
            else if (sortBy.Equals("dueDate", StringComparison.OrdinalIgnoreCase))
            {
                query = sortDescending ? query.OrderByDescending(l => l.dueDate) : query.OrderBy(l => l.dueDate);
            }
        }

        var skipResult = (pageNumber - 1) * pageSize;

        return await query.Skip(skipResult).Take(pageSize).ToListAsync();
    }
    
    public async Task<ToDoTask?> GetTaskByIdAsync(int id)
    {
        return await Context.ToDoTasks
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.id == id);
    }

    public async Task<ToDoTask> CreateTaskAsync(ToDoTask newTask)
    {
        return await CreateAsync(newTask);
    }

    public async Task<ToDoTask?> UpdateTaskAsync(int id, ToDoTask updatedTask)
    {
        return await UpdateAsync(id, updatedTask);
    }

    public async Task<bool> DeleteTaskAsync(int id)
    {
        return await DeleteAsync(id);
    }
}