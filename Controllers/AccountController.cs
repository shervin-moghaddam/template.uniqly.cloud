using template.Code;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Identity.UI.V5.Pages.Account.Manage.Internal;
using template.Models;
using template.Resources;
using template.Services;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace template.Controllers
{
    public class AccountController : Controller
    {
        public AccountController(
            IStringLocalizer<LoginResource> localizer,
            UserManager<AccountUserModel> userManager,
            SignInManager<AccountUserModel> signInManager,
            LogService _Logger,
            ProjectSetupClass projectSetup,
            GlobalContainerService GCS)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            GlobalContainer = GCS;
            Localizer = localizer;
            ProjectSetup = projectSetup;
            Logger = _Logger;
        }

        private readonly ProjectSetupClass ProjectSetup;
        private readonly UserManager<AccountUserModel> UserManager;
        private readonly SignInManager<AccountUserModel> SignInManager;
        private readonly GlobalContainerService GlobalContainer;
        private readonly IStringLocalizer<LoginResource> Localizer;
        protected readonly IHostingEnvironment HostingEnvironment;
        private readonly LogService Logger;

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
            return View(new LoginViewModel());
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            string UserIP = HttpContext.Connection.RemoteIpAddress?.ToString();
            
            if (ModelState.IsValid)
            {
                SignInResult result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, false,
                    lockoutOnFailure: false);
               

                if (result.Succeeded)
                {
                    Logger.SecurityLog(new SecurityLogModel
                    {
                        Identifier = "login",
                        Text1 = $"Login for user name ({model.UserName})",
                        IPAddress = UserIP,
                        StatusCode = (int)SecurityLogStatusCode.Login_Success,
                        Type = (int)LogType.Default,
                        UserID = GetCurrentUserId().Result
                    });

                    // Stores user ID into global values
                    GlobalContainer.StoreUser(model.UserName);
                    if (returnUrl.ToLower().Contains("home/index"))
                        return RedirectToAction(nameof(HomeController.Index), "Home",
                            new { WebDarkmode = "", model.UserName });
                    else
                        return RedirectToLocal(returnUrl);
                }

                // If no unsuccessful login, then show error message
                ModelState.AddModelError(string.Empty, Localizer["IncorrectUsernamePassword"].Value);
                Logger.SecurityLog(new SecurityLogModel
                {
                    Identifier = "login",
                    Text1 = $"Login for user name ({model.UserName})",
                    IPAddress = UserIP,
                    StatusCode = (int)SecurityLogStatusCode.Login_Failed,
                    Type = (int)LogType.Default
                });
                
                return View(model);
            }

            
            // If we got this far, something failed, log it and redisplay form
            var x = Localizer["IncorrectUsernamePassword"].Value;
            if (string.IsNullOrEmpty(model.UserName) || string.IsNullOrEmpty(model.Password))
                ModelState.AddModelError(string.Empty, Localizer["NoUserPasswordInput"].Value);
            else
                ModelState.AddModelError(string.Empty, Localizer["IncorrectUsernamePassword"].Value);

            // If no unsuccessful login, then show error message
            ModelState.AddModelError(string.Empty, Localizer["IncorrectUsernamePassword"].Value);
            Logger.SecurityLog(new SecurityLogModel
            {
                Identifier = "login",
                Text1 = $"Login for user name ({model.UserName})",
                IPAddress = UserIP,
                StatusCode = (int)SecurityLogStatusCode.Login_Invalid,
                Type = (int)LogType.Default
            });
            
            return View(model);
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ChangePasswordPartial()
        {
            //   return PartialView();
            return null;
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
            //return PartialView(model);
            return null;
        }


        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            //return View(model);
            return null;
        }


        public async Task<IActionResult> ResetPasswordCodePartial(ResetPasswordCodeModel model)
        {
            model.InfoText = Localizer["ResetPasswordCode_InfoText"];
            // return PartialView(model);
            return null;
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
            //return View();
            return null;
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