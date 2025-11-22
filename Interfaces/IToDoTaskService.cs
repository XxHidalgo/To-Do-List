using ToDoList.Models.Domain;
using ToDoList.Pagination;

namespace ToDoList.Interfaces;

public interface IToDoTaskService
{
    Task<IEnumerable<ToDoTask>> GetTasksAsync(PaginationParameters paginationParameters);
    Task<ToDoTask?> GetTaskByIdAsync(int id);
    Task<ToDoTask> CreateTaskAsync(ToDoTask newTask);
    Task<ToDoTask?> UpdateTaskAsync(int id, ToDoTask updatedTask);
    Task<bool> DeleteTaskAsync(int id);
}