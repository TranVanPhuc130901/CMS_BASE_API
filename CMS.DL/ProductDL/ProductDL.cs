using AutoMapper;
using CMS_Common;
using CMS_Common.Database;
using Microsoft.EntityFrameworkCore;

namespace CMS_DL.ProductDL
{
    public class ProductDL : BaseDL<Product>, IProductDL
    {
        #region filed
        private MyDbContext _context; 
        private IMapper _mapper;
        #endregion
        #region Constructor
        public ProductDL(MyDbContext context, IMapper mapper) : base(context, mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        #endregion
        #region class
        public async Task<int> GetNewCode()
        {
            // Lấy ProductId bằng phương thức RanDomID
            var productId = RandomID.GenerateRandomNumber();
            return productId;
        }
        public List<Object> GetImageByProductID(int productId)
        {
            try
            {
                // Tạo query join đến bảng image để lấy ra ảnh
                var query = from product in _context.Products
                            join image in _context.ProductImages
                            on product.ProductID equals image.ProductID
                            where product.ProductID == productId
                            select new
                            {
                                ProductID = product.ProductID,
                                ProductName = product.ProductName.FirstOrDefault() != null ? product.ProductName.FirstOrDefault().ToString() : "",
                                ProductImageSlug = image.ProductImageSlug,
                                IsDefault = image.IsDefault
                            };

                return query.Cast<object>().ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        /// <summary>
        /// Lấy tất cả sản phẩm có cùng danh mục
        /// </summary>
        /// <param name="catrgoryId">Id danh mục muốn lây</param>
        /// <returns>danh sách các sản phẩm</returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<ProductModel>> GetProductByCategoryId(int categoryId)
        {
            var products = await _context.Products.Join(
                _context.ProductCategories,
                product => product.ProductID,
                productCategory => productCategory.ProductID,
                (product, productCategory) => new { Product = product, ProductCategory = productCategory }
            )
            .Where(x => x.ProductCategory.CategoryID == categoryId)
            .Select(x => x.Product)
            .ToListAsync();

            return _mapper.Map<List<ProductModel>>(products);
        }
        #endregion
    }
}
