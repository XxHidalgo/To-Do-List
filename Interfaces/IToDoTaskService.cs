using ToDoList.Models.Domain;

namespace ToDoList.Interfaces;

public interface IToDoTaskService
{
    Task<IEnumerable<ToDoTask>> GetTasksAsync(string? filter = null);
    Task<ToDoTask> CreateTaskAsync(ToDoTask newTask);
    Task<ToDoTask?> UpdateTaskAsync(int id, ToDoTask updatedTask);
    Task<bool> DeleteTaskAsync(int id);
}