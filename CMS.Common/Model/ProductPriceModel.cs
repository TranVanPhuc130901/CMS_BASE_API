namespace CMS_Common
{
    public class ProductPriceModel
    {
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
    }
}
