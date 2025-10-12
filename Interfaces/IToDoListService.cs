using ToDoListModel = ToDoList.Models.Domain.ToDoList;

namespace ToDoList.Interfaces;

public interface IToDoListService
{
    Task<IEnumerable<ToDoListModel>> GetListsAsync(string? filter = null);
    Task<ToDoListModel> CreateListAsync(ToDoListModel newList);
    Task<ToDoListModel?> UpdateListAsync(int id, ToDoListModel updatedList);
    Task<bool> DeleteListAsync(int id);
}