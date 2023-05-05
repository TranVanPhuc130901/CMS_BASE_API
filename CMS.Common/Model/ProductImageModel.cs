using CMS_WT_API.Enums;
using CMS_Common.CustomAttribute;
using CMS_Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace CMS_Common
{
    public class ProductImageModel
    {
        /// <summary>
        /// Id ảnh
        /// </summary>
        public int ProductImageID { get; set; }
        /// Đường dẫn ảnh
        /// </summary>
        [MyMaxLength(20, ErrorCode.OutLengthEmployeeCode)]
        public string ?ProductImageSlug { get; set; }
        /// <summary>
        /// Trạng thái ảnh (1 là ảnh gốc, 0 là ảnh phụ)
        /// </summary>
        public StatusImage IsDefault { get; set; }
        /// <summary>
        /// ID sản phẩm
        /// </summary>
        public int ProductID { get; set; }
    }
}
