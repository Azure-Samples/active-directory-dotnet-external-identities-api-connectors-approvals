using CustomApproval.Web.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomApproval.Web.Services
{
    public interface IUserService
    {
        Task CreateUserAsync(UserData userData);

        Task<bool> IsUserExistAsync(string email, List<string> issuers);

        Task UpdateUserStatusAsync(string id, string status);

        Task<UserData> GetUserByEmailAsync(string email);

        Task<UserData> GetUserByEmailAndIdentityIssuersAsync(string email, List<string> issuers);

        Task<IEnumerable<UserData>> GetUsersByEmail(string email);

        Task<UserData> GetUsersById(string id);

        Task RemoveUsersById(string id);
    }
}
