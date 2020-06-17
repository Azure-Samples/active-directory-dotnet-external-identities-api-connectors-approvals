using CustomApproval.Web.Models.GraphApi;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CustomApproval.Web.Services
{
    public class GraphClientService : IGraphClientService
    {
        private readonly GraphSettings graphSettings;
        public GraphClientService(IConfiguration config)
        {
            graphSettings = config.GetSection("GraphApi")
              .Get<GraphSettings>();
        }

        public async Task CreateUser(Dictionary<string, object> user)
        {
            await SendGraphRequest("/users", JsonConvert.SerializeObject(user), HttpMethod.Post);
        }

        public async Task<InviteGuestUserOutputModel> InviteGuestUser(Dictionary<string, object> user)
        {
            var result = await SendGraphRequest("/invitations", JsonConvert.SerializeObject(user), HttpMethod.Post);
            return JsonConvert.DeserializeObject<InviteGuestUserOutputModel>(result);
        }

        public async Task UpdateUser(Dictionary<string, object> user, string id)
        {
            await SendGraphRequest($"/users/{id}", JsonConvert.SerializeObject(user), HttpMethod.Patch);
        }

        private async Task<AuthenticationResult> AcquireAccessToken()
        {
            var app = ConfidentialClientApplicationBuilder.Create(graphSettings.ClientId)
                      .WithClientSecret(graphSettings.ClientSecret)
                      .WithAuthority(new Uri(graphSettings.Authority))
                      .Build();
            return await app.AcquireTokenForClient(graphSettings.scopes)
                    .ExecuteAsync();
        }

        private async Task<string> SendGraphRequest(string api, string data, HttpMethod method)
        {
            var authResult = await AcquireAccessToken();
            var accessToken = authResult.AccessToken;

            var url = $"{graphSettings.ApiUrl}{graphSettings.ApiVersion}{api}";

            //Trace.WriteLine($"Graph API call: {url}");
            using (var http = new HttpClient())
            using (var request = new HttpRequestMessage(method, url))
            {
                // Set the authorization header
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // For POST and PATCH set the request content 
                if (!string.IsNullOrEmpty(data))
                {
                    //Trace.WriteLine($"Graph API data: {data}");
                    request.Content = new StringContent(data, Encoding.UTF8, "application/json");
                }

                // Send the request to Graph API endpoint
                using (var response = await http.SendAsync(request))
                {
                    var error = await response.Content.ReadAsStringAsync();

                    // Check the result for error
                    if (!response.IsSuccessStatusCode)
                    {
                        // Throw server busy error message
                        if (response.StatusCode == (HttpStatusCode)429)
                        {
                            // TBD: Add you error handling here
                        }

                        throw new Exception(error);
                    }

                    // Return the response body, usually in JSON format
                    return await response.Content.ReadAsStringAsync();
                }
            }
        }
    }
}
