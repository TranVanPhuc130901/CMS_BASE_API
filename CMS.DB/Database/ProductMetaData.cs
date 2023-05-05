namespace CMS_DB
{
    /// <summary>
    /// Bảng Seo sản phẩm
    /// </summary>
    public class ProductMetaData
    {
        /// <summary>
        ///  Id Seo sản phẩm
        /// </summary>
        public int ProductMetaDataID { get; set; }
        /// <summary>
        /// Tiêu đề Seo
        /// </summary>
        public string ProductMetaDataTitle { get; set;}
        /// <summary>
        /// Mô tả
        /// </summary>
        public string ProductMetadataDescrition { get; set; }
        /// <summary>
        /// Ngày tạo
        /// </summary>
        public DateTime CreatedDate { get; set; }
        /// <summary>
        /// Ngày sửa
        /// </summary>
        public DateTime ModifiedDate { get; set; }
        /// <summary>
        /// MÃ sản phẩm
        /// </summary>
        public int ProductID { get; set; }

        public Product Product { get; set; }

    }
}
