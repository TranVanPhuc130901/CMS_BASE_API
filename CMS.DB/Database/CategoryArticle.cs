namespace CMS_WT_API.Database
{
    /// <summary>
    /// Bảng danh mục bài viết
    /// </summary>
    public class CategoryArticle
    {
        /// <summary>
        /// Id danh mục bài viết
        /// </summary>
        public int CategoryActicleID { get; set; }
        /// <summary>
        /// Tên danh mục bài viết
        /// </summary>
        public string CategoryName { get; set; } = string.Empty;
    }
}
