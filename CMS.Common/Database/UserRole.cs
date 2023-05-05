namespace CMS_Common
{
    public class UserRole
    {
        /// <summary>
        /// ID quản trị viên
        /// </summary>
        public int UserID { get; set; }
        /// <summary>
        /// ID quyền quản trị
        /// </summary>
        public int RoleID { get; set; }

        public Account? User { get; set; }

        public Role? Role { get; set; }
    }
}
