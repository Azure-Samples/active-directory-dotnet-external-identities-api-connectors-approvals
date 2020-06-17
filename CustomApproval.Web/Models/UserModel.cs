using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomApproval.Web.Models
{
    public class UserModel
    {
        public string Id { get; set; }

        public string DisplayName { get; set; }

        public string GivenName { get; set; }

        public string SurName { get; set; }

        public string Email { get; set; }

        public string AlternativeSecurityId { get; set; }

        public string PhoneNumber { get; set; }

        public string ApprovalStatus { get; set; }

        public DateTime LastUpdatedTime { get; set; }

        public string StatusWithTime { get { return $"{ApprovalStatus} - {LastUpdatedTime.ToString("MM/dd/yyyy hh:mm tt")}"; } }

        public string IdentityProvider { get; set; }

        public string Locale { get; set; }

        public string Identities { get; set; }
    }
}
