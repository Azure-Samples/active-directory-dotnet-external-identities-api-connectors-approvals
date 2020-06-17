using CustomApproval.Web.Models.GraphApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomApproval.Web.Services
{
    public interface IGraphClientService
    {
        Task CreateUser(Dictionary<string, object> user);

        Task<InviteGuestUserOutputModel> InviteGuestUser(Dictionary<string, object> user);

        Task UpdateUser(Dictionary<string, object> user, string id);
    }
}
