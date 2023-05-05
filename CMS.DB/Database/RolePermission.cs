namespace CMS_WT_API.Database
{
    public class RolePermission
    {
        /// <summary>
        /// ID nhóm quyền truy cập
        /// </summary>
        public int RoleID { get; set; }

        /// <summary>
        /// ID quyền truy cập
        /// </summary>
        public int PermissionID { get; set; }


        public Role Role { get; set; }

        public Permission Permission { get; set; }
    }
}
