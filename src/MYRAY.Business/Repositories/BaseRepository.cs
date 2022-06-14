using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MYRAY.Business.Repositories.Interface;

namespace MYRAY.Business.Repositories;

/// <summary>
/// Generic repository.
/// </summary>
/// <typeparam name="TEntity">entity class.</typeparam>
public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
{
    
    private readonly DbSet<TEntity> _dbSet;
    
    public DbContext CurrentContext { get;}

    /// <summary>
    /// Initializes a new instance of <see cref="BaseRepository{TEntity}"/>
    /// </summary>
    /// <param name="currentContext">context</param>
    public BaseRepository(DbContext currentContext)
    {
        CurrentContext = currentContext;
        _dbSet = CurrentContext.Set<TEntity>();
    }

    /// <summary>
    /// Load include properties of entity using eager loading.
    /// </summary>
    /// <param name="query">object context</param>
    /// <param name="includeProperties">Array for include properties.</param>
    /// <returns>DbSet of entity</returns>
    private object IncludeProperties(object query, Expression<Func<TEntity, object>>[]? includeProperties)
    {
        if (includeProperties != null)
        {
            if(query.GetType() == typeof(DbSet<TEntity>))
            {
                query = includeProperties.Aggregate(query, (current, property) => ((DbSet<TEntity>)current).Include(property));
            }
            else
            {
                query = includeProperties.Aggregate(query, (current, property) => ((IQueryable<TEntity>)current).Include(property));
            }
        }

        return query;
    }
    
    /// <inheritdoc cref="IBaseRepository{Entity}.Table"/>
    public IQueryable<TEntity> Table => _dbSet;

    /// <inheritdoc cref="IBaseRepository{Entity}.EntityEntry"/>
    public EntityEntry<TEntity> EntityEntry(TEntity entity) => CurrentContext.Entry(entity);

    /// <inheritdoc cref="IBaseRepository{Entity}.GetAll"/>
    public IQueryable<TEntity> GetAll() => _dbSet.AsQueryable();
    
    /// <inheritdoc cref="IBaseRepository{Entity}.GetById"/>
    public TEntity? GetById(object? id, Expression<Func<TEntity, object>>[]? includeProperties = null)
    {
        DbSet<TEntity> query = _dbSet;
        query = (DbSet<TEntity>)IncludeProperties(query, includeProperties);
        return query.Find(id);
    }

    /// <inheritdoc cref="IBaseRepository{Entity}.GetByIdAsync"/>
    public async Task<TEntity?> GetByIdAsync(object id, Expression<Func<TEntity, object>>[]? includeProperties = null)
    {
        DbSet<TEntity> query = _dbSet;
        query = (DbSet<TEntity>)IncludeProperties(query, includeProperties);
        return await query.FindAsync(id);
    }

    /// <inheritdoc cref="IBaseRepository{Entity}.Get"/>
    public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>>? filter = null, Expression<Func<TEntity, object>>[]? includeProperties = null)
    {
        IQueryable<TEntity> query = _dbSet;
        if (filter != null)
        {
            query = query.Where(filter);
        }
        query = (IQueryable<TEntity>)IncludeProperties(query, includeProperties);

        return query;
    }

    /// <inheritdoc cref="IBaseRepository{Entity}.GetFirstOrDefault"/>
    public TEntity? GetFirstOrDefault(Expression<Func<TEntity, bool>>? filter = null, Expression<Func<TEntity, object>>[]? includeProperties = null)
    {
        IQueryable<TEntity> query = _dbSet;
        if (filter != null)
        {
            query = query.Where(filter);
        }
        query = (IQueryable<TEntity>)IncludeProperties(query, includeProperties);

        return query.FirstOrDefault();
    }

    /// <inheritdoc cref="IBaseRepository{Entity}.GetFirstOrDefaultAsync"/>
    public async Task<TEntity?> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>>? filter = null, Expression<Func<TEntity, object>>[]? includeProperties = null)
    {
        IQueryable<TEntity> query = _dbSet;
        if (filter != null)
        {
            query = query.Where(filter);
        }
        query = (IQueryable<TEntity>)IncludeProperties(query, includeProperties);

        return await query.FirstOrDefaultAsync();
    }

    /// <inheritdoc cref="IBaseRepository{TEntity}.Insert(TEntity)"/>
    public void Insert(TEntity entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        _dbSet.Add(entity);
    }

    public async void Insert(IEnumerable<TEntity> entities)
    {
        if (entities == null)
        {
            throw new ArgumentNullException(nameof(entities));
        }
        
        await _dbSet.AddRangeAsync(entities);
    }
    
    /// <inheritdoc cref="IBaseRepository{Entity}.InsertAsync(Entity)"/>
    public async Task InsertAsync(TEntity entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        await _dbSet.AddAsync(entity);
    }

    /// <inheritdoc cref="IBaseRepository{Entity}.InsertAsync(IEnumerable{Entity})"/>
    public void InsertAsync(IEnumerable<TEntity> entities)
    {
        if (entities == null)
        {
            throw new ArgumentNullException(nameof(entities));
        }
        
        _dbSet.AddRange(entities);
    }

    /// <inheritdoc cref="IBaseRepository{Entity}.Update(Entity)"/>
    public void Update(TEntity entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        _dbSet.Attach(entity);
        CurrentContext.Update(entity);
    }

    public void Modify(TEntity entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        int id = (int)entity.GetType().GetProperty("Id")!.GetValue(entity, null)!;
        TEntity? db = GetById(id);
        PropertyInfo[] prop = (PropertyInfo[])db!.GetType().GetProperties().Clone();
       
            for (int i = 0; i < prop.Length;i++)
            {
                
            
            if(!prop[i].GetAccessors()[0].IsVirtual)
            {
                var pro = entity.GetType().GetProperty(prop[i].Name);
                
                var valCur = pro!.GetValue(entity, null);
                var valOri = prop[i].GetValue(db, null);
                if(ReferenceEquals(valCur,valOri)) continue;
                if (valOri == null)
                {
                    db.GetType().GetProperty(prop[i].Name)!.SetValue(db, valCur);
                    continue;
                }

                if (valCur != null)
                {
                    db.GetType().GetProperty(prop[i].Name)!.SetValue(db, valCur);
                }
                
            }
            }
                
        Update(db);
    }

    /// <inheritdoc cref="IBaseRepository{Entity}.Update(IEnumerable{Entity})"/>
    public void Update(IEnumerable<TEntity> entities)
    {
        if (entities == null)
        {
            throw new ArgumentNullException(nameof(entities));
        }

        var enumerable = entities as TEntity[] ?? entities.ToArray();
        _dbSet.AttachRange(enumerable);
        CurrentContext.UpdateRange(enumerable);
    }

    /// <inheritdoc cref="IBaseRepository{Entity}.Delete(Entity)"/>
    public void Delete(TEntity entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        if (this.CurrentContext.Entry(entity).State == EntityState.Detached)
        {
            _dbSet.Attach(entity);
        }

        _dbSet.Remove(entity);
    }

    /// <inheritdoc cref="IBaseRepository{Entity}.Delete(IEnumerable{Entity})"/>
    public void Delete(IEnumerable<TEntity> entities)
    {
        if (entities == null)
        {
            throw new ArgumentNullException(nameof(entities));
        }

        var enumerable = entities as TEntity[] ?? entities.ToArray();
        foreach (var entity in enumerable)
        {
            if (CurrentContext.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }
        }
        _dbSet.RemoveRange(enumerable);
    }
}