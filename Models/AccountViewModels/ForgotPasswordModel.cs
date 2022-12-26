using robotmanden.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace robotmanden.Models
{
    [AllowAnonymous]
    public class ForgotPasswordModel
    {
        private readonly SignInManager<AccountUserModel> _signInManager;
        private readonly ILogger<LoginViewModel> _logger;
        private readonly IStringLocalizer<CommonResource> _sharedLocalizer;

        public ForgotPasswordModel(
            SignInManager<AccountUserModel> signInManager,
            ILogger<LoginViewModel> logger,
            IStringLocalizer<CommonResource> sharedLocalizer)
        {
            _signInManager = signInManager;
            _logger = logger;
            _sharedLocalizer = sharedLocalizer;
        }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "PasswordValidationError_StringLength", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "PasswordValidationError_DoesNotMatch")]
        public string ConfirmPassword { get; set; }

        public string ErrorMessage { get; set; }
    }
}
