using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace template.Models
{
    public class ResetPasswordCodeModel
    {
        public string UserName { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "FieldRequired")]
        public string ValidationCode { get; set; }
        public string ErrorMessage { get; set; }
        public string InfoText { get; set; }
        public string UserFirstname { get; set; }
    }
}
