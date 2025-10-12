using ToDoList.Models.Domain;

namespace ToDoList.Interfaces;
public interface IUserService
{
    Task<IEnumerable<User>> GetUsersAsync(string? filter = null);
    Task<User> CreateUserAsync(User newUser);
    Task<User?> UpdateUserAsync(int id, User updatedUser);
    Task<bool> DeleteUserAsync(int id);
}