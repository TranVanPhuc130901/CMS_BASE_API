using Microsoft.AspNetCore.Authorization;

namespace CMS_BASE_API.Authorization
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public string[] Permissions { get; set; }

        public PermissionRequirement(params string[] permissions)
        {
            Permissions = permissions;
        }
    }
}
