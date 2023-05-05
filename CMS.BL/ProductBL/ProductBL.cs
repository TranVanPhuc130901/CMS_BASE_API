using AutoMapper;
using CMS_Common;
using CMS_Common.Database;
using CMS_Common.Dtos;
using CMS_DL;
using CMS_DL.ProductDL;
using CMS_WT_API.Dtos;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace CMS_BL.ProductBL
{
    public class ProductBL : BaseBL<Product, ProductModel>, IProductBL<Product, ProductModel>
    {
        #region filed
        private IProductDL _productDL;
        private MyDbContext _context;
        private IMapper _mapper;
        #endregion
        #region constructor
        public ProductBL(IBaseDL<Product> baseDL, IMapper mapper, IProductDL productDL, MyDbContext context)
          : base(baseDL, mapper)
        {
            _productDL = productDL;
            _context = context;
            _mapper = mapper;
        }
        #endregion
        #region class
        public async Task<int> GetNewCode()
        {
            return await _productDL.GetNewCode();
        }
        public List<Object> GetImageByProductID(int productId)
        {
            return _productDL.GetImageByProductID(productId);
        }
        #endregion
        #region overide
        public async override Task<List<ProductModel>> GetAllRecord()
        {
            try
            {
                var query = _context.Products
                    .Include(p => p.ProductCategories)
                        .ThenInclude(pc => pc.Category)
                    .Include(p => p.ProductImages)
                    .Include(p => p.ProductContents)
                    .Include(p => p.ProductMetaDatas)
                    .Include(p => p.ProductPrices)
                    .AsQueryable();

                //.SelectMany(x => x.ProductImages, (p, i) => new { Product = p, Image = i })
                var records = await query.Select(x => new ProductModel()
                {
                    ProductID = x.ProductID,
                    ProductCode = x.ProductCode.FirstOrDefault() != null ? x.ProductCode.FirstOrDefault().ToString() : "",
                    ProductName = x.ProductName.FirstOrDefault() != null ? x.ProductName.FirstOrDefault().ToString() : "",
                    ProductDescription = x.ProductDescription.FirstOrDefault() != null ? x.ProductDescription.FirstOrDefault().ToString() : "",
                    CreatedDate = x.CreatedDate,
                    ModifinedDate = x.ModifinedDate,
                    ProductStatus = x.ProductStatus,
                    ProductImageSlug = x.ProductImages.FirstOrDefault() != null ? x.ProductImages.FirstOrDefault().ProductImageSlug : "",
                    IsDefault = x.ProductImages.FirstOrDefault() != null ? x.ProductImages.FirstOrDefault().IsDefault : 0,
                    ProductCost = x.ProductPrices.FirstOrDefault() != null ? x.ProductPrices.FirstOrDefault().ProductCost : 0,
                    ProductPromotional = x.ProductPrices.FirstOrDefault() != null ? x.ProductPrices.FirstOrDefault().ProductPromotional : 0,
                    ProductContentName = x.ProductContents.FirstOrDefault() != null ? x.ProductContents.FirstOrDefault().ProductContentName : "",
                    ProductMetaDataTitle = x.ProductMetaDatas.FirstOrDefault() != null ? x.ProductMetaDatas.FirstOrDefault().ProductMetaDataTitle : "",
                    ProductMetadataDescrition = x.ProductMetaDatas.FirstOrDefault() != null ? x.ProductMetaDatas.FirstOrDefault().ProductMetadataDescrition : "",
                    CategoryName = x.ProductCategories.FirstOrDefault() != null ? x.ProductCategories.FirstOrDefault().Category.CategoryName : ""
                }).ToListAsync();
                return records;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<ProductModel>();
            }
        }
        public override async Task<ProductModel> GetRecordbyID(int recordId)
        {
            var query = _context.Products
         .Include(p => p.ProductImages)
         .Include(p => p.ProductContents)
         .Include(p => p.ProductMetaDatas)
         .Include(p => p.ProductPrices)
         .Where(p => p.ProductID == recordId);

            if (query == null)
            {
                return null;
            }

            var record = await query.Select(x => new ProductModel()
            {
                ProductID = x.ProductID,
                ProductCode = x.ProductCode.FirstOrDefault() != null ? x.ProductCode.FirstOrDefault().ToString() : "",
                ProductName = x.ProductName.FirstOrDefault() != null ? x.ProductName.FirstOrDefault().ToString() : "",
                ProductDescription = x.ProductDescription.FirstOrDefault() != null ? x.ProductDescription.FirstOrDefault().ToString() : "",
                CreatedDate = x.CreatedDate,
                ModifinedDate = x.ModifinedDate,
                ProductStatus = x.ProductStatus,
                ProductImageSlug = x.ProductImages.FirstOrDefault() != null ? x.ProductImages.FirstOrDefault().ProductImageSlug : "",
                IsDefault = x.ProductImages.FirstOrDefault() != null ? x.ProductImages.FirstOrDefault().IsDefault : 0,
                ProductCost = x.ProductPrices.FirstOrDefault() != null ? x.ProductPrices.FirstOrDefault().ProductCost : 0,
                ProductPromotional = x.ProductPrices.FirstOrDefault() != null ? x.ProductPrices.FirstOrDefault().ProductPromotional : 0,
                ProductContentName = x.ProductContents.FirstOrDefault() != null ? x.ProductContents.FirstOrDefault().ProductContentName : "",
                ProductMetaDataTitle = x.ProductMetaDatas.FirstOrDefault() != null ? x.ProductMetaDatas.FirstOrDefault().ProductMetaDataTitle : "",
                ProductMetadataDescrition = x.ProductMetaDatas.FirstOrDefault() != null ? x.ProductMetaDatas.FirstOrDefault().ProductMetadataDescrition : "",
                CategoryName = x.ProductCategories.FirstOrDefault() != null ? x.ProductCategories.FirstOrDefault().Category.CategoryName : ""
            }).FirstOrDefaultAsync();
            return record;
        }
        public async override Task<ServicesResult> CreateRecord(ProductModel newProduct)
        {
            try
            {
                // Gọi phương thức GetNewCode để tạo ra ProductID mới
                var newCode = await GetNewCode();

                // Gán ProductID mới cho đối tượng newProduct
                newProduct.ProductID = newCode;

                // Thực hiện lưu mới sản phẩm vào cơ sở dữ liệu
                var product = _mapper.Map<Product>(newProduct);

                var result = await _productDL.CreateRecord(product);

                return new ServicesResult
                {
                    isSuccess = true,

                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new MyException("This is my exception");
            }
        }
        public async override Task<PagedResult<ProductModel>> GetRecordPagingAndFilter(int pageSize, int pageIndex, string keyWord)
        {
            try
            {
                var query = _context.Products
                .Include(p => p.ProductCategories)
                    .ThenInclude(pc => pc.Category)
                .Include(p => p.ProductImages)
                .Include(p => p.ProductContents)
                .Include(p => p.ProductMetaDatas)
                .Include(p => p.ProductPrices)
                .AsQueryable();

                if (!string.IsNullOrEmpty(keyWord))
                {
                    query = query.Where(x => x.ProductCode.Contains(keyWord));
                }
                else
                {
                    keyWord = "";
                }

                int totalRow = await query.CountAsync();

                if (pageSize == 0)
                {
                    pageSize = 20;
                    pageIndex = 1;
                }

                var data = await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).Select(x => new ProductModel()
                {
                    ProductID = x.ProductID,
                    ProductCode = x.ProductCode.FirstOrDefault() != null ? x.ProductCode.FirstOrDefault().ToString() : "",
                    ProductName = x.ProductName.FirstOrDefault() != null ? x.ProductName.FirstOrDefault().ToString() : "",
                    ProductDescription = x.ProductDescription.FirstOrDefault() != null ? x.ProductDescription.FirstOrDefault().ToString() : "",
                    CreatedDate = x.CreatedDate,
                    ModifinedDate = x.ModifinedDate,
                    ProductStatus = x.ProductStatus,
                    ProductImageSlug = x.ProductImages.FirstOrDefault() != null ? x.ProductImages.FirstOrDefault().ProductImageSlug : "",
                    IsDefault = x.ProductImages.FirstOrDefault() != null ? x.ProductImages.FirstOrDefault().IsDefault : 0,
                    ProductCost = x.ProductPrices.FirstOrDefault() != null ? x.ProductPrices.FirstOrDefault().ProductCost : 0,
                    ProductPromotional = x.ProductPrices.FirstOrDefault() != null ? x.ProductPrices.FirstOrDefault().ProductPromotional : 0,
                    ProductContentName = x.ProductContents.FirstOrDefault() != null ? x.ProductContents.FirstOrDefault().ProductContentName : "",
                    ProductMetaDataTitle = x.ProductMetaDatas.FirstOrDefault() != null ? x.ProductMetaDatas.FirstOrDefault().ProductMetaDataTitle : "",
                    ProductMetadataDescrition = x.ProductMetaDatas.FirstOrDefault() != null ? x.ProductMetaDatas.FirstOrDefault().ProductMetadataDescrition : "",
                    CategoryName = x.ProductCategories.FirstOrDefault() != null ? x.ProductCategories.FirstOrDefault().Category.CategoryName : ""
                }).ToListAsync();

                var productModel = _mapper.Map<List<ProductModel>>(data);

                return new PagedResult<ProductModel>(productModel, totalRow, pageSize, pageIndex, keyWord);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new MyException("This is my exception");
            }
        }
        /// <summary>
        /// Lấy tất cả sản phẩm có cùng danh mục
        /// </summary>
        /// <param name="categoryId">Id danh mục muốn lấy</param>
        /// <returns>Danh sách sản phẩm</returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<ProductModel>> GetAllProductByCategoryId(int categoryId)
        {
            var query = _context.Products
                .Include(p => p.ProductImages)
                .Include(p => p.ProductContents)
                .Include(p => p.ProductMetaDatas)
                .Include(p => p.ProductPrices)
                .Where(p => p.ProductCategories.Any(pc => pc.CategoryID == categoryId));
            var records = await query.Select(x => new ProductModel()
            {
                ProductID = x.ProductID,
                ProductCode = x.ProductCode.FirstOrDefault() != null ? x.ProductCode.FirstOrDefault().ToString() : "",
                ProductName = x.ProductName.FirstOrDefault() != null ? x.ProductName.FirstOrDefault().ToString() : "",
                ProductDescription = x.ProductDescription.FirstOrDefault() != null ? x.ProductDescription.FirstOrDefault().ToString() : "",
                CreatedDate = x.CreatedDate,
                ModifinedDate = x.ModifinedDate,
                ProductStatus = x.ProductStatus,
                ProductImageSlug = x.ProductImages.FirstOrDefault() != null ? x.ProductImages.FirstOrDefault().ProductImageSlug : "",
                IsDefault = x.ProductImages.FirstOrDefault() != null ? x.ProductImages.FirstOrDefault().IsDefault : 0,
                ProductCost = x.ProductPrices.FirstOrDefault() != null ? x.ProductPrices.FirstOrDefault().ProductCost : 0,
                ProductPromotional = x.ProductPrices.FirstOrDefault() != null ? x.ProductPrices.FirstOrDefault().ProductPromotional : 0,
                ProductContentName = x.ProductContents.FirstOrDefault() != null ? x.ProductContents.FirstOrDefault().ProductContentName : "",
                ProductMetaDataTitle = x.ProductMetaDatas.FirstOrDefault() != null ? x.ProductMetaDatas.FirstOrDefault().ProductMetaDataTitle : "",
                ProductMetadataDescrition = x.ProductMetaDatas.FirstOrDefault() != null ? x.ProductMetaDatas.FirstOrDefault().ProductMetadataDescrition : "",
                CategoryName = x.ProductCategories.FirstOrDefault() != null ? x.ProductCategories.FirstOrDefault().Category.CategoryName : ""
            }).ToListAsync();
            return _mapper.Map<List<ProductModel>>(records);
        }
        #endregion
    }
}

