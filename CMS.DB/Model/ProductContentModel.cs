using System.ComponentModel.DataAnnotations;

namespace CMS_WT_API.Model
{
    public class ProductContentModel
    {
        public int ProductContentID { get; set; }
        /// <summary>
        /// content sản phẩm
        /// </summary>
        public string ProductContentName { get; set; }
        /// <summary>
        /// mã sản phẩm
        /// </summary>
        public int ProductID { get; set; }
    }
}
