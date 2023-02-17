using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WebApiDemo.Common.Models;

namespace WebApiDemo.Services.Infrastructure.Repasitories
{
    public  interface IRepasitory<TEntity>
    {
        Task<IEnumerable<TEntity>> GetAsync();
        Task<IEnumerable<TEntity>> GetNoTrackingAsync(Expression<Func<TEntity, bool>>? predicate = default);
        Task Add(TEntity entity);
        Task AddRange(IEnumerable<TEntity> entities);
        Task Update(TEntity entity);
        Task Remove(TEntity entity);
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
