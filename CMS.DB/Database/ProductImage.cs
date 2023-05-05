using CMS_WT_API.Enums;

namespace CMS_DB
{
    public class ProductImage
    {
        /// <summary>
        /// ID ảnh sản phẩm
        /// </summary>
        public int ProductImageID { get; set; }
        /// Đường dẫn ảnh
        /// </summary>
        public string ProductImageSlug { get; set; }
        /// <summary>
        /// Trạng thái ảnh (1 là ảnh gốc, 0 là ảnh phụ)
        /// </summary>
        public StatusImage IsDefault { get; set; }

        public int ProductID { get; set; }

        public Product Product { get; set; }
    }
}
