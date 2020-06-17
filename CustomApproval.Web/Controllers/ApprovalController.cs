using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomApproval.Web.Entities;
using CustomApproval.Web.Extensions;
using CustomApproval.Web.Models;
using CustomApproval.Web.Models.GraphApi;
using CustomApproval.Web.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace CustomApproval.Web.Controllers
{
    public class ApprovalController : Controller
    {

        private readonly IUserService userService;
        private readonly IGraphClientService graphClientService;
        private readonly IMailService mailService;
        private readonly GraphSettings graphSettings;
        private readonly AppSettings appSettings;

        public ApprovalController(IConfiguration config, IUserService userService, IGraphClientService graphClientService, IMailService mailService)
        {
            this.userService = userService;
            this.graphClientService = graphClientService;
            this.mailService = mailService;
            graphSettings = config.GetSection("GraphApi")
              .Get<GraphSettings>();
            appSettings = config.GetSection("AppSettings")
              .Get<AppSettings>();
        }

        // GET: Approval
        public ActionResult Index()
        {

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Details(FindRequestModel data)
        {
            ViewBag.Email = data.Email;

            var users = await userService.GetUsersByEmail(data.Email);

            return View(users.ToUserModel());
        }

        [HttpPost]
        public async Task<ActionResult> ApproveRequest(string Id)
        {
            var user = await userService.GetUsersById(Id);

            ViewBag.Message = "An error occurred while updating the request.";

            if (user == null)
            {
                return View("Index");
            }

            if (user.IsSocialUser())
            {
                await graphClientService.CreateUser(user.ToSocialUserInput(graphSettings.Tenant));
                await mailService.SendApprovalNotification(user.Email, user.Locale);
            }
            else 
            {
                var result = await graphClientService.InviteGuestUser(user.ToInviteGuestUserInput(appSettings.ParentAppRedirectUrl));

                if (result == null || string.IsNullOrEmpty(result.invitedUser?.id))
                {
                    return View("Index");
                }

                await graphClientService.UpdateUser(user.ToUpdateGuestUserInput(), result.invitedUser.id);
            }

            await userService.RemoveUsersById(Id);

            ViewBag.Message = "Update was successful.";
            return View("Index");
        }

        [HttpPost]
        public async Task<ActionResult> DenyRequest(string Id)
        {
            await userService.UpdateUserStatusAsync(Id, Constants.UserApprovalStatus.Denied);

            ViewBag.Message = "Update was successful.";

            return View("Index");
        }
    }
}