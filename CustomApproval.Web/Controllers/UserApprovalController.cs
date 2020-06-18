using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CustomApproval.Authentication;
using CustomApproval.Web.Entities;
using CustomApproval.Web.Models;
using CustomApproval.Web.Models.GraphApi;
using CustomApproval.Web.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CustomApproval.Web.Controllers
{
    [ApiController]
    [Route("api/approvals/[action]")]
    public class UserApprovalController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserApprovalController(IUserService userService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        [BasicAuth]
        [ActionName("checkstatus")]
        [HttpPost]
        public async Task<IActionResult> CheckApprovalStatus([FromBody] CheckApprovalStatusInputModel inputClaims)
        {
            if (inputClaims == null)
            {
                return Ok(new ResponseContent("ValidationFailed", "Can not deserialize input claims.", HttpStatusCode.BadRequest, action: "ValidationError"));
            }

            if (string.IsNullOrEmpty(inputClaims.email))
            {
                return Ok(new ResponseContent("EmailEmpty", "The value entered for email is invalid", HttpStatusCode.BadRequest, action: "ValidationError"));
            }

            var user = await _userService.GetUserByEmailAndIdentityIssuersAsync(inputClaims.email, inputClaims.identities?.Select(x => x.issuer).ToList());

            if (user == null || user.ApprovalStatus == Constants.UserApprovalStatus.Approved)
            {
                return Ok(new ResponseContent(string.Empty, string.Empty, HttpStatusCode.OK, action: "Allow"));
            }

            if (user.ApprovalStatus == Constants.UserApprovalStatus.Denied)
            {
                return Ok(new ResponseContent("ApprovalDenied", "Your request to sign up has been denied by the administrator. Please reach out to admins@contoso.com in case of any queries.", HttpStatusCode.BadRequest, action: "ShowBlockPage"));
            }

            return Ok(new ResponseContent("ApprovalPending", "Your account is pending the approval of an administrator. Please reach out to admins@contoso.com in case it has been pending for more than 3 business days.", HttpStatusCode.OK, action: "ShowBlockPage"));

        }

        [BasicAuth]
        [HttpPost]
        [ActionName("submit")]
        public async Task<IActionResult> RegisterForApproval()
        {
            RegisterForApprovalInputModel inputClaims;
            using (var reader = new StreamReader(Request.Body))
            {
                var body = await reader.ReadToEndAsync();
                inputClaims = JsonConvert.DeserializeObject<RegisterForApprovalInputModel>(body);
                inputClaims.inputData = body;
            }

            if (inputClaims == null)
            {
                return BadRequest(new ResponseContent("ValidationFailed", "Can not deserialize input claims.", HttpStatusCode.BadRequest, action: "ValidationError"));
            }

            if (string.IsNullOrEmpty(inputClaims.email))
            {
                return BadRequest(new ResponseContent("EmailEmpty", "The value entered for email is invalid", HttpStatusCode.BadRequest, action: "ValidationError"));
            }

            if (string.IsNullOrEmpty(inputClaims.displayName))
            {
                return BadRequest(new ResponseContent("DisplayNameEmpty", "The value entered for display name is invalid", HttpStatusCode.BadRequest, action: "ValidationError"));
            }

            if (await _userService.IsUserExistAsync(inputClaims.email, inputClaims.identities?.Select(x => x.issuer).ToList()))
            {
                return Ok(new ResponseContent("ApprovalPending", "Your request to sign up has been submitted for approval. Please check back after some time.", HttpStatusCode.OK, action: "ShowBlockPage"));
            }

            await _userService.CreateUserAsync(inputClaims.CreateUserData());

            return Ok(new ResponseContent("ApprovalPending", "Your request to sign up has been submitted for approval. Please check back after some time.", HttpStatusCode.OK, action: "ShowBlockPage"));
        }

    }
}