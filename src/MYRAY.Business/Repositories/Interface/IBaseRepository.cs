using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace MYRAY.Business.Repositories.Interface;


/// <summary>
/// Interface for Generic repository
/// </summary>
/// <typeparam name="TEntity">entity class.</typeparam>
public interface IBaseRepository<TEntity> where TEntity : class
{
    /// <summary>
    /// Gets a table
    /// </summary>
    IQueryable<TEntity> Table { get; }
    
    /// <summary>
    /// Entry to tracking and operations entity.
    /// </summary>
    /// <param name="entity">entity.</param>
    /// <returns>Entity entry.</returns>
    EntityEntry<TEntity> EntityEntry(TEntity entity);
    
    /// <summary>
    /// Get all entities.
    /// </summary>
    /// <returns>Entities.</returns>
    IQueryable<TEntity> GetAll();

    /// <summary>
    /// Get entity bi identifier.
    /// </summary>
    /// <param name="id">Identifier.</param>
    /// <param name="includeProperties">Array for include properties.</param>
    /// <returns>Entity.</returns>
    TEntity? GetById(object id, Expression<Func<TEntity, object>>[]? includeProperties = null);

    /// <summary>
    /// Get async entity bi identifier.
    /// </summary>
    /// <param name="id">Identifier.</param>
    /// <param name="includeProperties">Array for include properties.</param>
    /// <returns>Entity.</returns>
    Task<TEntity?> GetByIdAsync(object id, Expression<Func<TEntity, object>>[]? includeProperties = null);

    /// <summary>
    /// Get entities with condition.
    /// </summary>
    /// <param name="filter">Condition Filters.</param>
    /// <param name="includeProperties">Array for include properties.</param>
    /// <returns>Entities.</returns>
    IQueryable<TEntity> Get(
        Expression<Func<TEntity, bool>>? filter = null,
        Expression<Func<TEntity, object>>[]? includeProperties = null);

    /// <summary>
    /// Get an entity with condition.
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="includeProperties">Array for include properties.</param>
    /// <returns>Entity.</returns>
    TEntity? GetFirstOrDefault(
        Expression<Func<TEntity, bool>>? filter = null,
        Expression<Func<TEntity, object>>[]? includeProperties = null);
    
    /// <summary>
    /// Get async an entity with condition.
    /// </summary>
    /// <param name="filter">Condition Filters.</param>
    /// <param name="includeProperties">Array for include properties.</param>
    /// <returns>Entity.</returns>
    Task<TEntity?> GetFirstOrDefaultAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        Expression<Func<TEntity, object>>[]? includeProperties = null);
    
    /// <summary>
    /// Insert entity.
    /// </summary>
    /// <param name="entity">entity.</param>
    void Insert(TEntity entity);
    
    /// <summary>
    /// Insert list of entity
    /// </summary>
    /// <param name="entities">entities.</param>
    void Insert(IEnumerable<TEntity> entities);
    
    /// <summary>
    /// Insert async entity.
    /// </summary>
    /// <param name="entity">entity.</param>
    Task InsertAsync(TEntity entity);
    
    /// <summary>
    /// Insert async list of entities
    /// </summary>
    /// <param name="entities"></param>
    /// <returns></returns>
    void InsertAsync(IEnumerable<TEntity> entities);

    /// <summary>
    /// Update entity
    /// </summary>
    /// <param name="entity">entity.</param>
    void Update(TEntity entity);
    
    /// <summary>
    /// Delete entity.
    /// </summary>
    /// <param name="entities">entities.</param>
    void Update(IEnumerable<TEntity> entities);

    /// <summary>
    /// Delete entity.
    /// </summary>
    /// <param name="entity">entity.</param>
    void Delete(TEntity entity);
    
    /// <summary>
    /// Delete entities.
    /// </summary>
    /// <param name="entities">entities.</param>
    void Delete(IEnumerable<TEntity> entities);
    
    
}