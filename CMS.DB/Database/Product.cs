using CMS_WT_API.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS_DB
{
    /// <summary>
    /// Bảng sản phẩm
    /// </summary>
    public class Product 
    {
        /// <summary>
        /// ID sản phẩm
        /// </summary>
        public int ProductID { set; get; }
        /// <summary>
        /// Mã Sản phẩm
        /// </summary>
        public string ProductCode { get; set; }
        /// <summary>
        /// Tên sản phẩm
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// Mô tả ngắn về sản phẩm
        /// </summary>
        public string ProductDescription { get; set; }
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

        public ICollection<ProductCategory> ProductCategories { get; set; }

        public ICollection<ProductImage> ProductImages { get; set; }

        public ICollection<ProductContent> ProductContents { get; set; }

        public ICollection<ProductPrice> ProductPrices { get; set; }

        public ICollection<ProductMetaData> ProductMetaDatas { get; set; }
        


        //public Product()
        //{
        //    ProductCategories = new List<ProductCategory>();
        //}


    }
}
