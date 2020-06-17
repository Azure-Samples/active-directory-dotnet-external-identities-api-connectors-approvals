using CustomApproval.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomApproval.Web
{
    public static class Constants
    {
        public static class UserApprovalStatus
        {
            public static string Approved => "Approved";

            public static string Denied => "Denied";

            public static string Pending => "Pending";
        }

        public static Dictionary<string, CustomLocaleValue> CustomLocalization => new Dictionary<string, CustomLocaleValue>()
        {
            {
                "en-us", new CustomLocaleValue
                {
                    EmailApproveSubject = "You have been approved",
                    EmailApproveHtmlContent =
                    "<p>Hi,</p><p>You have been approved as a partner with Woodgrove.</p>Please <a href='{Settings:RedirectUrl}'>click here </a> to continue to the site.<p>Regards,</p><p>The WoodGrove Team</p>"
                }
             },
            {
                "es", new CustomLocaleValue
                {
                    EmailApproveSubject = "Has sido aprobado",
                    EmailApproveHtmlContent =
                                    "<p>Hola,</p><p>Usted ha sido aprobado como socio de Woodgrove.</p>Por favor <a href='{Settings:RedirectUrl}'>haga clic aquí </a> para continuar al sitio.<p>Saludos,</p><p>El equipo de WoodGrove</p>"
                }
            }
        };
    }
}
