namespace ToDoList.Pagination
{
    public class PaginationParameters
    {
        public string? filterOn = null;
        public string? filterQuery = null;
        public string? sortBy = null;
        public bool sortDescending = false;
        public int pageNumber = 1;
        public int pageSize = 10;
    }
}
