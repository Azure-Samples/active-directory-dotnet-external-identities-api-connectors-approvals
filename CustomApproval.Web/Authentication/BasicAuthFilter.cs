namespace CustomApproval.Authentication
{
    using System;
    using System.Net;
    using System.Net.Http.Headers;
    using System.Text;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.Options;
    using Microsoft.Extensions.Configuration;
    using Web.Models.Configuration;

    public class BasicAuthFilter : IAuthorizationFilter
    {

        private IConfiguration _config;

        public BasicAuthFilter(IConfiguration config)
        {
            _config = config.GetSection("BasicAuth");
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            try
            {
                string authHeader = context.HttpContext.Request.Headers["Authorization"];
                if (authHeader != null)
                {
                    var authHeaderValue = AuthenticationHeaderValue.Parse(authHeader);
                    if (authHeaderValue.Scheme.Equals(AuthenticationSchemes.Basic.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        var credentials = Encoding.UTF8
                            .GetString(Convert.FromBase64String(authHeaderValue.Parameter ?? string.Empty))
                            .Split(':', 2);
                        if (credentials.Length == 2)
                        {
                            if (IsAuthorized(context, credentials[0], credentials[1]))
                            {
                                return;
                            }
                        }
                    }
                }

                ReturnUnauthorizedResult(context);
            }
            catch (FormatException)
            {
                ReturnUnauthorizedResult(context);
            }
        }

        public bool IsAuthorized(AuthorizationFilterContext context, string username, string password)
        {
            return IsValidUser(username, password);
        }

        private bool IsValidUser(string username, string password)
        {
            if (username.Equals(_config.GetValue(typeof(string), "ApiUsername")) && password.Equals(_config.GetValue(typeof(string), "ApiPassword")))
            {
                return true;
            }

            return false;
        }

        private void ReturnUnauthorizedResult(AuthorizationFilterContext context)
        {
            context.Result = new UnauthorizedResult();
        }
    }
}
