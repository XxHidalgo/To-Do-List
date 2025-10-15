using ToDoList.Interfaces;
using ToDoList.Database;
using Microsoft.EntityFrameworkCore;
using ToDoList.Models.Domain;
 
namespace ToDoList.Services;
 
public class UserService : IUserService
{
    private readonly ToDoListContext _context;
 
    public UserService(ToDoListContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<User>> GetUsersAsync(
        string? filterOn = null,
        string? filterQuery = null,
        string? sortBy = null,
        bool sortDescending = false
    )
    {
        var query = _context.Users.AsQueryable();

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

        return await query.ToListAsync();
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        return await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.id == id);
    }
    
    public async Task<User> CreateUserAsync(User newUser)
    {
        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();
        return newUser;
    }
 
    public async Task<User?> UpdateUserAsync(int id, User updatedUser)
    {
        var existingUser = await _context.Users.FindAsync(id);

        if (existingUser == null) return null;

        existingUser.username   = updatedUser.username;
        existingUser.email      = updatedUser.email;
        existingUser.password   = updatedUser.password;

        await _context.SaveChangesAsync();
        return existingUser;
    }
 
    public async Task<bool> DeleteUserAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null) return false;

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return true;
    }
}