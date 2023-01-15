using System.ComponentModel.DataAnnotations;

namespace template.Models
{
    public class LoginViewModel
    {
      //  [EmailAddress(ErrorMessage = "NotValidEmail")]
       // [Required(AllowEmptyStrings = false, ErrorMessage = "LoginValidationError_NoUsername")]
        public string UserName { get; set; }

        //[Required(AllowEmptyStrings = false, ErrorMessage = "LoginValidationError_NoPassword")]
        //[DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me")]
        public bool RememberMe { get { return false; } init { RememberMe = value; } }

        public string ErrorMessage { get; set; }
        public string InfoText { get; set; }
        public string WebDarkmode { get; set; }
        
        // Project setup
        public string WebsiteName { get; set; }
    }
}
