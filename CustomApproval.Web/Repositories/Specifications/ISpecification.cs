using CustomApproval.Web.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CustomApproval.Web.Repositories.Specifications
{
    public interface ISpecification<TEntity>
        where TEntity : EntityBase
    {
        Expression<Func<TEntity, bool>> Criteria { get; }
    }
}
