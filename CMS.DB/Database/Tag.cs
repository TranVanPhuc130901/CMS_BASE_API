namespace CMS_WT_API.Database
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
        public int TagName { get; set; }
    }
}
