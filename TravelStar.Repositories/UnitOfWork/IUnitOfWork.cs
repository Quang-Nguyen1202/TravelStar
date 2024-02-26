using TravelStar.Entities;

namespace TravelStar.Repositories.UnitOfWork;
 public interface IUnitOfWork : IDisposable
{
    void BeginTransaction();

    void CommitTransaction();

    void RollBackTransaction();

    TravelStarContext GetDbContext();

    IGenericRepositories<T> Repository<T>() where T : class;

    bool Save();
    Task<bool> SaveAsync();
}
