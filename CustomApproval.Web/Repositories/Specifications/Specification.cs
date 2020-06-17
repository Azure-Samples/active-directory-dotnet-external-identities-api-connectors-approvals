using CustomApproval.Web.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CustomApproval.Web.Repositories.Specifications
{
    public class Specification<TEntity> : ISpecification<TEntity> where TEntity : EntityBase
    {

        public Specification(Expression<Func<TEntity, bool>> criteria)
        {
            Criteria = criteria ?? throw new ArgumentNullException(nameof(criteria));
        }
        public Expression<Func<TEntity, bool>> Criteria { get; }
    }
}
