using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelStar.Entities;

namespace TravelStar.Repositories.UnitOfWork;
public class UnitOfWork : IUnitOfWork
{
    #region -- Fields --
    private bool disposed;
    private Dictionary<Type, object> repositories;
    private IDbContextTransaction transactionScope;
    protected TravelStarContext context;
    #endregion

    public UnitOfWork(TravelStarContext context)
    {
        this.context = context;

        // Initialize data.
        Initialize();
    }

    private void Initialize()
    {
        repositories = new Dictionary<Type, object>();
    }

    public void BeginTransaction()
    {
        if (transactionScope == null)
        {
            transactionScope = context.Database.BeginTransaction();
        }
    }

    public void CommitTransaction()
    {
        if (transactionScope != null)
        {
            var connectionState = transactionScope.GetDbTransaction().Connection!.State;
            if (connectionState != System.Data.ConnectionState.Closed)
            {
                context.SaveChanges();
                transactionScope.Commit();
                transactionScope.Dispose();
                transactionScope = null!;
            }
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!this.disposed)
        {
            if (disposing)
            {
                // Clear data.
                if (repositories != null)
                {
                    repositories.Clear();
                    repositories = null!;
                }

                // Compete and dispose transaction.
                if (transactionScope != null)
                {
                    transactionScope.Commit();
                    transactionScope.Dispose();
                    transactionScope = null!;
                }

                // Dispose context.
                if (context != null)
                {
                    context.Dispose();
                    context = null;
                }
            }
        }

        // Mark dispose.
        this.disposed = true;
    }

    public TravelStarContext GetDbContext()
    {
        return this.context;
    }

    public IGenericRepositories<T> Repository<T>() where T : class
    {
        IGenericRepositories<T> repository = null!;

        if (repositories.ContainsKey(typeof(T)))
        {
            // In case exist, return.
            repository = repositories[typeof(T)] as IGenericRepositories<T>;
        }
        else
        {
            // In case not custom, return generic type.                
            repository = new GenericRepositories<T>(context);

            repositories.Add(typeof(T), repository);
        }

        return repository;
    }

    public void RollBackTransaction()
    {
        if (transactionScope != null)
        {
            transactionScope.Rollback();
            transactionScope.Dispose();
            transactionScope = null!;
        }
    }

    public bool Save()
    {
        try
        {
            return context.SaveChanges() > 0;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<bool> SaveAsync()
    {
        try
        {
            return await context.SaveChangesAsync() > 0;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}
