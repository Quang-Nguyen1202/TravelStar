using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TravelStar.Repositories;
 public interface IGenericRepositories<TEntity> where TEntity : class
{
    TEntity Find(object id);
    IQueryable<TEntity> GetAll();
    IQueryable<TEntity> GetAll(params Expression<Func<TEntity, object>>[] includes);
    TEntity GetById(object id);
    IQueryable<TEntity> GetEntities();
    IQueryable<TEntity> GetEntities(Expression<Func<TEntity, bool>> filter);
    TEntity Insert(TEntity entityToInsert);
    IEnumerable<TEntity> Insert(IEnumerable<TEntity> entitiesToInsert);
    TEntity Update(TEntity entityToUpdate);
    IQueryable<TEntity> GetEntitiesWithInclude(Expression<Func<TEntity, bool>> filter, params Expression<Func<TEntity, object>>[] includes);
    bool Delete(TEntity entityToDelete);
}