using System.Linq.Expressions;
using Domain.Abstractions;
using Domain.Exceptions;
using Domain.Primitives;
using Infrastructure.Configuration.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Base;
public class RepositoryBase<T> : IRepositoryBase<T>
    where T : BaseEntity
{
    private readonly ApplicationDbContext _dbContext;
    protected readonly DbSet<T> _dbSet;
    public RepositoryBase(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<T>();
    }

    #region RepositoryBase Methods Implementations
    
    ///<inheritdoc/>
    public virtual async Task AddAsync(T entity)
    {
        var existingEntity = await _dbSet.FirstOrDefaultAsync(e => e.Id == entity.Id);
        if (existingEntity != null)
        {
            throw new DuplicateValueException(typeof(T).FullName!, existingEntity.Id);
        }
        _dbSet.Add(entity);
    }

    ///<inheritdoc/>
    public async virtual Task DeleteByIdAsync(Guid id)
    {
        var entity = await _dbSet.FindAsync(id)
            ?? throw new NotFoundException(typeof(T).FullName!, id);
        await DeleteAsync(entity);
    }

    ///<inheritdoc/>
    public virtual Task DeleteAsync(T entity)
    {
        if (_dbContext.Entry(entity).State == EntityState.Detached)
        {
            _dbSet.Attach(entity);
        }
        _dbSet.Remove(entity);
        return Task.CompletedTask;
    }

    ///<inheritdoc/>
    public async virtual Task<IReadOnlyCollection<T>> GetAsync(
        Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>,
        IOrderedQueryable<T>>? orderBy = null,
        bool trackChanges = false)
    {
        var query = _dbSet.AsQueryable();

        if (!trackChanges)
        {
            query.AsNoTracking();
        }

        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (orderBy != null)
        {
            query = orderBy(query);
        }

        var result = await query.ToListAsync();

        return result.AsReadOnly();
    }


    /// <inheritdoc/>
    public async virtual Task<ICollection<T>> GetAllAsync()
        => await _dbSet.ToListAsync();

    /// <inheritdoc/>
    public async virtual Task<T?> GetByIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    ///<inheritdoc/>
    public virtual async Task UpdateAsync(T entity)
    {
        _ = await _dbSet.AsNoTracking().FirstOrDefaultAsync(p => p.Id == entity.Id)
            ?? throw new NotFoundException(typeof(T).FullName!, entity.Id);
        _dbSet.Attach(entity);
        _dbContext.Entry(entity).State = EntityState.Modified;
    }

    ///<inheritdoc/>
    public async Task<bool> ExistsAsync(Expression<Func<T, bool>> filter)
    {
        var result = await _dbSet.AnyAsync(filter);
        return result;
    }

    ///<inheritdoc/>
    public async Task<int> CountAsync() 
        => await _dbSet.CountAsync();
    #endregion
}
