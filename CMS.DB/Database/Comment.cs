namespace CMS_WT_API.Database
{
    /// <summary>
    /// Bình luận của người dùng
    /// </summary>
    public class Comment
    {
        /// <summary>
        /// ID Bình luận
        /// </summary>
        public int CommentID { get; set; }
        /// <summary>
        /// Chi tiết Bình luận
        /// </summary>
        public int Content { get; set; }
        /// <summary>
        /// Ngày tạo bình luận
        /// </summary>
        public int CreateDate { get; set; }
    }
}
