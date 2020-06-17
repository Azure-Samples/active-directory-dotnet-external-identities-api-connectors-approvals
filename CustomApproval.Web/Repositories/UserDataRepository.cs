using CustomApproval.Web.Entities;
using CustomApproval.Web.EntityFramework;
using CustomApproval.Web.Repositories.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomApproval.Web.Repositories
{
    public class UserDataRepository : DbRepository<UserData>, IUserDataRepository
    {
        public UserDataRepository(CustomApprovalDbContext dbContext)
            : base(dbContext)
        {
        }

        public async Task<UserData> GetByEmail(string email)
        {
            var users = await ListAsync(new Specification<UserData>(x => x.Email == email));

            return users.FirstOrDefault();
        }

        public async Task<UserData> GetByEmailAndIdentityIssuers(string email, List<string> issuers)
        {
            var users = await ListAsync(new Specification<UserData>(x => x.Email == email && (issuers == null || !issuers.Any() || (x.Identities != null && x.Identities.Any(x => issuers.Contains(x.Issuer))))));

            return users.FirstOrDefault();
        }

        public async Task<IEnumerable<UserData>> GetListByEmail(string email)
        {
            return await ListAsync(new Specification<UserData>(x => x.Email == email));
        }
    }
}
