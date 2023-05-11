using CMS_WT_API.Enums;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace CMS_Common
{
    public class ProductModel
    {
        /// <summary>
        /// ID sản phẩm
        /// </summary>
        [Key]
        //[SwaggerSchema(ReadOnly = true)]
        public int ProductID { get; set; }

        /// <summary>
        /// Mã Sản phẩm
        /// </summary>
        public string? ProductCode { get; set; }
        /// <summary>
        /// Tên sản phẩm
        /// </summary>
        public string? ProductName { get; set; }
        /// <summary>
        /// Mô tả ngắn về sản phẩm
        /// </summary>
        public string? ProductDescription { get; set; }
        /// <summary>
        /// Ngày tạo
        /// </summary>
        public DateTime CreatedDate { get; set; }
        /// <summary>
        /// Ngày sửa
        /// </summary>
        public DateTime ModifinedDate { get; set; }
        /// <summary>
        /// Trạng thái của sản phẩm
        /// </summary>
        public ProductStatus ProductStatus { get; set; }

        public string? ProductImageSlug { get; set; }

        public decimal ProductCost { get; set; }
        /// <summary>
        /// Giá khuyến mãi sản phẩm
        /// </summary>
        public decimal ProductPromotional { get; set; }

        public string? ProductContentName { get; set; }

        public string? ProductMetaDataTitle { get; set; }
        /// <summary>
        /// Mô tả
        /// </summary>
        public string? ProductMetadataDescrition { get; set; }

        public string? CategoryName { get; set; }

        public StatusImage IsDefault { get; set; }

        public int CategoryId { get; set; }

        //public List<ProductImage> ProductImages { get; set; }


    }
}
