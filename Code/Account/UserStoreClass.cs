using template.SQL;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

using static template.SQL.SQLHelperClass;
using static template.Code.DataTypeConversionHelperClass;
using System.Data;
using System.Security.Principal;
using template.Models;
using template.Services;

namespace template.Code
{
    public class UserStoreClass : IUserStore<AccountUserModel>, IUserPasswordStore<AccountUserModel>
    {
        private readonly string _connectionString;
        private readonly GlobalContainerService _globalcontainer;

        public UserStoreClass(GlobalContainerService globalcontainer, IConfiguration configuration)
        {
            _globalcontainer = globalcontainer;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IdentityResult> CreateAsync(AccountUserModel user, CancellationToken cancellationToken)
        {
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(AccountUserModel user, CancellationToken cancellationToken)
        {
            return IdentityResult.Success;
        }

        public async Task<AccountUserModel> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            // Get email from UserID
            // string Email = cStr(GetScalar($"SELECT TOP 1 EMAIL FROM WU01_LoginIndex WHERE ID={userId}", MasterDBConn));
            //
            // using (var connection = new SqlConnection(GetConnectionString(MasterDBConn)))
            // {
            //     await connection.OpenAsync(cancellationToken);
            //     var parameters = new { Email = Email };
            //     AccountUserModel AccountUser = new AccountUserModel();
            //     AccountUser = await connection.QuerySingleOrDefaultAsync<AccountUserModel>(
            //         $"SELECT TOP 1 * FROM WU01_LoginIndex WHERE Email=@Email", parameters);
            //     AccountUser.UserName = Email;
            //     AccountUser.CompanyName =
            //         cStr(GetScalar(
            //             "select CompanyName from WU01_LoginIndex W01 INNER JOIN MDB01_CustomerTable MDB01 ON W01.CustomerID = MDB01.CustomerID WHERE w01.id = " +
            //             userId, MasterDBConn));
            //
            //     DataTable dtUserData = await GetDataTableAsync("SELECT TOP 1 LanguageID, ID, FirstName FROM U01_Users WHERE Email=@Email",
            //         GetConnForUser(AccountUser.UserName),
            //         CreateParam(Email, "Email"));
            //
            //     if (dtUserData.Rows.Count > 0)
            //     {
            //         DataRow r = dtUserData.Rows[0];
            //         AccountUser.LanguageID = cInt(r["LanguageID"]);
            //         AccountUser.Id = cInt(r["ID"]);
            //         AccountUser.FirstName = cStr(r["FirstName"]);
            //     }
            //     
            //     AccountUser.UserID = AccountUser.Id;
            //     return AccountUser;
            // }
            return null;
        }

        /// <summary>
        /// Login screen will connect via this function
        /// </summary>
        /// <param name="username"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<AccountUserModel> FindByNameAsync(string email, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                var parameters = new { Email = email };
                // Create new instance of a user (might be changed later to existing)
                AccountUserModel mAccountUser = new AccountUserModel();
                // mAccountUser = await connection.QuerySingleOrDefaultAsync<AccountUserModel>(
                //     $@"SELECT WebPassword as PasswordHash, ID FROM U01_Users WHERE Email=@Email", parameters);
                mAccountUser.UserName = email;
                return mAccountUser;
            }

            return null;
        }

        #region dummy methods needed for identity

        public Task<string> GetNormalizedUserNameAsync(AccountUserModel user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedUserName);
        }

        public Task<string> GetUserIdAsync(AccountUserModel user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id.ToString());
        }

        public Task<string> GetUserNameAsync(AccountUserModel user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        public Task SetNormalizedUserNameAsync(AccountUserModel user, string normalizedName,
            CancellationToken cancellationToken)
        {
            user.NormalizedUserName = normalizedName;
            return Task.FromResult(0);
        }

        public Task SetUserNameAsync(AccountUserModel user, string userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;
            return Task.FromResult(0);
        }

        public async Task<IdentityResult> UpdateAsync(AccountUserModel user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return IdentityResult.Success;
        }


        public Task SetPasswordHashAsync(AccountUserModel user, string passwordHash,
            CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;
            return Task.FromResult(0);
        }

        public Task<string> GetPasswordHashAsync(AccountUserModel user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(AccountUserModel user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash != null);
        }

        public void Dispose()
        {
            // Nothing to dispose.
        }

        #endregion
    }
}