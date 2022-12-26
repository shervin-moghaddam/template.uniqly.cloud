using robotmanden.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// CAPNORDIC WEB DOES NOT USE ROLES AS WE FOLLOW A06 TABLES
/// </summary>
namespace robotmanden.Code
{

    public class RoleStoreClass : IRoleStore<AccountRoleModel>
    {
        public Task<IdentityResult> CreateAsync(AccountRoleModel role, CancellationToken cancellationToken)
        {
            return null;
        }

        public Task<IdentityResult> DeleteAsync(AccountRoleModel role, CancellationToken cancellationToken)
        {
            return null;
        }

        public void Dispose()
        {
        
        }

        public Task<AccountRoleModel> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            return null;
        }

        public Task<AccountRoleModel> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            return null;
        }

        public Task<string> GetNormalizedRoleNameAsync(AccountRoleModel role, CancellationToken cancellationToken)
        {
            return null;
        }

        public Task<string> GetRoleIdAsync(AccountRoleModel role, CancellationToken cancellationToken)
        {
            return null;
        }

        public Task<string> GetRoleNameAsync(AccountRoleModel role, CancellationToken cancellationToken)
        {
            return null;
        }

        public Task SetNormalizedRoleNameAsync(AccountRoleModel role, string normalizedName, CancellationToken cancellationToken)
        {
            return null;
        }

        public Task SetRoleNameAsync(AccountRoleModel role, string roleName, CancellationToken cancellationToken)
        {
            return null;
        }

        public Task<IdentityResult> UpdateAsync(AccountRoleModel role, CancellationToken cancellationToken)
        {
            return null;
        }
    }
}
