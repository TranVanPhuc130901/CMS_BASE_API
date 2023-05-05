using Microsoft.AspNetCore.Authorization;

namespace CMS_BASE_API.Authorization
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PermissionAuthorizationHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            // Tìm kiếm Claim với tên là "Permission" trong User của request hiện tại
            var permissionClaim = context.User.FindAll(c => c.Type == "Permission");
            if (permissionClaim == null)
            {
                return Task.CompletedTask;
            }

            // Lấy các quyền từ chuỗi quyền được lưu trong claim "Permission"
            var permissions = permissionClaim.SelectMany(c => c.Value.Split(';')).ToList();


            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
            {
                return Task.CompletedTask;
            }

            // Lấy HTTP Method của request hiện tại
            var method = httpContext.Request.Method;

            // Nếu người dùng có quyền "view" và phương thức là GET, đánh dấu thành công cho yêu cầu
            if (permissions.Contains("view") && (method == HttpMethods.Get))
            {
                context.Succeed(requirement);
            }

            // Nếu người dùng có quyền "add" và phương thức là POST, đánh dấu thành công cho yêu cầu
            if (permissions.Contains("add") && (method == HttpMethods.Post))
            {
                context.Succeed(requirement);
            }

            // Nếu người dùng có quyền "edit" và phương thức là PUT, đánh dấu thành công cho yêu cầu
            if (permissions.Contains("edit") && (method == HttpMethods.Put))
            {
                context.Succeed(requirement);
            }

            // Nếu người dùng có quyền "delete" và phương thức là DELETE, đánh dấu thành công cho yêu cầu
            if (permissions.Contains("delete") && (method == HttpMethods.Delete))
            {
                context.Succeed(requirement);
            }

            // Hoàn thành task và trả về kết quả
            return Task.CompletedTask;
        }
    }
}
