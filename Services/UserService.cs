using ToDoList.Models.Domain;
using Microsoft.EntityFrameworkCore;
using ToDoList.Database;
using ToDoList.Interfaces;
 
namespace ToDoList.Services;
 
public class UserService : BaseService<User>, IUserService
{
    public UserService(ToDoListContext context) : base(context)
    {
    }

    public async Task<IEnumerable<User>> GetUsersAsync(
        string? filterOn = null,
        string? filterQuery = null,
        string? sortBy = null,
        bool sortDescending = false,
        int pageNumber = 1,
        int pageSize = 100
    )
    {
    var query = Context.Users.AsQueryable();

        if (!string.IsNullOrWhiteSpace(filterOn) && !string.IsNullOrWhiteSpace(filterQuery))
        {
            if (filterOn.Equals("username", StringComparison.OrdinalIgnoreCase))
            {
                query = query.Where(u => u.username != null && EF.Functions.Like(u.username, $"%{filterQuery}%"));
            }
        }

        if (!string.IsNullOrWhiteSpace(sortBy))
        {
            if (sortBy.Equals("username", StringComparison.OrdinalIgnoreCase))
            {
                query = sortDescending ? query.OrderByDescending(u => u.username) : query.OrderBy(u => u.username);
            }
            else if (sortBy.Equals("id", StringComparison.OrdinalIgnoreCase))
            {
                query = sortDescending ? query.OrderByDescending(u => u.id) : query.OrderBy(u => u.id);
            }
        }
        
        var skipResult = (pageNumber - 1) * pageSize;

        return await query.Skip(skipResult).Take(pageSize).ToListAsync();
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