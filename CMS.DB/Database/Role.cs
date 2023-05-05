namespace CMS_WT_API.Database
{
    public class Role
    {
        /// <summary>
        /// ID nhóm quyên quản trị
        /// </summary>
        public int RoleID { get; set; }

        /// <summary>
        /// Tên nhóm quyền quản trị
        /// </summary>
        public string RoleName { get; set; }

        public string RoleDescription { get; set; }

        public ICollection<UserRole> UserRoles { get; set; }

        public ICollection<RolePermission> RolePermission { get; set; }
    }
}
