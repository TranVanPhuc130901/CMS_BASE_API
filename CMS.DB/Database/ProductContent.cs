namespace CMS_DB
{
    /// <summary>
    ///  Bảng content sản phẩm
    /// </summary>
    public class ProductContent
    {
        /// <summary>
        /// Id content sản phẩm
        /// </summary>
        public int ProductContentID { get; set; }
        /// <summary>
        /// content sản phẩm
        /// </summary>
        public string ProductContentName { get; set; }
        /// <summary>
        /// mã sản phẩm
        /// </summary>
        public int ProductID { get; set; }

        public Product Product { get; set; }
    }
}
