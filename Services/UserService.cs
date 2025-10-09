using ToDoList.Interfaces;
using ToDoList.Models;
using ToDoList.Database;
using Microsoft.EntityFrameworkCore;
 
namespace ToDoList.Services;
 
public class UserService : IUserService
{
    private readonly ToDoListContext _context;
 
    public UserService(ToDoListContext context)
    {
        _context = context;
    }
 
    public async Task<IEnumerable<User>> GetUsersAsync(string? filter = null)
    {
        if (string.IsNullOrWhiteSpace(filter))
        {
            return await _context.Users.ToListAsync();
        }

        var normalized = filter.ToLower();
        
        return await _context.Users
            .Where(u => u.username != null && u.username.ToLower().Contains(normalized))
            .ToListAsync();
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

        if (existingUser == null)
        {
            return null;
        }

        existingUser.username   = updatedUser.username;
        existingUser.email      = updatedUser.email;
        existingUser.password   = updatedUser.password;

        await _context.SaveChangesAsync();
        return existingUser;
    }
 
    public async Task<bool> DeleteUserAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return false;
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return true;
    }
}