using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace robotmanden.Models
{
    public class ResetPasswordModel
    {
        [EmailAddress(ErrorMessage = "NotValidEmail")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "LoginValidationError_NoUsername")]
        public string UserName { get; set; }
        public int ValidationCode { get; set; }
        public string ErrorMessage { get; set; }
        public string InfoText { get; set; }
        public string UserFirstname { get; set; }
    }
}
