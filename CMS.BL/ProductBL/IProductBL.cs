using CMS_Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_BL.ProductBL
{
    public interface IProductBL<T, TResult> : IBaseBL<Product,ProductModel>
    {
        /// <summary>
        /// Lấy mã Product mới
        /// </summary>
        /// <returns></returns>
        Task<int> GetNewCode();
        /// <summary>
        /// Lấy danh sách Image theo ProductID
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        List<Object> GetImageByProductID(int productId);
        /// <summary>
        /// Lấy tất cả sản phẩm cùng danh mục
        /// </summary>
        /// <param name="categoryId">ID danh mục muốn lấy</param>
        /// <returns>Danh sách sản phẩm</returns>
        Task<List<ProductModel>> GetAllProductByCategoryId(int categoryId);
    }
}
