using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomApproval.Web.Models
{
    public class IdentityModel
    {
        public string signInType { get; set; }

        public string issuer { get; set; }

        public string issuerAssignedId { get; set; }
    }
}
