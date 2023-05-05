namespace CMS_WT_API.Database
{
    /// <summary>
    /// Bảng liên kết giữ từ khóa(Tag) và Article(bài viết)
    /// </summary>
    public class TagArticle
    {
        /// <summary>
        /// ID Từ khóa bài viết
        /// </summary>
        public int TagID { get; set; }
        /// <summary>
        /// ID bài viết
        /// </summary>
        public int ArticleID { get; set; }
    }
}
