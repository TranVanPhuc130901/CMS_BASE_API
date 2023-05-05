namespace CMS_DB
{
    /// <summary>
    /// Bảng giá tiền sản phẩm
    /// </summary>
    public class ProductPrice
    {
        /// <summary>
        /// Id giá sản phẩm
        /// </summary>
        public int ProductPriceID { get; set; }
        /// <summary>
        /// Giá gốc sản phẩm
        /// </summary>
        public decimal ProductCost { get; set; }
        /// <summary>
        /// Giá khuyến mãi sản phẩm
        /// </summary>
        public decimal ProductPromotional { get; set; }
        /// <summary>
        /// Mã sản phẩm
        /// </summary>
        public int ProductID { get; set; }

        public Product Product { get; set; }


    }
}
