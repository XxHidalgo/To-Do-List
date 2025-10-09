using ToDoList.Interfaces;
using ToDoList.Models;
using ToDoList.Database;
using Microsoft.EntityFrameworkCore;

namespace ToDoList.Services;

public class ToDoTaskService : IToDoTaskService
{
    private readonly ToDoListContext _context;

    public ToDoTaskService(ToDoListContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ToDoTask>> GetTasksAsync(string? filter = null)
    {
        if (string.IsNullOrEmpty(filter))
        {
            return await _context.ToDoTasks.ToListAsync();
        }

        var normalized = filter.ToLower();

        return await _context.ToDoTasks
                .Where(l =>
                    l.title != null
                    && l.title.ToLower().Contains(normalized)
                )
                .ToListAsync();
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

        if (existingTask == null)
        {
            return null;
        }

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
        if (list == null)
        {
            return false;
        }

        _context.ToDoTasks.Remove(list);
        await _context.SaveChangesAsync();
        return true;
    }
}