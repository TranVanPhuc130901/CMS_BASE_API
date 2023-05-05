using System.ComponentModel.DataAnnotations;

namespace CMS_Common
{
    public class ProductContentModel
    {
        /// <summary>
        /// ID content sản phẩm
        /// </summary>
        public int ProductContentID { get; set; }
        /// <summary>
        /// content sản phẩm
        /// </summary>
        public string? ProductContentName { get; set; }
        /// <summary>
        /// mã sản phẩm
        /// </summary>
        public int ProductID { get; set; }
    }
}
