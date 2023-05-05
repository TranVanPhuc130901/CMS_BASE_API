namespace CMS_Common
{
    /// <summary>
    /// Bảng danh mục sản phẩm
    /// </summary>
    public class Category
    {
        /// <summary>
        /// Mã danh mục sản phẩm
        /// </summary>
        public int CategoryID { get; set; }
        /// <summary>
        /// Tên danh mục sản phẩm
        /// </summary>
        public string ?CategoryName { get; set; }

        public ICollection<ProductCategory> ProductCategories { get; set;}

        public Category()
        {
        
            ProductCategories = new List<ProductCategory>();
        }
    }
}
