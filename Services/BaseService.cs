using Microsoft.EntityFrameworkCore;
using ToDoList.Database;
using ToDoList.Interfaces;

namespace ToDoList.Services;

public class BaseService<T> : IBaseService<T> where T : class
{
    protected BaseService(ToDoListContext context)
    {
        Context = context;
        DbSet = Context.Set<T>();
    }

    protected ToDoListContext Context { get; }

    protected DbSet<T> DbSet { get; }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await DbSet.AsNoTracking().ToListAsync();
    }

    public virtual async Task<T?> GetByIdAsync(int id)
    {
        return await DbSet.FindAsync(id);
    }

    public virtual async Task<T> CreateAsync(T entity)
    {
        DbSet.Add(entity);
        await Context.SaveChangesAsync();
        return entity;
    }

    public virtual async Task<T?> UpdateAsync(int id, T entity)
    {
        var existingEntity = await DbSet.FindAsync(id);
        if (existingEntity == null)
        {
            return null;
        }

        Context.Entry(existingEntity).CurrentValues.SetValues(entity);
        await Context.SaveChangesAsync();
        return existingEntity;
    }

    public virtual async Task<bool> DeleteAsync(int id)
    {
        var entity = await DbSet.FindAsync(id);
        if (entity == null)
        {
            return false;
        }

        DbSet.Remove(entity);
        await Context.SaveChangesAsync();
        return true;
    }
}