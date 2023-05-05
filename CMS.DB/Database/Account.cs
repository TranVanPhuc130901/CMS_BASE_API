namespace CMS_WT_API.Database
{
    public class Account
    { /// <summary>
      /// id Quản trị viên
      /// </summary>
        public int UserID { get; set; }
        /// <summary>
        /// Tên Tài khoản
        /// </summary>
        public string Username { get; set; } = string.Empty;
        /// <summary>
        /// Email tài khoản
        /// </summary>
        public string Email { get; set; } = string.Empty;
        /// <summary>
        /// Tên đầy đủ tài khoản
        /// </summary>
        public string Fullname { get; set; } = string.Empty;
        /// <summary>
        /// Ngày thêm 
        /// </summary>
        public DateTime Created { get; set; } = DateTime.Now;
        /// <summary>
        /// ngày sửa
        /// </summary>
        public DateTime Modifined { get; set; }
        /// <summary>
        /// mật khẩu đã được mã hoá 1 chiều
        /// </summary>
        public byte[] PasswordHash { get; set; }
        /// <summary>
        /// Mật khẩu được mã hoá cùng với 1 chuỗi + passwordHash
        /// </summary>
        public byte[] PasswordSaft { get; set; }
        /// <summary>
        /// token được refresh khi hết hạn
        /// </summary>
        public string Token { get; set; } = string.Empty;

        public string RefreshToken { get; set; } = string.Empty;

        public DateTime CreatedRefreshToken { get; set; }

        public DateTime ExpiresRefreshToken { get; set; }

        public DateTime CreateToken { get; set; }

        public DateTime ExpiresToken { set; get; }
        public ICollection<UserRole> userRoles { get; set; }

    }
}
