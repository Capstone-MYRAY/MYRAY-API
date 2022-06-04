namespace MYRAY.Business.Repositories.Interface;

/// <summary>
/// Interface for UnitOfWork
/// </summary>
/// <typeparam name="TContext"></typeparam>
public interface IUnitOfWork<TContext>
{
    /// <summary>
    /// Save change async in one context
    /// </summary>
    /// <returns>number of row affected.</returns>
    Task<int> SaveChangeAsync();
    
    /// <summary>
    /// Roll back transaction of all contexts.
    /// </summary>
    /// <returns></returns>
    Task RollbackAsync();
    
    /// <summary>
    /// Get repository based on entity.
    /// </summary>
    /// <typeparam name="T">entity class.</typeparam>
    /// <returns>a repository of an entity.</returns>
    IBaseRepository<T>? GetRepository<T>() where T : class;
}