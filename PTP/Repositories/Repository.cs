﻿using Microsoft.EntityFrameworkCore;
using PTP.Core.Domain.Entities;
using PTP.Core.Interfaces.Repositories;
using PTP.Database;
using System.Reflection.Metadata.Ecma335;

namespace PTP.Repositories
{
    public class Repository<T> : IRepository<T> where T : Entity
    {
        private readonly PTPContext _ptpContext;
        private readonly DbSet<T> _set;

        public DbSet<T> DbSet {get { return _set;} }

        public Repository(PTPContext ptpContext)
        {
            _ptpContext = ptpContext;
            _set = _ptpContext.Set<T>();
        }
       
        public IQueryable<T> Get()
        {
            return _set.Where(x => true);
        }

        public async Task<T?> GetAsync(int id, CancellationToken cancellationToken = default)
        {
            return await Get().SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            await _set.AddRangeAsync(entities, cancellationToken);
        }

        public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _set.AddAsync(entity, cancellationToken);
        }

        public void Delete(params T[] entities)
        {
            _set.RemoveRange(entities);
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _ptpContext.SaveChangesAsync(cancellationToken);
        }

        public void Attach(params T[] entities)
        {
            _set.AttachRange(entities);
        }

        public void Update(params T[] entities)
        {
            _set.UpdateRange(entities);
            foreach (T entity in entities)
            {
                _ptpContext.Entry(entity).Property(a => a.Version).OriginalValue = entity.Version;
            }
        }

        public async Task<T?> GetAsyncNoTracking(int id, CancellationToken cancellationToken = default)
        {
            return await Get().AsNoTracking().SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<IEnumerable<T>?> GetAllAsyncNoTracking(CancellationToken cancellationToken = default)
        {
            return await Get().AsNoTracking().ToListAsync(cancellationToken);
        }
    }
}
