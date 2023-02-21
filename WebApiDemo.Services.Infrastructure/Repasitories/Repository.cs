using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WebApiDemo.Common.Models;
using WebApiDemo.Services.Infrastructure.Repasitories;

namespace WebApiDemo.Services.Infrastructure
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity:BaseModel
    {
        protected readonly DbContext _db;
        public Repository(DbContext db) => _db = db;
        public Task Add(TEntity entity)
        {
            return Task.FromResult(_db.Set<TEntity>().Add(entity));
        }

        public Task AddRange(IEnumerable<TEntity> entities)
        {
            return Task.FromResult(_db.Set<TEntity>().AddRange(entities));
        }

        public async  Task<IEnumerable<TEntity>> GetAsync()
        {
            return await _db.Set<TEntity>().ToListAsync();
        }

        public async  Task<IEnumerable<TEntity>> GetNoTrackingAsync(Expression<Func<TEntity, bool>>? predicate = null)
        {
            var query = Include(_db.Set<TEntity>());
            query = predicate is not null ? query.Where(predicate).AsQueryable() : query.AsQueryable();
            var entities = await query.AsNoTracking().ToListAsync();

            return entities;
        }

        public Task Remove(TEntity entity)
        {
            return Task.FromResult(_db.Set<TEntity>().Remove(entity));
        }

        public async  Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
             await _db.SaveChangesAsync(cancellationToken);
        }

        public Task Update(TEntity entity)
        {
            _db.Set<TEntity>().AddOrUpdate(entity);
            return Task.CompletedTask;
        }
        protected virtual IQueryable<TEntity> Include(DbSet<TEntity> set) => set;

    }
}
