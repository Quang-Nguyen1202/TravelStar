using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;
using TravelStar.Business.Interfaces;
using TravelStar.Entities;
using TravelStar.Repositories.UnitOfWork;

namespace TravelStar.Business.Implements;
public class BaseBo<TModel, TEntity> : IBaseBo<TModel, TEntity>
       where TModel : class
       where TEntity : class
{
    protected readonly IServiceProvider _serviceProvider;

    public BaseBo(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IUnitOfWork NewDbContext()
    {
        var newContext = _serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope()
            .ServiceProvider.GetRequiredService<TravelStarContext>();
        return new UnitOfWork(newContext);
    }

    public virtual IQueryable<TEntity> GetAll(IUnitOfWork unitOfWork)
    {
        try
        {
            var models = new List<TModel>();

            // Get entity from data context.
            // Get repository.
            var repository = unitOfWork.Repository<TEntity>();

            // Get all entity in datacontext.
            var entities = repository.GetAll().AsNoTracking();

            return entities.AsQueryable();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public virtual IQueryable<TEntity> GetAllWithInclude(IUnitOfWork unitOfWork, params Expression<Func<TEntity, object>>[] includes)
    {
        try
        {
            var models = new List<TModel>();

            // Get entity from data context.
            // Get repository.
            var repository = unitOfWork.Repository<TEntity>();

            // Get all entity in datacontext.
            var entities = repository.GetAll(includes);

            return entities.AsQueryable();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public virtual TEntity GetById(IUnitOfWork unitOfWork, object id)
    {
        // Get entity from data context.

        // Get repository.
        var repository = unitOfWork.Repository<TEntity>();

        var item = repository.GetById(id);

        return item;
    }
    
    public virtual TEntity InsertEntity(TEntity item, IUnitOfWork unitOfWork)
    {
        try
        {
            // Get repository.
            var repository = unitOfWork.Repository<TEntity>();

            // Insert TEntity

            var entityResult = repository.Insert(item);

            // Save change to datacontext.
            unitOfWork.Save();

            // Return TModel
            return entityResult;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public virtual IEnumerable<TEntity> InsertEntities(IEnumerable<TEntity> items, IUnitOfWork unitOfWork)
    {
        var repository = unitOfWork.Repository<TEntity>();

        var entityResult = repository.Insert(items);

        // Save change to datacontext.
        unitOfWork.Save();

        return entityResult;
    }

    public virtual TEntity UpdateEntity(TEntity item, object id, IUnitOfWork unitOfWork)
    {
        try
        {
            // Get repository.
            var repository = unitOfWork.Repository<TEntity>();

            // Get Entity
            var entity = repository.Find(id);

            // Update TEntity
            var entityResult = repository.Update(entity);

            // Save change to datacontext.
            unitOfWork.Save();

            // Return TModel
            return entityResult;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public IQueryable<TEntity> GetEntitiesWithInclude(IUnitOfWork unitOfWork, Expression<Func<TEntity, bool>> filter, params Expression<Func<TEntity, object>>[] includes)
    {
        var repository = unitOfWork.Repository<TEntity>();
        return repository.GetEntitiesWithInclude(filter, includes);
    }

    public IQueryable<TEntity> GetEntities(IUnitOfWork unitOfWork)
    {
        var repository = unitOfWork.Repository<TEntity>();
        return repository.GetEntities();
    }

    public IQueryable<TEntity> GetEntities(IUnitOfWork unitOfWork, Expression<Func<TEntity, bool>> filter)
    {
        var repository = unitOfWork.Repository<TEntity>();
        return repository.GetEntities(filter);
    }

    public virtual bool DeleteEntity(TEntity item, IUnitOfWork unitOfWork)
    {
        try
        {
            // Get repository.
            var repository = unitOfWork.Repository<TEntity>();

            // Delete model.
            repository.Delete(item);

            // Save change to datacontext
            unitOfWork.Save();

            return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}
