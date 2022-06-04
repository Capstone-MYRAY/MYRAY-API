using Microsoft.EntityFrameworkCore;

namespace MYRAY.Business.Repositories.Interface;
/// <summary>
/// Interface for DbContextFactory
/// </summary>
public interface IDbContextFactory
{
    /// <summary>
    /// Get the dbcontext when using factory.
    /// </summary>
    /// <typeparam name="T">db context.</typeparam>
    /// <returns>UoW</returns>
    IUnitOfWork<T> GetContext<T>() where T : DbContext;

    /// <summary>
    /// Save change for all context in UoW with async.
    /// </summary>
    /// <returns>number of row affected.</returns>
    Task<int> SaveAllAsync();

    /// <summary>
    /// Roll back the transaction of all contexts.
    /// </summary>
    /// <returns>task async.</returns>
    Task RollbackAsync();
}