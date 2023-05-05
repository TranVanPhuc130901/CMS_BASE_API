namespace CMS_WT_API.Database
{
    /// <summary>
    /// Bảng thông tin bài viết
    /// </summary>
    public class Article
    {
        /// <summary>
        /// ID bài viết
        /// </summary>
        public int ArticleID { get; set; }
        /// <summary>
        /// Tiêu đề bài viết
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Nội dung bài viết
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// Chi tiết bài viết
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Đường dẫn ảnh
        /// </summary>
        public string Image { get; set; }
        /// <summary>
        /// Tác giả
        /// </summary>
        public string Author { get; set; }
        /// <summary>
        /// Thời gian tạo bài viết
        /// </summary>
        public DateTime CreateAt { get; set; } = DateTime.Now;
        /// <summary>
        /// Thời gian sửa bài viết
        /// </summary>
        public DateTime UpdateAt { get; set; }
    }
}
