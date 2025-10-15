using ToDoList.Models.Domain;

namespace ToDoList.Interfaces;

public interface IToDoTaskService
{
    Task<IEnumerable<ToDoTask>> GetTasksAsync(
        string? filterOn = null,
        string? filterQuery = null,
        string? sortBy = null,
        bool sortDescending = false
    );
    Task<ToDoTask?> GetTaskByIdAsync(int id);
    Task<ToDoTask> CreateTaskAsync(ToDoTask newTask);
    Task<ToDoTask?> UpdateTaskAsync(int id, ToDoTask updatedTask);
    Task<bool> DeleteTaskAsync(int id);
}