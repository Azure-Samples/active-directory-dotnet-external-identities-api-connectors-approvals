using CustomApproval.Web.Models;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CustomApproval.Web.Services
{
    public class MailService : IMailService
    {
        private SendGridSettings sendGridSettings;
        private AppSettings appSettings;
        public MailService(IConfiguration config)
        {
            sendGridSettings = config.GetSection("SendGrid")
              .Get<SendGridSettings>();
            appSettings = config.GetSection("AppSettings")
              .Get<AppSettings>();
        }

        public async Task SendApprovalNotification(string email, string locale)
        {
            if (string.IsNullOrEmpty(locale) || !Constants.CustomLocalization.ContainsKey(locale.ToLower()))
            {
                locale = appSettings.DefaultLocale;
            }

            var localizedvalues = Constants.CustomLocalization[locale?.ToLower()];

            await SendEmail(email, localizedvalues.EmailApproveSubject, localizedvalues.EmailApproveHtmlContent.Replace("{Settings:RedirectUrl}", appSettings.ParentAppRedirectUrl));
        }


        private async Task SendEmail(string toEmail, string subject, string content)
        {
            var client = new SendGridClient(sendGridSettings.ApiKey);
            var from = new EmailAddress(sendGridSettings.FromEmail);
            var to = new EmailAddress(toEmail);
            var plainTextContent = Regex.Replace(content, "<[^>]*>", "");
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, content);
            await client.SendEmailAsync(msg);
        }
    }
}
