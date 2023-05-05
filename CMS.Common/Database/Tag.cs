namespace CMS_Common
{
    /// <summary>
    /// Bảng lưu thông tin từ khóa bài viết
    /// </summary>
    public class Tag
    {
        /// <summary>
        /// ID từ khóa
        /// </summary>
        public int TagID { get; set; }
        /// <summary>
        /// Từ khóa
        /// </summary>
        public string TagName { get; set; }

        public ICollection<TagArticle> ?TagArticles { get; set; }
    }
}
