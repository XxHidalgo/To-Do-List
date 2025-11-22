using ToDoList.Models.Domain;
using ToDoList.Pagination;

namespace ToDoList.Interfaces;
public interface IUserService
{
    Task<IEnumerable<User>> GetUsersAsync(PaginationParameters paginationParameters);
    Task<User?> GetUserByIdAsync(int id);
    Task<User> CreateUserAsync(User newUser);
    Task<User?> UpdateUserAsync(int id, User updatedUser);
    Task<bool> DeleteUserAsync(int id);
}