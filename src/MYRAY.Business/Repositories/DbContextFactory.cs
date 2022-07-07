using System.Collections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using MYRAY.Business.Repositories.Interface;

namespace MYRAY.Business.Repositories;

/// <summary>
/// DbContext Factory
/// </summary>
public class DbContextFactory : IDbContextFactory, IDisposable
{
    private bool _disposed;
    private bool _completed;
    
    /// <summary>
    /// Gets or sets configuration.
    /// </summary>
    public IConfiguration Configuration { get; set; }
    
    public Hashtable? Contexts { get; set; }

    public DbContextFactory(IConfiguration configuration)
    {
        Configuration = configuration;
    }


    public IUnitOfWork<TContext> GetContext<TContext>() where TContext : DbContext
    {
        if (Contexts == null)
        {
            Contexts = new Hashtable();
        }

        string key = typeof(TContext).Name;
        if (!Contexts.ContainsKey(key))
        {
            string nameOfConectionString = Configuration
                .GetConnectionString($"{typeof(TContext).Name.Replace("Context", String.Empty)}");
            TContext contextInstance = (TContext) Activator.CreateInstance(typeof(TContext), ConfigureSqlServer<TContext>(nameOfConectionString).Options);
            UnitOfWork<TContext> unitOfWorkInstance = new UnitOfWork<TContext>(contextInstance);
            Contexts.Add(key, unitOfWorkInstance);
        }

        return (Contexts[key] as IUnitOfWork<TContext>)!;
    }

    public async Task<int> SaveAllAsync()
    {
        if (_disposed)
        {
            throw new ObjectDisposedException("UoW disposed");
        }

        if (_completed)
        {
            throw new InvalidOperationException("Can not SaveChange()");
        }

        int rowAffected = 0;
        try
        {
            foreach (DictionaryEntry dictionaryEntry in Contexts)
            {
                dynamic dbContext = GetContextFromCollection(dictionaryEntry);
                rowAffected += await (dbContext as DbContext)?.SaveChangesAsync()!;
            }

            foreach (DictionaryEntry dictionaryEntry in Contexts)
            {
                dynamic dbContext = GetContextFromCollection(dictionaryEntry);
                IDbContextTransaction transaction = (dbContext as DbContext).Database.CurrentTransaction;
                if (transaction != null)
                {
                    transaction.Commit();
                    transaction.Dispose();
                }
            }
        }
        catch (Exception e)
        {
            await RollbackAsync();
            throw new Exception($"{nameof(SaveAllAsync)} :", e);
        }

        _completed = true;

        return rowAffected;
    }

    private dynamic GetContextFromCollection(DictionaryEntry dbCtxColection)
    {
        return dbCtxColection.Value.GetType().GetProperty(nameof(UnitOfWork<DbContext>.CurrentContext))
            .GetValue(dbCtxColection.Value);
    }

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
            foreach (DictionaryEntry dictionaryEntry in Contexts)
            {
                dynamic dbctx = GetContextFromCollection(dictionaryEntry);

                var transaction = (dbctx as DbContext)?.Database.CurrentTransaction;
                if (transaction != null)
                {
                    await transaction.RollbackAsync();
                    await transaction.DisposeAsync();
                }
            }
        }
        catch (Exception e)
        {
            throw new Exception($"{nameof(RollbackAsync)}:", e);
        }

        _completed = true;
    }

    public async void Dispose()
    {
        (Configuration as IDisposable)?.Dispose();
        if (Contexts.Count > 0)
        {
            try
            {
                if (!_completed)
                {
                    await RollbackAsync();
                }

                if (!_disposed)
                {
                    foreach (DictionaryEntry dE in Contexts)
                    {
                        dynamic dbctx = GetContextFromCollection(dE);
                        await ((dbctx as DbContext)!).DisposeAsync();
                    }
                }

                _disposed = true;
            }
            catch (Exception e)
            {
                throw new Exception($"{nameof(Dispose)}:", e);
            }
        }
    }


    private DbContextOptionsBuilder<TContext> ConfigureSqlServer<TContext>(string connectionString) where TContext : DbContext
    {
        DbContextOptionsBuilder<TContext> optionsBuilder = new DbContextOptionsBuilder<TContext>();
        optionsBuilder.UseSqlServer(connectionString)
            .UseLazyLoadingProxies();
        return optionsBuilder;
    }
}