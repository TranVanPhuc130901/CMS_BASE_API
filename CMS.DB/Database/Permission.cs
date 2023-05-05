namespace CMS_WT_API.Database
{
    public class Permission
    {
        /// <summary>
        /// Mã quyền quản trị
        /// </summary>
        public int PermissionID { get; set; }

        /// <summary>
        /// Tên quyền quản trị
        /// </summary>
        public string PermissionsName { get; set; }

        public string PermissionDesc { get; set; }

        public ICollection<RolePermission> RolePermission { get; set; }
    }
}
