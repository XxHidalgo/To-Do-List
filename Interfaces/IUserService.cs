using ToDoList.Models.Domain;

namespace ToDoList.Interfaces;
public interface IUserService
{
    Task<IEnumerable<User>> GetUsersAsync(
        string? filterOn = null,
        string? filterQuery = null,
        string? sortBy = null,
        bool sortDescending = false,
        int pageNumber = 1,
        int pageSize = 10
    );
    Task<User?> GetUserByIdAsync(int id);
    Task<User> CreateUserAsync(User newUser);
    Task<User?> UpdateUserAsync(int id, User updatedUser);
    Task<bool> DeleteUserAsync(int id);
}