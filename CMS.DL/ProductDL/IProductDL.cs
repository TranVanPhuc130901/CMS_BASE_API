using CMS_Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_DL.ProductDL
{
    public interface IProductDL : IBaseDL<Product> 
    {
        /// <summary>
        /// Lấy mã Product mới
        /// </summary>
        /// <returns>Mã mới</returns>
        Task<int> GetNewCode();
        /// <summary>
        /// Lấy tất cả ảnh có cung ProductID
        /// </summary>
        /// <param name="productId">ID Product muốn lấy ảnh</param>
        /// <returns>Danh sách ảnh cân lấy</returns>
        List<Object> GetImageByProductID(int productId);
        /// <summary>
        /// Lấy tất cả sản phẩm có cùng danh mục
        /// </summary>
        /// <param name="catrgoryId">Mã danh mục cần tìm</param>
        /// <returns>Danh sách sản phẩm cần lấy</returns>
        Task<List<ProductModel>> GetProductByCategoryId(int catrgoryId);
    }
}
