using CMS_Common;
using Microsoft.AspNetCore.Mvc;

namespace CMS_BL
{
    public interface IUserAuth
    {
        public Task<Account> Regester(UserModel requet);

        public Task<AuthMessageDto> Login(Account request, LoginModel user);

        public Task<ActionResult<string>> ResetToken();

        public Task<UserRoleModel> SetUsage(int UserID, int RoleID, int newRoleID);

        public Task<List<object>> GetUsage();

        public Task<int> CreateUsage(UserRoleModel role);

        public Task<List<RoleModel>> GetRole();

        public Task<RoleModel> AddRole(RoleModel model);

        public Task<int> DeleteRole(int RoleID);

        public Task<List<Permission>> GetPermissionByRoleID(int RoleID);

        public Task<RolePermissionModel> CreateRolePermissionModel(RolePermissionModel model);

        public Task<int> DeleteRolePermissionModel(int RoleID, int PermissionID);
    }
}
