using ToDoList.Models.Domain;
using Microsoft.EntityFrameworkCore;
using ToDoList.Database;
using ToDoList.Interfaces;
using ToDoList.Pagination;
 
namespace ToDoList.Services;
 
public class UserService : BaseService<User>, IUserService
{
    public UserService(ToDoListContext context) : base(context)
    {
    }

    public async Task<IEnumerable<User>> GetUsersAsync(PaginationParameters paginationParameters)
    {
        return await GetAsync(paginationParameters);
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        return await Context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.id == id);
    }
    
    public async Task<User> CreateUserAsync(User newUser)
    {
        return await CreateAsync(newUser);
    }
 
    public async Task<User?> UpdateUserAsync(int id, User updatedUser)
    {
        return await UpdateAsync(id, updatedUser);
    }
 
    public async Task<bool> DeleteUserAsync(int id)
    {
        return await DeleteAsync(id);
    }
}