using ToDoListModel = ToDoList.Models.Domain.ToDoList;

namespace ToDoList.Interfaces;

public interface IToDoListService
{
    Task<IEnumerable<ToDoListModel>> GetListsAsync(
        string? filterOn = null,
        string? filterQuery = null,
        string? sortBy = null,
        bool sortDescending = false,
        int pageNumber = 1,
        int pageSize = 10
    );
    Task<ToDoListModel?> GetListByIdAsync(int id);
    Task<ToDoListModel> CreateListAsync(ToDoListModel newList);
    Task<ToDoListModel?> UpdateListAsync(int id, ToDoListModel updatedList);
    Task<bool> DeleteListAsync(int id);
}