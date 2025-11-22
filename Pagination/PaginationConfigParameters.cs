namespace ToDoList.Pagination;

public class PaginationConfigParameters
{
    public List<string>? filterOnFields { get; set; }
    public List<string>? sortByFields { get; set; }
    public List<string>? foreignKeyFields = null;
}