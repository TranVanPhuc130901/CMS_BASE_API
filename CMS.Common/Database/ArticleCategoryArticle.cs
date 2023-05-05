namespace CMS_Common
{
    /// <summary>
    ///  bảng liên kết bài viết(Article) và danh mục bài viết(CategoryArticle)
    /// </summary>
    public class ArticleCategoryArticle
    {
        /// <summary>
        /// ID bài viết
        /// </summary>
        public int ArticleID { get; set; }
        /// <summary>
        /// ID danh mcuj bài viết
        /// </summary>
        public int CategoryArticleID { get; set; }

        public Article ?Articles { get; set; }

        public CategoryArticle ?CategoryArticles { get; set; }
    }
}
