using CustomApproval.Web.Entities;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomApproval.Web.Models
{
    public class RegisterForApprovalInputModel
    {
        public string email { get; set; }

        public List<IdentityModel> identities { get; set; }

        public string displayName { get; set; }

        public string givenName { get; set; }

        public string surName { get; set; }

        public string phoneNumber { get; set; }

        public string ui_locales { get; set; }

        public string inputData { get; set; }

        public UserData CreateUserData()
        {
            var data = new UserData();
            data.Email = email;
            data.DisplayName = displayName;
            data.GivenName = givenName;
            data.SurName = surName;
            data.PhoneNumber = phoneNumber;
            data.Identities = identities?.Select(x => new IdentityData() { Id = Guid.NewGuid().ToString(), Issuer = x.issuer, IssuerAssignedId = x.issuerAssignedId, SignInType = x.signInType }).ToList();
            data.Locale = ui_locales;
            data.InputData = inputData; 
            data.ApprovalStatus = Constants.UserApprovalStatus.Pending;

            return data;
        }
    }
}
