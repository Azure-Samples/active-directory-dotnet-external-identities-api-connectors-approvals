using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomApproval.Web.Models
{
    public class SendGridSettings
    {
        public string ApiKey { get; set; }

        public string FromEmail { get; set; }
    }
}
