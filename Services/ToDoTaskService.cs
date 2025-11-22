using Microsoft.EntityFrameworkCore;
using ToDoList.Database;
using ToDoList.Interfaces;
using ToDoList.Models.Domain;
using ToDoList.Pagination;

namespace ToDoList.Services;

public class ToDoTaskService : BaseService<ToDoTask>, IToDoTaskService
{
    public ToDoTaskService(ToDoListContext context) : base(context)
    {
    }

    public async Task<IEnumerable<ToDoTask>> GetTasksAsync(PaginationParameters paginationParameters)
    {
        return await GetAsync(paginationParameters);
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