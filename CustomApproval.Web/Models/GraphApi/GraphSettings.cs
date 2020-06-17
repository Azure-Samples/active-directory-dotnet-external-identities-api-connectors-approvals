using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace CustomApproval.Web.Models.GraphApi
{
    public class GraphSettings
    {
        public string Tenant { get; set; }

        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public string Instance => "https://login.microsoftonline.com/{0}";
        public string ApiUrl => "https://graph.microsoft.com/";

        public string ApiVersion => "v1.0";


        public string[] scopes = new string[] { "https://graph.microsoft.com/.default" };
        public string Authority
        {
            get
            {
                return String.Format(CultureInfo.InvariantCulture, Instance, Tenant);
            }
        }
    }
}
