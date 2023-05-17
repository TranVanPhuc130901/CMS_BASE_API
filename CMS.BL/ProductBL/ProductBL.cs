using AutoMapper;
using CMS_Common;
using CMS_Common.Database;
using CMS_Common.Dtos;
using CMS_DL;
using CMS_DL.ProductDL;
using CMS_WT_API.Dtos;
using Microsoft.EntityFrameworkCore;

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
        public override async Task<List<ProductModel>> GetAllRecord()
        {
            try
            {
                var query = _context.Products
                    .Include(p => p.ProductCategories)!
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
                    ProductCode = x.ProductCode,
                    ProductName = x.ProductName,
                    ProductDescription = x.ProductDescription,
                    CreatedDate = x.CreatedDate,
                    ModifinedDate = x.ModifinedDate,
                    ProductStatus = x.ProductStatus,
                    ProductImageSlug = x.ProductImages != null && x.ProductImages.FirstOrDefault() != null ? x.ProductImages.FirstOrDefault()!.ProductImageSlug : "",
                    ProductCost = x.ProductPrices != null && x.ProductPrices.FirstOrDefault() != null ? x.ProductPrices.FirstOrDefault()!.ProductCost : 0,
                    ProductPromotional = x.ProductPrices != null && x.ProductPrices.FirstOrDefault() != null ? x.ProductPrices.FirstOrDefault()!.ProductPromotional : 0,
                    ProductContentName = x.ProductContents != null && x.ProductContents.FirstOrDefault() != null ? x.ProductContents.FirstOrDefault()!.ProductContentName : "",
                    ProductMetaDataTitle = x.ProductMetaDatas != null && x.ProductMetaDatas.FirstOrDefault() != null ? x.ProductMetaDatas.FirstOrDefault()!.ProductMetaDataTitle : "",
                    ProductMetadataDescrition = x.ProductMetaDatas != null && x.ProductMetaDatas.FirstOrDefault() != null ? x.ProductMetaDatas.FirstOrDefault()!.ProductMetadataDescrition : "",
                    CategoryName = x.ProductCategories != null && x.ProductCategories.FirstOrDefault() != null ? x.ProductCategories.FirstOrDefault()!.Category!.CategoryName : "",
                    CategoryId = x.ProductCategories != null && x.ProductCategories.FirstOrDefault() != null ? x.ProductCategories.FirstOrDefault()!.Category!.CategoryID : 0
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
                ProductCode = x.ProductCode,
                ProductName = x.ProductName,
                ProductDescription = x.ProductDescription,
                CreatedDate = x.CreatedDate,
                ModifinedDate = x.ModifinedDate,
                ProductStatus = x.ProductStatus,
                ProductImageSlug = x.ProductImages != null && x.ProductImages.FirstOrDefault() != null ? x.ProductImages.FirstOrDefault()!.ProductImageSlug : "",
                ProductCost = x.ProductPrices != null && x.ProductPrices.FirstOrDefault() != null ? x.ProductPrices.FirstOrDefault()!.ProductCost : 0,
                ProductPromotional = x.ProductPrices != null && x.ProductPrices.FirstOrDefault() != null ? x.ProductPrices.FirstOrDefault()!.ProductPromotional : 0,
                ProductContentName = x.ProductContents != null && x.ProductContents.FirstOrDefault() != null ? x.ProductContents.FirstOrDefault()!.ProductContentName : "",
                ProductMetaDataTitle = x.ProductMetaDatas != null && x.ProductMetaDatas.FirstOrDefault() != null ? x.ProductMetaDatas.FirstOrDefault()!.ProductMetaDataTitle : "",
                ProductMetadataDescrition = x.ProductMetaDatas != null && x.ProductMetaDatas.FirstOrDefault() != null ? x.ProductMetaDatas.FirstOrDefault()!.ProductMetadataDescrition : "",
                CategoryName = x.ProductCategories != null && x.ProductCategories.FirstOrDefault() != null ? x.ProductCategories.FirstOrDefault()!.Category!.CategoryName : "",
                CategoryId = x.ProductCategories != null && x.ProductCategories.FirstOrDefault() != null ? x.ProductCategories.FirstOrDefault()!.Category!.CategoryID : 0
            }).FirstOrDefaultAsync();
            return record!;
        }

        public override async Task<ServicesResult> UpdateRecord(ProductModel record, int recordId)
        {
            try
            {
                var existingProduct = await _context.Products
                    .Include(p => p.ProductContents)
                    .Include(p => p.ProductImages)
                    .Include(p => p.ProductMetaDatas)
                    .Include(p => p.ProductPrices)
                    .Include(p => p.ProductCategories)
                    .FirstOrDefaultAsync(p => p.ProductID == recordId);

                if (existingProduct != null)
                {
                    existingProduct.ProductCode = record.ProductCode;
                    existingProduct.ProductName = record.ProductName;
                    existingProduct.ProductDescription = record.ProductDescription;
                    // Cập nhật các trường khác trong bảng Product

                    var existingProductContent = existingProduct.ProductContents.FirstOrDefault(pc => pc.ProductID == recordId);
                    if (existingProductContent != null)
                    {
                        existingProductContent.ProductContentName = record.ProductContentName;
                        // Cập nhật các trường khác trong bảng ProductContent
                    }
                    else
                    {
                        existingProductContent = new ProductContent
                        {
                            ProductContentName = record.ProductContentName,
                            // Gán các trường khác trong bảng ProductContent
                        };
                        existingProduct.ProductContents.Add(existingProductContent);
                    }

                    var existingProductImage = existingProduct.ProductImages.FirstOrDefault();
                    if (existingProductImage != null)
                    {
                        existingProductImage.ProductImageSlug = record.ProductImageSlug;
                        // Cập nhật các trường khác trong bảng ProductImage
                    }
                    else
                    {
                        existingProductImage = new ProductImage
                        {
                            ProductImageSlug = record.ProductImageSlug,
                            // Gán các trường khác trong bảng ProductImage
                        };
                        existingProduct.ProductImages.Add(existingProductImage);
                    }

                    var existingProductMetaData = existingProduct.ProductMetaDatas.FirstOrDefault();
                    if (existingProductMetaData != null)
                    {
                        existingProductMetaData.ProductMetaDataTitle = record.ProductMetaDataTitle;
                        existingProductMetaData.ProductMetadataDescrition = record.ProductMetadataDescrition;
                        // Cập nhật các trường khác trong bảng ProductMetaData
                    }
                    else
                    {
                        existingProductMetaData = new ProductMetaData
                        {
                            ProductMetaDataTitle = record.ProductMetaDataTitle,
                            ProductMetadataDescrition = record.ProductMetadataDescrition,
                            // Gán các trường khác trong bảng ProductMetaData
                        };
                        existingProduct.ProductMetaDatas.Add(existingProductMetaData);
                    }

                    var existingProductPrice = existingProduct.ProductPrices.FirstOrDefault();
                    if (existingProductPrice != null)
                    {
                        existingProductPrice.ProductCost = record.ProductCost;
                        existingProductPrice.ProductPromotional = record.ProductPromotional;
                        // Cập nhật các trường khác trong bảng ProductPrice
                    }
                    else
                    {
                        existingProductPrice = new ProductPrice
                        {
                            ProductCost = record.ProductCost,
                            ProductPromotional = record.ProductPromotional,
                        };
                        existingProduct.ProductPrices.Add(existingProductPrice);
                    }

                    var category = await _context.Categories.FindAsync(record.CategoryId);
                    if (category != null)
                    {
                        var existingProductCategory = existingProduct.ProductCategories.FirstOrDefault();
                        if (existingProductCategory != null)
                        {
                            existingProduct.ProductCategories.Remove(existingProductCategory);
                        }

                        await _context.SaveChangesAsync();

                        existingProductCategory = new ProductCategory
                        {
                            Category = category
                        };
                        existingProduct.ProductCategories.Add(existingProductCategory);
                    }

                    await _context.SaveChangesAsync();

                    return new ServicesResult
                    {
                        isSuccess = true
                    };
                }

                return new ServicesResult
                {
                    isSuccess = false
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new MyException("This is my exception");
            }
        }


        public override async Task<ServicesResult> CreateRecord(ProductModel model)
        {
            try
            {
                var newProduct = new Product
                {
                    ProductCode = model.ProductCode,
                    ProductName = model.ProductName,
                    ProductDescription = model.ProductDescription,
                    // Gán các trường khác trong bảng Product
                };

                var newProductContent = new ProductContent
                {
                    ProductContentName = model.ProductContentName,
                    // Gán các trường khác trong bảng ProductContent
                };

                var newProductImage = new ProductImage
                {
                    ProductImageSlug = model.ProductImageSlug,
                    //IsDefault = model.IsDefault,
                    // Gán các trường khác trong bảng ProductImage
                };

                var newProductMetaData = new ProductMetaData
                {
                    ProductMetaDataTitle = model.ProductMetaDataTitle,
                    ProductMetadataDescrition = model.ProductMetadataDescrition,
                    // Gán các trường khác trong bảng ProductMetaData
                };

                var newProductPrice = new ProductPrice
                {
                    ProductCost = model.ProductCost,
                    ProductPromotional = model.ProductPromotional,
                    // Gán các trường khác trong bảng ProductPrice
                };

                var category = await _context.Categories.FindAsync(model.CategoryId);
                var newProductCategory = new ProductCategory
                {
                    Category = category
                };

                newProduct.ProductContents = new List<ProductContent> { newProductContent };
                newProduct.ProductImages = new List<ProductImage> { newProductImage };
                newProduct.ProductMetaDatas = new List<ProductMetaData> { newProductMetaData };
                newProduct.ProductPrices = new List<ProductPrice> { newProductPrice };
                newProduct.ProductCategories = new List<ProductCategory> { newProductCategory };

                _context.Products.Add(newProduct);
                await _context.SaveChangesAsync();

                return new ServicesResult
                {
                    isSuccess = true
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
                    .Include(p => p.ProductCategories)!
                    .ThenInclude(pc => pc.Category)
                    .Include(p => p.ProductImages)
                    .Include(p => p.ProductContents)
                    .Include(p => p.ProductMetaDatas)
                    .Include(p => p.ProductPrices)
                    .AsQueryable();

                if (!string.IsNullOrEmpty(keyWord))
                {
                    query = query.Where(x => x.ProductCode!.Contains(keyWord));
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
                    ProductCode = x.ProductCode,
                    ProductName = x.ProductName,
                    ProductDescription = x.ProductDescription,
                    CreatedDate = x.CreatedDate,
                    ModifinedDate = x.ModifinedDate,
                    ProductStatus = x.ProductStatus,
                    ProductImageSlug = x.ProductImages != null && x.ProductImages.FirstOrDefault() != null ? x.ProductImages.FirstOrDefault()!.ProductImageSlug : "",
                    ProductCost = x.ProductPrices != null && x.ProductPrices.FirstOrDefault() != null ? x.ProductPrices.FirstOrDefault()!.ProductCost : 0,
                    ProductPromotional = x.ProductPrices != null && x.ProductPrices.FirstOrDefault() != null ? x.ProductPrices.FirstOrDefault()!.ProductPromotional : 0,
                    ProductContentName = x.ProductContents != null && x.ProductContents.FirstOrDefault() != null ? x.ProductContents.FirstOrDefault()!.ProductContentName : "",
                    ProductMetaDataTitle = x.ProductMetaDatas != null && x.ProductMetaDatas.FirstOrDefault() != null ? x.ProductMetaDatas.FirstOrDefault()!.ProductMetaDataTitle : "",
                    ProductMetadataDescrition = x.ProductMetaDatas != null && x.ProductMetaDatas.FirstOrDefault() != null ? x.ProductMetaDatas.FirstOrDefault()!.ProductMetadataDescrition : "",
                    CategoryName = x.ProductCategories != null && x.ProductCategories.FirstOrDefault() != null ? x.ProductCategories.FirstOrDefault()!.Category!.CategoryName : "",
                    CategoryId = x.ProductCategories != null && x.ProductCategories.FirstOrDefault() != null ? x.ProductCategories.FirstOrDefault()!.Category!.CategoryID : 0
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

