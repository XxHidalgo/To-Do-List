using ToDoList.Pagination;

namespace ToDoList.Interfaces;

public interface IPaginationFilter<T>
{
    Task<IEnumerable<T>> getDataWithPaginationApplied();
}