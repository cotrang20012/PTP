using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace PTP.Core.Interfaces.Repositories
{
    public interface IRepository<T> where T : class
    { 
        IQueryable<T> Get();
        Task<T?> GetAsync(int id, CancellationToken cancellationToken = default);
        Task<T?> GetAsyncNoTracking(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<T>?> GetAllAsyncNoTracking(int id, CancellationToken cancellationToken = default);
        Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
        Task AddAsync(T entity, CancellationToken cancellationToken = default);
        void Delete(params T[] entities);
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
        DbSet<T> DbSet { get; }
        void Attach(params T[] entities);
        void Update(params T[] entities);

    }
}
