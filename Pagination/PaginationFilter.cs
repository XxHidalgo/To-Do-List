using ToDoList.Models.Domain;
using ToDoList.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ToDoList.Pagination;

public class PaginationFilter<T> : IPaginationFilter<T> where T : class
{
    private IQueryable<T> _data { get; set; }
    private PaginationParameters _paginationParameters;

    public PaginationFilter(IQueryable<T> dbSet, PaginationParameters paginationParameters)
    {
        _paginationParameters = paginationParameters;
        _data = dbSet;
        _applyFiltering();
    }

    internal void _applyFiltering()
    {
        try
        {
            PaginationConfigParameters config = _getConfigParameters();

            if (!string.IsNullOrWhiteSpace(_paginationParameters.filterOn) && !string.IsNullOrWhiteSpace(_paginationParameters.filterQuery))
            {
                if(config.foreignKeyFields != null && config.foreignKeyFields.Contains(_paginationParameters.filterOn))
                {
                    foreach (var field in config.foreignKeyFields)
                    {     
                        if (_paginationParameters.filterOn.Equals(field, StringComparison.OrdinalIgnoreCase))
                        {
                            if (int.TryParse(_paginationParameters.filterQuery, out int foreignKeyValue))
                            {
                                _data = _data.Where(l => EF.Property<int>(l, field) == foreignKeyValue);
                            }
                        }
                    }
                }
                else{
                    foreach (var field in config.filterOnFields!)
                    {     
                        if (_paginationParameters.filterOn.Equals(field, StringComparison.OrdinalIgnoreCase))
                        {
                            _data = _data.Where(l => EF.Property<string>(l, field) != null && EF.Functions.Like(EF.Property<string>(l, field), $"%{_paginationParameters.filterQuery}%"));
                        }
                    }
                }

            }

            if (!string.IsNullOrWhiteSpace(_paginationParameters.sortBy))
            {
                foreach (var field in config.sortByFields!)
                {
                    if (_paginationParameters.sortBy.Equals(field, StringComparison.OrdinalIgnoreCase))
                    {
                        _data = _paginationParameters.sortDescending ? _data.OrderByDescending(l => EF.Property<string>(l, field)) : _data.OrderBy(l => EF.Property<string>(l, field));
                    }
                }
            }

            var skipResult = (_paginationParameters.pageNumber - 1) * _paginationParameters.pageSize;

            _data = _data.Skip(skipResult).Take(_paginationParameters.pageSize);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error applying filtering: {ex.Message}");
        }
    }

    internal PaginationConfigParameters _getConfigParameters()
    {
         var config = new PaginationConfigParameters();

        switch (typeof(T).Name)
        {
            case "ToDoList":

                    config = new PaginationConfigParameters
                    {
                        filterOnFields = new List<string> { "title", "description" },
                        sortByFields = new List<string> { "title", "id" }
                    };

                    break;
            case "ToDoTask":

                    config = new PaginationConfigParameters
                    {
                        foreignKeyFields = new List<string> { "toDoList_id" },
                        filterOnFields = new List<string> { "title", "description" },
                        sortByFields = new List<string> { "title", "id", "dueDate" }
                    };

                    break;  
            case "User":

                    config = new PaginationConfigParameters
                    {
                        filterOnFields = new List<string> { "username"},
                        sortByFields = new List<string> { "username", "id" }
                    };

                    break;
            default:
                throw new NotImplementedException($"Pagination configuration for type {typeof(T).Name} is not implemented.");
        }

        return config;
    }

    public async Task<IEnumerable<T>> getDataWithPaginationApplied()
    {
        return await _data.ToListAsync();
    }
        
}