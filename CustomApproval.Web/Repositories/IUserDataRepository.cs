using CustomApproval.Web.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomApproval.Web.Repositories
{
    public interface IUserDataRepository : IRepository<UserData>
    {
        public Task<UserData> GetByEmail(string email);

        Task<UserData> GetByEmailAndIdentityIssuers(string email, List<string> issuers);

        Task<IEnumerable<UserData>> GetListByEmail(string email);
    }
}
