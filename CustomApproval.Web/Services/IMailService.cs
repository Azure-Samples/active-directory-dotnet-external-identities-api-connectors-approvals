using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomApproval.Web.Services
{
    public interface IMailService
    {
        Task SendApprovalNotification(string email, string locale);
    }
}
