using ToDoListModel = ToDoList.Models.Domain.ToDoList;
using ToDoList.Pagination;

namespace ToDoList.Interfaces;

public interface IToDoListService
{
    Task<IEnumerable<ToDoListModel>> GetListsAsync(PaginationParameters paginationParameters);
    Task<ToDoListModel?> GetListByIdAsync(int id);
    Task<ToDoListModel> CreateListAsync(ToDoListModel newList);
    Task<ToDoListModel?> UpdateListAsync(int id, ToDoListModel updatedList);
    Task<bool> DeleteListAsync(int id);
}