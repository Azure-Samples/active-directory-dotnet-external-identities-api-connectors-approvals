using CustomApproval.Web.Entities;
using CustomApproval.Web.Repositories.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomApproval.Web.Repositories
{
    public interface IRepository<TEntity>
        where TEntity : EntityBase
    {
        Task<TEntity> AddAsync(TEntity entity);

        Task<TEntity> GetAsync(string id);

        Task<IEnumerable<TEntity>> ListAsync();

        Task<IEnumerable<TEntity>> ListAsync(ISpecification<TEntity> specification);

        Task RemoveAsync(string id);

        Task UpdateAsync(TEntity entity);
    }
}
