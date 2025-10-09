using ToDoList.Interfaces;
using ToDoList.Models;
using ToDoListModel = ToDoList.Models.ToDoList;
using ToDoList.Database;
using Microsoft.EntityFrameworkCore;

namespace ToDoList.Services;

public class ToDoListService : IToDoListService
{
    private readonly ToDoListContext _context;

    public ToDoListService(ToDoListContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ToDoListModel>> GetListsAsync(string? filter = null)
    {
        if (string.IsNullOrEmpty(filter))
        {
            return await _context.ToDoLists.ToListAsync();
        }
        return await _context.ToDoLists
            .Where(l =>
                l.title != null && l.title.Contains(filter, StringComparison.OrdinalIgnoreCase))
            .ToListAsync();
    }

    public async Task<ToDoListModel> CreateListAsync(ToDoListModel newList)
    {
        _context.ToDoLists.Add(newList);
        await _context.SaveChangesAsync();
        return newList;
    }

    public async Task<ToDoListModel?> UpdateListAsync(int id, ToDoListModel updatedList)
    {
        var existingList = await _context.ToDoLists.FindAsync(id);

        if (existingList == null)
        {
            return null;
        }

        existingList.title          = updatedList.title;
        existingList.description    = updatedList.description;
        existingList.isCompleted    = updatedList.isCompleted;
        existingList.user_id        = updatedList.user_id;

        await _context.SaveChangesAsync();
        return existingList;
    }

    public async Task<bool> DeleteListAsync(int id)
    {
        var list = await _context.ToDoLists.FindAsync(id);
        if (list == null)
        {
            return false;
        }

        _context.ToDoLists.Remove(list);
        await _context.SaveChangesAsync();
        return true;
    }
}