using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TravelStar.Entities;

namespace TravelStar.Repositories;
 public class GenericRepositories<TEntity> : IGenericRepositories<TEntity> where TEntity : class
{
    //DBContext
    private readonly TravelStarContext context;
    private DbSet<TEntity> entities;
    string errorMessage = string.Empty;

    public GenericRepositories(TravelStarContext _context)
    {
        this.context = _context;
        entities = context.Set<TEntity>();
    }

    public TEntity Find(object id)
    {
        return entities.Find(id)!;
    }
    public bool Delete(TEntity entityToDelete)
    {
        if (entityToDelete == null)
        {
            throw new ArgumentNullException("entityToDelete");
        }
        if (entities.Remove(entityToDelete) != null)
        {
            return true;
        }

        return false;
    }

    public IQueryable<TEntity> GetAll()
    {
        return entities.AsQueryable();
    }

    public IQueryable<TEntity> GetAll(params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = this.entities;
        return includes.Aggregate(query, (current, inc) => current.Include(inc)).AsQueryable();
    }

    public virtual TEntity GetById(object id)
    {
        return entities.Find(id)!;
    }

    public IQueryable<TEntity> GetEntities()
    {
        return entities;
    }

    public IQueryable<TEntity> GetEntities(Expression<Func<TEntity, bool>> filter)
    {
        return entities.Where(filter);
    }

    public TEntity Insert(TEntity entityToInsert)
    {
        if (entityToInsert == null)
        {
            throw new ArgumentNullException("entityToInsert");
        }
        entities.Add(entityToInsert);
        return entityToInsert;
    }

    public IEnumerable<TEntity> Insert(IEnumerable<TEntity> entitiesToInsert)
    {
        if (entitiesToInsert == null)
        {
            throw new ArgumentNullException("entitiesToInsert");
        }
        entities.AddRange(entitiesToInsert);
        return entitiesToInsert;
    }

    public TEntity Update(TEntity entityToUpdate)
    {
        if (entityToUpdate == null)
        {
            throw new ArgumentNullException("entityToUpdate");
        }
        context.Entry(entityToUpdate).State = EntityState.Modified;
        return entityToUpdate;
    }

    public IQueryable<TEntity> GetEntitiesWithInclude(Expression<Func<TEntity, bool>> filter, params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = this.entities.Where(filter);
        return includes.Aggregate(query, (current, inc) => current.Include(inc));
    }
}
