using ToDoList.Models.Domain;
using ToDoList.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;

namespace ToDoList.Pagination;

public class PaginationFilter<T> : IPaginationFilter<T> where T : class
{
    private IQueryable<T> _data { get; set; }
    private PaginationParameters _paginationParameters;
    private Dictionary<string, string>? _dynamicWhere;

    public PaginationFilter(IQueryable<T> dbSet, PaginationParameters paginationParameters, Dictionary<string, string>? dynamicWhere)
    {
        _paginationParameters = paginationParameters;
        _data = dbSet;
        _dynamicWhere = dynamicWhere?? null;
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

            if (_dynamicWhere is { Count: > 0 })
            {
                _data = ApplyDynamicWhere(_data, _dynamicWhere);
            }

            if (!string.IsNullOrWhiteSpace(_paginationParameters.sortBy))
            {
                foreach (var field in config.sortByFields!)
                {
                    if (_paginationParameters.sortBy.Equals(field, StringComparison.OrdinalIgnoreCase))
                    {
                        _data = ApplyDynamicOrderBy(_data, field, _paginationParameters.sortDescending);
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

    private IQueryable<T> ApplyDynamicWhere(IQueryable<T> data, Dictionary<string, string> dynamicWhere)
    {
        if (dynamicWhere == null || dynamicWhere.Count == 0)
            return data;

        foreach (var item in dynamicWhere)
        {
            var property = GetPropertyInfo(item.Key);

            if (property == null || !TryConvertValue(item.Value, property.PropertyType, out var typedValue))
                continue;

            var parameter = Expression.Parameter(typeof(T), "x");
            var propertyAccess = Expression.Property(parameter, property);
            var constant = Expression.Constant(typedValue, property.PropertyType);
            var body = Expression.Equal(propertyAccess, constant);
            var predicate = Expression.Lambda<Func<T, bool>>(body, parameter);

            data = data.Where(predicate);
        }

        return data;
    }

    private static IQueryable<T> ApplyDynamicOrderBy(IQueryable<T> data, string field, bool sortDescending)
    {
        var property = GetPropertyInfo(field);

        if (property == null)
            return data;

        var parameter = Expression.Parameter(typeof(T), "x");
        var propertyAccess = Expression.Property(parameter, property);
        var keySelector = Expression.Lambda(propertyAccess, parameter);
        var methodName = sortDescending ? nameof(Queryable.OrderByDescending) : nameof(Queryable.OrderBy);
        var orderedExpression = Expression.Call(
            typeof(Queryable),
            methodName,
            new[] { typeof(T), property.PropertyType },
            data.Expression,
            Expression.Quote(keySelector));

        return data.Provider.CreateQuery<T>(orderedExpression);
    }

    private static PropertyInfo? GetPropertyInfo(string propertyName)
    {
        return typeof(T).GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
    }

    private static bool TryConvertValue(string value, Type targetType, out object? convertedValue)
    {
        var underlyingType = Nullable.GetUnderlyingType(targetType) ?? targetType;

        try
        {
            if (underlyingType == typeof(string))
            {
                convertedValue = value;
                return true;
            }

            if (underlyingType.IsEnum)
            {
                convertedValue = Enum.Parse(underlyingType, value, ignoreCase: true);
                return true;
            }

            convertedValue = Convert.ChangeType(value, underlyingType);
            return true;
        }
        catch
        {
            convertedValue = null;
            return false;
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
            case "Image":

                    config = new PaginationConfigParameters
                    {
                        sortByFields = new List<string> {"id"}
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