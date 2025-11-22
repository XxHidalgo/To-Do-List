using ToDoList.Interfaces;
using ToDoListModel = ToDoList.Models.Domain.ToDoList;
using ToDoList.Database;
using Microsoft.EntityFrameworkCore;
using ToDoList.Pagination;

namespace ToDoList.Services;

public class ToDoListService : BaseService<ToDoListModel>, IToDoListService
{
    public ToDoListService(ToDoListContext context) : base(context)
    {
    }

    public async Task<IEnumerable<ToDoListModel>> GetListsAsync(PaginationParameters paginationParameters)
    {
        return await GetAsync(paginationParameters);
    }
    
    public async Task<ToDoListModel?> GetListByIdAsync(int id)
    {
        return await Context.ToDoLists
            .Include(l => l.user)
            .Include(l => l.tasks)
            .AsNoTracking()
            .FirstOrDefaultAsync(l => l.id == id);
    }

    public async Task<ToDoListModel> CreateListAsync(ToDoListModel newList)
    {
        return await CreateAsync(newList);
    }

    public async Task<ToDoListModel?> UpdateListAsync(int id, ToDoListModel updatedList)
    {
        return await UpdateAsync(id, updatedList);
    }

    public async Task<bool> DeleteListAsync(int id)
    {
        return await DeleteAsync(id);
    }
}