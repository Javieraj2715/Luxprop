using Luxprop.Data.Models;
using Microsoft.EntityFrameworkCore;

public interface IRepositoryBase<T>
{
    /// <summary>
    /// Method that updates the entity if exists otherwise inserts a new record
    /// </summary>
    /// <param name="entity">The entity to be deleted.</param>
    /// <param name="isUpdating">The indicator that tells if I need to update or create</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task<bool> UpsertAsync(T entity, bool isUpdating);

    /// <summary>
    /// Creates a new entity asynchronously.
    /// </summary>
    /// <param name="entity">The entity to be created.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating success.</returns>
    Task<bool> CreateAsync(T entity);

    /// <summary>
    /// Deletes an existing entity asynchronously.
    /// </summary>
    /// <param name="entity">The entity to be deleted.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating success.</returns>
    Task<bool> DeleteAsync(T entity);

    /// <summary>
    /// Reads all entities of type T asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of entities.</returns>
    Task<IEnumerable<T>> ReadAsync();

    /// <summary>
    /// Finds an entity from the list of objects
    /// </summary>
    /// <param name="id">integer</param>
    /// <returns>Entity by id</returns>
    Task<T> FindAsync(int id);

    /// <summary>
    /// Updates an existing entity asynchronously.
    /// </summary>
    /// <param name="entity">The entity to be updated.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating success.</returns>
    Task<bool> UpdateAsync(T entity);

    /// <summary>
    /// Updates multiple entities asynchronously.
    /// </summary>
    /// <param name="entities">The collection of entities to be updated.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating success.</returns>
    Task<bool> UpdateManyAsync(IEnumerable<T> entities);

    /// <summary>
    /// Checks if an entity exists asynchronously.
    /// </summary>
    /// <param name="entity">The entity to check for existence.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating if the entity exists.</returns>
    Task<bool> ExistsAsync(T entity);
}

public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
{
    protected readonly LuxpropContext _context;
    protected readonly DbSet<T> DbSet;
    protected LuxpropContext DbContext => _context;

    public RepositoryBase(LuxpropContext context)
    {
        _context = context;
        DbSet = _context.Set<T>();
    }

    public async Task<bool> UpsertAsync(T entity, bool isUpdating)
    {
        return isUpdating
            ? await UpdateAsync(entity)
            : await CreateAsync(entity);
    }

    public async Task<bool> CreateAsync(T entity)
    {
        await DbSet.AddAsync(entity);
        return await SaveAsync();
    }

    public async Task<bool> UpdateAsync(T entity)
    {
        DbSet.Update(entity);
        return await SaveAsync();
    }

    public async Task<bool> UpdateManyAsync(IEnumerable<T> entities)
    {
        DbSet.UpdateRange(entities);
        return await SaveAsync();
    }

    public async Task<bool> DeleteAsync(T entity)
    {
        DbSet.Remove(entity);
        return await SaveAsync();
    }

    public async Task<IEnumerable<T>> ReadAsync()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<T> FindAsync(int id)
    {
        return await DbSet.FindAsync(id);
    }

    public async Task<bool> ExistsAsync(T entity)
    {
        var items = await ReadAsync();
        return items.Any(x => x.Equals(entity));
    }

    protected async Task<bool> SaveAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}
