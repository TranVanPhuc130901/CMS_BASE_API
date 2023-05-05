namespace CMS_Common
{
    /// <summary>
    /// Bảng liên kết của danh mục sản phẩm và sản phẩm
    /// </summary>
    public class ProductCategory
    {
        /// <summary>
        /// ID bảng
        /// </summary>
        public int ProductCategoryID { get; set; }
        /// <summary>
        ///  Id mã danh mục sản phẩm
        /// </summary>
        public int CategoryID { get; set; }
        /// <summary>
        /// ID sản phẩms
        /// </summary>
        public int ProductID { get; set; }

        public Category? Category { get; set; }
        public Product? Product { get; set; }
    }
}
