using System.Collections;
using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MYRAY.Business.Repositories.Interface;

namespace MYRAY.Business.Repositories;

/// <summary>
/// Unit of work
/// </summary>
/// <typeparam name="TContext">DbContext</typeparam>
public class UnitOfWork<TContext> : IUnitOfWork<TContext> where TContext : DbContext
{
    private IDbContextTransaction? _transaction;
    private bool _disposed;
    private bool _completed;
    
    /// <summary>
    /// Gets or sets current context.
    /// </summary>
    public DbContext CurrentContext { get; set; }

    /// <summary>
    /// Gets or sets collection of repository.
    /// </summary>
    public Hashtable? Repositories { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="UnitOfWork{TContext}"/>
    /// </summary>
    /// <param name="currentContext"></param>
    public UnitOfWork(DbContext currentContext)
    {
        _disposed = _completed = false;
        CurrentContext = currentContext;
        _transaction = CurrentContext.Database?.CurrentTransaction;
    }

    /// <inheritdoc cref="IUnitOfWork{TContext}.SaveChangeAsync"/>
    public async Task<int> SaveChangeAsync()
    {
        if (_disposed)
        {
            throw new ObjectDisposedException("UoW disposed");
        }

        if (_completed)
        {
            throw new InvalidOperationException("Can not SaveChange()");
        }

        int rowAffect = 0;

        try
        {
            IDbContextTransaction? currentTransaction = CurrentContext.Database.CurrentTransaction;
            if (currentTransaction != null)
            {
                currentTransaction = await CurrentContext.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);
            }

            _transaction = currentTransaction;

            rowAffect = await CurrentContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            await RollbackAsync(); 
            throw new Exception($"{nameof(UnitOfWork<TContext>)} SaveChange(): ", e);
        }

        return rowAffect;
    }
    
    /// <inheritdoc cref="IUnitOfWork{TContext}.RollbackAsync"/>
    public async Task RollbackAsync()
    {
        if (_disposed)
        {
            throw new ObjectDisposedException("UoW disposed");
        }

        if (_completed)
        {
            throw new InvalidOperationException("Can not SaveChange()");
        }

        try
        {
            _transaction = CurrentContext.Database.CurrentTransaction;
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
            }
        }
        catch (Exception e)
        {
            throw new Exception($"{nameof(UnitOfWork<TContext>)} RollBack: ", e);
        }

        _completed = true;
    }
    
    /// <inheritdoc cref="IUnitOfWork{TContext}.GetRepository{T}"/>
    public IBaseRepository<T> GetRepository<T>() where T : class
    {
        if (Repositories == null)
        {
            Repositories = new Hashtable();
        }

        string key = typeof(T).Name;

        if (!Repositories.ContainsKey(key))
        {
            var repositoryInstance = new BaseRepository<T>(CurrentContext);
            Repositories.Add(key, repositoryInstance);
        }

        return Repositories[key] as IBaseRepository<T>;
    }
}