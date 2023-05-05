namespace CMS_Common
{
    /// <summary>
    /// bảng liên kết bình luận với bài viết và người dùng
    /// </summary>
    public class Comment
    {
        /// <summary>
        /// ID bình luận
        /// </summary>
        public int CommentID { get; set; }
        /// <summary>
        /// Mô tả Comment
        /// </summary>
        public string ?Content { get; set; }
        /// <summary>
        /// Thời gian commnet
        /// </summary>
        public DateTime CreateAt { get; set; } = DateTime.Now;
        /// <summary>
        /// ID người dùng
        /// </summary>
        public int UserID { get; set; }
        /// <summary>
        /// ID bài viết
        /// </summary>
        public int ArticleID { get; set; }

        public Account ?Accounts { get; set; }

        public Article ?Articles { get; set; }
    }
}
