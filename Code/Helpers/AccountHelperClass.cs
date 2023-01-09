using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using static template.SQL.SQLHelperClass;
using static template.Code.DataTypeConversionHelperClass;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Data;
using template.Models;

namespace template.Code
{
    public class AccountHelperClass
    {
   
      
        public static void GetUserPassword(string Email)
        {
           
        }

        public static string HashPassword(AccountUserModel mAccountUser, string Password)
        {
            PasswordHasher<AccountUserModel> hasher = new PasswordHasher<AccountUserModel>(
                new OptionsWrapper<PasswordHasherOptions>(
                    new PasswordHasherOptions()
                    {
                        CompatibilityMode = PasswordHasherCompatibilityMode.IdentityV3
                    })
            );
            string HashedPassword = hasher.HashPassword(mAccountUser, Password);

            return HashedPassword;
        }
    }
}
