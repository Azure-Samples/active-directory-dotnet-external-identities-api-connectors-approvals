using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomApproval.Web.Models.GraphApi
{
    public class InviteGuestUserOutputModel
    {
        public InvitedUser invitedUser { get; set; }
    }

    public class InvitedUser
    {
        public string id { get; set; }
    }
}
