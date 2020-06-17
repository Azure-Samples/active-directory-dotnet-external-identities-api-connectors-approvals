using CustomApproval.Web.Entities;
using CustomApproval.Web.EntityFramework;
using CustomApproval.Web.Repositories.Specifications;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomApproval.Web.Repositories
{
    public class DbRepository<TEntity> : IRepository<TEntity>
      where TEntity : EntityBase
    {
        protected CustomApprovalDbContext DbContext { get; }
        public DbRepository(CustomApprovalDbContext dbContext)
        {
            DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            entity.LastUpdatedTime = DateTime.Now;

            DbContext.Set<TEntity>()
                .Add(entity);

            await DbContext.SaveChangesAsync();

            return entity;
        }

        public Task<TEntity> GetAsync(string id)
        {
            return DbContext.Set<TEntity>().FindAsync(id).AsTask();
        }

        public async Task<IEnumerable<TEntity>> ListAsync()
        {
            return await DbContext.Set<TEntity>()
                .ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> ListAsync(ISpecification<TEntity> specification)
        {
            return await DbContext.Set<TEntity>()
                .Where(specification.Criteria)
                .ToListAsync();
        }

        public async Task RemoveAsync(string id)
        {
            var entity = await GetAsync(id);

            DbContext.Set<TEntity>()
                .Remove(entity);

            await DbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(TEntity entity)
        {
            entity.LastUpdatedTime = DateTime.Now;
            DbContext.Entry(entity).State = EntityState.Modified;
            await DbContext.SaveChangesAsync();
        }
    }
}
