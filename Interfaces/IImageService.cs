using ToDoList.Models.Domain;
using ToDoList.Pagination;
using ToDoList.Enums;

namespace ToDoList.Interfaces;
public interface IImageService
{
   Task<Image> Upload(Image image);
   Task<IEnumerable<Image>> GetListsAsync(PaginationParameters paginationParameters, Dictionary<string, string>? dynamicWhere);
}