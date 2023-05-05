namespace CMS_WT_API.Database
{
    /// <summary>
    /// bảng liên kết bình luận với bài viết và người dùng
    /// </summary>
    public class CommentUserArticle
    {
        /// <summary>
        /// ID bình luận
        /// </summary>
        public int CommentID { get; set; }
        /// <summary>
        /// ID người dùng
        /// </summary>
        public int UserID { get; set; }
        /// <summary>
        /// ID bài viết
        /// </summary>
        public int ArticleID { get; set; }
    }
}
