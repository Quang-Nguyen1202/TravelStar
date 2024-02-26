using System.Linq.Expressions;
using TravelStar.Repositories.UnitOfWork;

namespace TravelStar.Business.Interfaces;
public interface IBaseBo<TModel, TEntity>
        where TModel : class
        where TEntity : class
{
    TEntity GetById(IUnitOfWork unitOfWork, object id);
    IQueryable<TEntity> GetAll(IUnitOfWork unitOfWork);
    IQueryable<TEntity> GetAllWithInclude(IUnitOfWork unitOfWork, params Expression<Func<TEntity, object>>[] includes);
    IQueryable<TEntity> GetEntitiesWithInclude(IUnitOfWork unitOfWork, Expression<Func<TEntity, bool>> filter, params Expression<Func<TEntity, object>>[] includes);
    IQueryable<TEntity> GetEntities(IUnitOfWork unitOfWork);
    IQueryable<TEntity> GetEntities(IUnitOfWork unitOfWork, Expression<Func<TEntity, bool>> filter);
    TEntity InsertEntity(TEntity item, IUnitOfWork unitOfWork);
    IEnumerable<TEntity> InsertEntities(IEnumerable<TEntity> items, IUnitOfWork unitOfWork);
    TEntity UpdateEntity(TEntity item, object id, IUnitOfWork unitOfWork);
    bool DeleteEntity(TEntity entity, IUnitOfWork unitOfWork);

    IUnitOfWork NewDbContext();
}
