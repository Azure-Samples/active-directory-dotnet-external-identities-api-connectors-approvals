using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomApproval.Web.Models
{
    public class CheckApprovalStatusInputModel
    {
        public string email { get; set; }

        public List<IdentityModel> identities { get; set; }
    }
}
