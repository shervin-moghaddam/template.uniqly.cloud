using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace robotmanden.Models
{
    public class AccountUserModel
    {
        public int Id { get; set; }
        public string UserName = "";
        public int UserID = 0;
        public int LanguageID { get; set; }
        public string FullName = "";
        public string FirstName = "";

        public string Password = "";
        public string PasswordHash = "";
        public bool RememberMe = false;
        public string NormalizedUserName = "";
        public string Email = "";
        public string CompanyName { get; set; }

        public string NormalizedEmail = "";

        public bool EmailConfirmed = false;


        public string PhoneNumber = "";

        public bool PhoneNumberConfirmed = false;

        public bool TwoFactorEnabled = false;
        public int Dev { get; set; }
    }
}