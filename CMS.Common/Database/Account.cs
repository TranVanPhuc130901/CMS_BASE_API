namespace CMS_Common
{
    public class Account
    {
        /// <summary>
        /// id User
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
        public byte[]? PasswordHash { get; set; }
        /// <summary>
        /// Mật khẩu được mã hoá cùng với 1 chuỗi + passwordHash
        /// </summary>
        public byte[]? PasswordSaft { get; set; }
        /// <summary>
        /// token Khi đăng nhập
        /// </summary>
        public string Token { get; set; } = string.Empty;
        /// <summary>
        /// RefreshToken khi đăng nhập dùng để cáp lại Token
        /// </summary>
        public string RefreshToken { get; set; } = string.Empty;
        /// <summary>
        /// Ngày tạo RefreshToken
        /// </summary>
        public DateTime CreatedRefreshToken { get; set; }
        /// <summary>
        /// Thời gian hết hạn RefreshToken
        /// </summary>
        public DateTime ExpiresRefreshToken { get; set; }
        /// <summary>
        /// Thời gian tạo Token
        /// </summary>
        public DateTime CreateToken { get; set; }
        /// <summary>
        /// Thời gian RefreshToken hết hạn
        /// </summary>
        public DateTime ExpiresToken { set; get; }
        public ICollection<UserRole> ?userRoles { get; set; }

        public ICollection<Comment> ?Comments { get; set; }

    }
}
