namespace CMS_Common
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
        public string? RoleName { get; set; }
        /// <summary>
        /// Mô tả nhóm quyền quản trị
        /// </summary>
        public string? RoleDescription { get; set; }

        public ICollection<UserRole>? UserRoles { get; set; }

        public ICollection<RolePermission>? RolePermission { get; set; }
    }
}
