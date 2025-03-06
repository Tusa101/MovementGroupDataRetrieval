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
    public virtual async Task Add(T entity)
    {
        var existingEntity = await _dbSet.FirstOrDefaultAsync(e => e.Id == entity.Id);
        if (existingEntity != null)
        {
            throw new DuplicateValueException(typeof(T).FullName!, existingEntity.Id);
        }
        _dbSet.Add(entity);
    }

    public async virtual Task Delete(Guid id)
    {
        var entity = await _dbSet.FindAsync(id)
            ?? throw new NotFoundException(typeof(T).FullName!, id);
        await Delete(entity);
    }

    public virtual Task Delete(T entity)
    {
        if (_dbContext.Entry(entity).State == EntityState.Detached)
        {
            _dbSet.Attach(entity);
        }
        _dbSet.Remove(entity);
        return Task.CompletedTask;
    }

    public async virtual Task<IReadOnlyCollection<T>> Get(
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
    public async virtual Task<IReadOnlyCollection<T>> GetAll()
    {
        var result = await _dbSet.ToListAsync();
        return result.AsReadOnly();
    }

    /// <inheritdoc/>

    public async virtual Task<T?> GetById(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    public virtual async Task Update(T entity)
    {
        var existingEntity = await _dbSet.AsNoTracking().FirstOrDefaultAsync(p => p.Id == entity.Id)
            ?? throw new NotFoundException(typeof(T).FullName!, entity.Id);
        _dbSet.Attach(entity);
        _dbContext.Entry(entity).State = EntityState.Modified;
    }

    public async Task<bool> ExistsAsync(Expression<Func<T, bool>> filter)
    {
        var result = await _dbSet.AnyAsync(filter);
        return result;
    }
    #endregion
}
