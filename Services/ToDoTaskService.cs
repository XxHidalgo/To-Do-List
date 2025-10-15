using ToDoList.Interfaces;
using ToDoList.Database;
using Microsoft.EntityFrameworkCore;
using ToDoList.Models.Domain;
using Microsoft.EntityFrameworkCore.Query;

namespace ToDoList.Services;

public class ToDoTaskService : IToDoTaskService
{
    private readonly ToDoListContext _context;

    public ToDoTaskService(ToDoListContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ToDoTask>> GetTasksAsync(
        string? filterOn = null,
        string? filterQuery = null,
        string? sortBy = null,
        bool sortDescending = false
    )
    {
        var query = _context.ToDoTasks
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

        return await query.ToListAsync();
    }
    
    public async Task<ToDoTask?> GetTaskByIdAsync(int id)
    {
        return await _context.ToDoTasks
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.id == id);
    }

    public async Task<ToDoTask> CreateTaskAsync(ToDoTask newTask)
    {
        _context.ToDoTasks.Add(newTask);
        await _context.SaveChangesAsync();
        return newTask;
    }

    public async Task<ToDoTask?> UpdateTaskAsync(int id, ToDoTask updatedTask)
    {
        var existingTask = await _context.ToDoTasks.FindAsync(id);

        if (existingTask == null) return null;

        existingTask.title          = updatedTask.title;
        existingTask.description    = updatedTask.description;
        existingTask.isCompleted    = updatedTask.isCompleted;
        existingTask.dueDate        = updatedTask.dueDate;
        existingTask.toDoList_id    = updatedTask.toDoList_id;

        await _context.SaveChangesAsync();
        return existingTask;
    }

    public async Task<bool> DeleteTaskAsync(int id)
    {
        var list = await _context.ToDoTasks.FindAsync(id);

        if (list == null) return false;

        _context.ToDoTasks.Remove(list);
        await _context.SaveChangesAsync();
        return true;
    }
}