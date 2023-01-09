using template.Code;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.V5.Pages.Account.Manage.Internal;
using template.Models;
using template.Resources;
using template.Services;
using static template.Code.DataTypeConversionHelperClass;
using static template.SQL.SQLConnectionClass;

using static template.SQL.SQLHelperClass;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace template.Controllers
{
    public class AccountController : Controller
    {
        public AccountController(
            IStringLocalizer<LoginResource> localizer,
            UserManager<AccountUserModel> userManager,
            SignInManager<AccountUserModel> signInManager,
            ProjectSetupClass projectSetup,
            GlobalContainerService GCS)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            GlobalContainer = GCS;
            Localizer = localizer;
            ProjectSetup = projectSetup;
        }
        private readonly ProjectSetupClass ProjectSetup;
        private readonly UserManager<AccountUserModel> UserManager;
        private readonly SignInManager<AccountUserModel> SignInManager;
        private readonly GlobalContainerService GlobalContainer;
        private readonly IStringLocalizer<LoginResource> Localizer;
        protected readonly IHostingEnvironment HostingEnvironment;
        private Task<AccountUserModel> GetCurrentUserAsync() => UserManager.GetUserAsync(HttpContext.User);

        [HttpGet]
        public async Task<int> GetCurrentUserId()
        {
            AccountUserModel usr = await GetCurrentUserAsync();
            return usr.Id;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null) // Showing the login view
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ViewData["ReturnUrl"] = returnUrl;
            LoginViewModel m = new LoginViewModel();

            return View(m);
        }
        
                [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                // Some users with the flag "OrderFormUser" in Roles, is only allowed to access the order form
                int OrderFormUser = cInt(GetScalar(
                    "SELECT Users_OrderFormUser FROM vUsers WHERE Users_UserName=@UserName",
                    CreateParam(model.UserName, "UserName")));

                if (!returnUrl.ToLower().Contains("forhandler") && OrderFormUser == 1)
                {
                    //returnUrl = "";
                    //ViewData["ReturnUrl"] = returnUrl;

                    // Deny access
                    ModelState.AddModelError(string.Empty,
                        "Denne bruger har kun adgang til forhandler bestillings form.");
                    return View(model);
                }

                ;

                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, false,
                    lockoutOnFailure: false);


                if (result.Succeeded)
                {
                    // Log
                    string UserIP = HttpContext.Connection.RemoteIpAddress?.ToString();
                    //var feature = HttpContext.Features.Get<IHttpConnectionFeature>();
                    //UserIP = feature?.LocalIpAddress?.ToString();

                    try
                    {
                        SecurityLog($"Login for user name ({model.UserName})", $"Ip address:{UserIP}", 0, 0,
                            "user_login");
                    }
                    catch (Exception ex)
                    {
                    }

                    // Stores user ID into global values
                    globalcontainer.StoreUser(model.UserName);
                    if (returnUrl.ToLower().Contains("home/index"))
                        return RedirectToAction(nameof(HomeController.Index), "Home",
                            new { WebDarkmode = "", model.UserName });
                    else
                        return RedirectToLocal(returnUrl);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "De indtastede initialer eller kodeord er forkert");
                    return View(model);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult>
            Login(LoginViewModel model, string returnUrl = null) // Login validation and submit
        {
            return View(model);
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ChangePasswordPartial()
        {
            return PartialView();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPasswordPartial(string UserName)
        {
            ResetPasswordModel model = new ResetPasswordModel();

            if (!string.IsNullOrEmpty(UserName))
            {
                
                model.UserName = UserName;
            }

            model.InfoText = Localizer["ResetPassword_InfoText"];
            return PartialView(model);
        }


        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            return View(model);
        }

       
        public async Task<IActionResult> ResetPasswordCodePartial(ResetPasswordCodeModel model)
        {
            model.InfoText = Localizer["ResetPasswordCode_InfoText"];
            return PartialView(model);
        }


        [HttpPost]
        //  [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await SignInManager.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        public Task SetPasswordHashAsync(AccountUserModel user, string passwordHash,
            CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;
            return Task.FromResult(0);
        }


        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        #region Notused

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> LoginWith2fa(bool rememberMe, string returnUrl = null)
        {
            return null;
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginWith2fa(LoginWith2faViewModel model, bool rememberMe,
            string returnUrl = null)
        {
            return null;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> LoginWithRecoveryCode(string returnUrl = null)
        {
            return null;
            ;
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginWithRecoveryCode(LoginWithRecoveryCodeViewModel model,
            string returnUrl = null)
        {
            return null;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Lockout()
        {
            return null;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            return null;
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            return null;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string code = null)
        {
            return null;
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            return null;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return null;
        }


        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        #endregion

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        #endregion
    }
}