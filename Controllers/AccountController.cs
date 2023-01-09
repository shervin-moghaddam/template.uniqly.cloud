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

        public async Task<JsonResult> UniqlyLogin()
        {
            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return Json(Localizer["Login_LoginNotFound"]);
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

        // This code works, but needs alterations to how it sends email!
        //[HttpPost]
        /*public async Task<IActionResult> SendResetValidationCode(ResetPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                // 1. Find the server setup for the customerID, the user is connected to
                string[] UserConn = GetConnForUser(model.UserName);
                string UserConnectionString = GetConnectionString(UserConn);

                // 2. Check if the connection string works
                SqlConnection SQLC;
                try
                {
                    SQLC = SQLConnect();
                }
                catch (Exception ex)
                {
                    // On error, return to login screen. The users probably didn't exist, but for 
                    // security reasons we don't tell the user that, to prevent a bot from keep trying
                    // different emails until it findes one.
                    if (HostingEnvironment.IsDevelopment())
                    { model.ErrorMessage = $"ConnectionString:{UserConnectionString} - {ex.Message}"; }
                    else { model.ErrorMessage = Localizer["ConnectionString_Error"]; }
                    return PartialView("ResetPasswordCodePartial");
                }

                // Get user data
                DataTable dtUser = GetDataTable($"SELECT TOP 1 Active, GUID, WebPassword, FirstName, CultureIdentifier FROM vwU01_GetLoginUserInfo WHERE Email='{model.UserName}'",
                    UserConn);

                if (dtUser.Rows.Count > 0)
                {
                    DataRow r = dtUser.Rows[0];
                    string CultureIdentifier = cStr(r["CultureIdentifier"]);
                    string UserFirstname = cStr(r["FirstName"]);

                    // Change culture to match users settings
                    Response.Cookies.Append(
              CookieRequestCultureProvider.DefaultCookieName,
              CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(CultureIdentifier)),
              new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
          );
                    CultureInfo.CurrentCulture = new CultureInfo(CultureIdentifier);
                    model.UserFirstname = UserFirstname;
                }

                // Create a code (available for 15 minutes)
                Tuple<SqlCommand, DataTable> SQLReturnValues; // Item1 is the command, Item2 is the return select
                SQLReturnValues = ExecStoredProcedureReturnDTAndExec("spGenerateValidationCode",
                   MasterDBConn,
                   CreateParam(model.UserName, "Email"),
                   CreateParam(DateTime.UtcNow.AddMinutes(15), "ExpireDateTime"),
                   CreateReturnParam("", "GeneratedCode", 6));

                string ValidationCode = cStr(SQLReturnValues.Item1.Parameters["GeneratedCode"].Value);

                // EMAIL Message
                string Message = Localizer["ResetPasswordMail_Message", model.UserFirstname, ValidationCode];

                // Create mail body and insert message
                string mailBody = "<html><head><style>img {width: 200px;}body {padding: 20px;font-family: Calibri;}h1 {font-size: 18px; padding: 0px;}</style><meta charset='utf-8'></head><body><p>{MESSAGE}</p><h1><strong>template Support Team</strong><br></h1><img src='https://www.dropbox.com/s/fqhhpihvq4rliwz/Logo_500x195.jpg?raw=1'></body></html>";
                mailBody = mailBody.Replace("{MESSAGE}", Message);
                MimeMessage messageToSend = new MimeMessage
                {
                    Sender = new MailboxAddress("template support", "noreply@template.dk"),
                    Subject = Localizer["ResetPasswordMail_Subject"]
                };

                messageToSend.Body = new TextPart(TextFormat.Html) { Text = mailBody };
                messageToSend.To.Add(new MailboxAddress(model.UserName));

                var client = new SmtpClient();
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                client.Connect("smtp.WebSiteLive.net", 465, SecureSocketOptions.Auto);
                client.Authenticate("noreply@template.dk", "Moghaddam169#");
                try
                {
                    client.Send(messageToSend);
                }
                catch (Exception ex)
                {
                    return NotFound("");
                }
                client.Disconnect(true);

                ResetPasswordCodeModel m = new ResetPasswordCodeModel();
                m.UserName = model.UserName;
                m.UserFirstname = model.UserFirstname;
                m.InfoText = Localizer["ResetPasswordCode_InfoText"];
                return PartialView("ResetPasswordCodePartial", m);
            }
            else
            {
                return PartialView("ResetPasswordCodePartial", model);
            }
        }*/

       
        public async Task<IActionResult> ResetPasswordCodePartial(ResetPasswordCodeModel model)
        {
            model.InfoText = Localizer["ResetPasswordCode_InfoText"];
            return PartialView(model);
        }

        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> Logout()
        // {
        //     await SignInManager.SignOutAsync();
        //     Debug.WriteLine("Logged out");
        //
        //     return RedirectToAction(nameof(HomeController.Index), "Home");
        // }

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