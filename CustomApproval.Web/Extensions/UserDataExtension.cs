using CustomApproval.Web.Entities;
using CustomApproval.Web.Models;
using CustomApproval.Web.Models.GraphApi;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomApproval.Web.Extensions
{
    public static class UserDataExtension
    {
        public static IEnumerable<UserModel> ToUserModel(this IEnumerable<UserData> userData)
        {
            var result = new List<UserModel>();

            if (userData == null || !userData.Any())
            {
                return result;
            }

            foreach (var item in userData)
            {
                result.Add(item.ToUserModel());
            }

            return result;
        }

        public static UserModel ToUserModel(this UserData userData)
        {
            if (userData == null)
            {
                return null;
            }

            var identityProvider = userData?.Identities?.First()?.Issuer.ToString() ?? "Microsoft.com";

            return new UserModel()
            {
                Id = userData.Id,
                AlternativeSecurityId = userData.AlternativeSecurityId,
                ApprovalStatus = userData.ApprovalStatus,
                DisplayName = userData.DisplayName,
                Email = userData.Email,
                GivenName = userData.GivenName,
                LastUpdatedTime = userData.LastUpdatedTime,
                PhoneNumber = userData.PhoneNumber,
                SurName = userData.SurName,
                Locale = userData.Locale,
                IdentityProvider = identityProvider,
                Identities = JsonConvert.SerializeObject(userData.Identities)
            };
        }

        public static bool IsSocialUser(this UserData userData)
        {
            var identityProvider = userData?.Identities?.First()?.Issuer.ToString() ?? string.Empty;
            return !string.IsNullOrEmpty(identityProvider);
        }

        public static Dictionary<string, object> ToSocialUserInput(this UserData userData, string tenant)
        {
            var data = new Dictionary<string, object>();
            data.Add("displayName", userData.DisplayName);
            data.Add("mail", userData.Email);
            data.Add("userPrincipalName", $"{userData.Email.Replace("@", "_")}#EXT@{tenant}");
            data.Add("mailNickname", userData.Email.Split("@")[0]);
            data.Add("accountEnabled", true);
            data.Add("userType", "Guest");
            data.Add("passwordPolicies", "DisablePasswordExpiration");

            if (userData.Identities != null && userData.Identities.Any())
            {
                data.Add("identities", userData.Identities.Select(x => new { signInType = x.SignInType, issuer = x.Issuer, issuerAssignedId = x.IssuerAssignedId }).ToArray());
            }

            var customAttributes = GetCustomAttributes(userData);
            if (customAttributes.Any())
            {
                data = data.Union(customAttributes).ToDictionary(d => d.Key, d => d.Value);
            }

            return data;
        }

        public static Dictionary<string, object> ToInviteGuestUserInput(this UserData userData, string redirectUrl)
        {
            var data = new Dictionary<string, object>();
            data.Add("invitedUserEmailAddress", userData.Email);
            data.Add("inviteRedirectUrl", redirectUrl);
            data.Add("sendInvitationMessage", true);
            return data;
        }

        public static Dictionary<string, object> ToUpdateGuestUserInput(this UserData userData)
        {
            var data = new Dictionary<string, object>();
            data.Add("displayName", userData.DisplayName);
            data.Add("givenName", userData.GivenName);
            data.Add("surname", userData.SurName);

            var customAttributes = GetCustomAttributes(userData);
            if (customAttributes.Any())
            {
                data = data.Union(customAttributes).ToDictionary(d => d.Key, d => d.Value);
            }

            return data;
        }

        /// <summary>
        ///     Generate temporary password
        /// </summary>
        private static string GeneratePassword()
        {
            const string A = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string a = "abcdefghijklmnopqrstuvwxyz";
            const string num = "1234567890";
            const string spe = "!@#$!&";

            var rv = GenerateLetters(4, A) + GenerateLetters(4, a) + GenerateLetters(4, num) + GenerateLetters(1, spe);
            return rv;
        }

        /// <summary>
        ///     Generate random letters from string of letters
        /// </summary>
        private static string GenerateLetters(int length, string baseString)
        {
            var res = new StringBuilder();
            var rnd = new Random();
            while (0 < length--)
            {
                res.Append(baseString[rnd.Next(baseString.Length)]);
            }

            return res.ToString();
        }

        private static Dictionary<string, object> GetCustomAttributes(UserData userData)
        {
            var data = new Dictionary<string, object>();

            if (string.IsNullOrEmpty(userData?.InputData))
            {
                return data;
            }

            var inputData = JObject.Parse(userData.InputData);

            foreach (var token in inputData)
            {
                if (token.Key.StartsWith("extension_"))
                {
                    data.Add(token.Key, token.Value);
                }
            }

            return data;
        }
    }
}
