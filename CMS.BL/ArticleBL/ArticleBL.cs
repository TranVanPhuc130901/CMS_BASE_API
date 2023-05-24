using AutoMapper;
using CMS_Common;
using CMS_Common.Database;
using CMS_Common.Dtos;
using CMS_Common.Model;
using CMS_DL;
using CMS_WT_API.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace CMS_BL.ArticleBL
{
    public class ArticleBL : BaseBL<Article, ArticleModel>, IArticleBL<Article, ArticleModel>
    {
        private readonly MyDbContext _context;
        private readonly IMapper _mapper;
        public ArticleBL(IBaseDL<Article> baseDL, IMapper mapper, MyDbContext context) : base(baseDL, mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async override Task<List<ArticleModel>> GetAllRecord()
        {
            try
            {
                var records = _context.Articels
           .Include(a => a.TagArticles)
               .ThenInclude(ta => ta.Tags)
           .Include(a => a.Comments)
               .ThenInclude(c => c.Accounts)
           .Include(a => a.ArticleCategoryArticles)
               .ThenInclude(caa => caa.CategoryArticles)
           .AsQueryable();
                if (records == null)
                {
                    return null;
                }

                var data = await records.Select(x => new ArticleModel()
                {
                    ArticleID = x.ArticleID,
                    Title = x.Title,
                    Content = x.Content,
                    Description = x.Description,
                    Author = x.Author,
                    Image = x.Image,
                    CreateAt = x.CreateAt,
                    TagName = x.TagArticles.FirstOrDefault().Tags.TagName != null ? x.TagArticles.FirstOrDefault().Tags.TagName : "",
                    UserName = x.Comments.FirstOrDefault().Accounts.Username != null ? x.Comments.FirstOrDefault().Accounts.Username : "",
                    ContentComment = x.Comments.FirstOrDefault().Content != null ? x.Comments.FirstOrDefault().Content : "",
                    CategoryName = x.ArticleCategoryArticles.FirstOrDefault().CategoryArticles.CategoryName != null ?
                         x.ArticleCategoryArticles.FirstOrDefault().CategoryArticles.CategoryName : ""
                }).ToListAsync();
                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<ArticleModel>();
            }
        }

        public override async Task<ArticleModel> GetRecordbyID(int recordId)
        {

            var record = _context.Articels
            .Include(a => a.TagArticles)
                .ThenInclude(ta => ta.Tags)
            .Include(a => a.Comments)
                .ThenInclude(c => c.Accounts)
            .Include(a => a.ArticleCategoryArticles)
                .ThenInclude(caa => caa.CategoryArticles)
            .Where(a => a.ArticleID == recordId)
            .AsQueryable();

            if (record == null)
            {
                return null;
            }

            var data = await record.Select(x => new ArticleModel()
            {
                ArticleID = x.ArticleID,
                Title = x.Title,
                Content = x.Content,
                Description = x.Description,
                Author = x.Author,
                Image = x.Image,
                CreateAt = x.CreateAt,
                TagName = x.TagArticles.FirstOrDefault().Tags.TagName != null ? x.TagArticles.FirstOrDefault().Tags.TagName : "",
                UserName = x.Comments.FirstOrDefault().Accounts.Username != null ? x.Comments.FirstOrDefault().Accounts.Username : "",
                ContentComment = x.Comments.FirstOrDefault().Content != null ? x.Comments.FirstOrDefault().Content : "",
                CategoryName = x.ArticleCategoryArticles.FirstOrDefault().CategoryArticles.CategoryName != null ?
                     x.ArticleCategoryArticles.FirstOrDefault().CategoryArticles.CategoryName : ""
            }).FirstOrDefaultAsync();
            return data;
        }

        public async override Task<PagedResult<ArticleModel>> GetRecordPagingAndFilter(int pageSize, int pageIndex, string keyWord)
        {
            try
            {
                var records = _context.Articels
                .Include(a => a.TagArticles)
                    .ThenInclude(ta => ta.Tags)
                .Include(a => a.Comments)
                    .ThenInclude(c => c.Accounts)
                .Include(a => a.ArticleCategoryArticles)
                    .ThenInclude(caa => caa.CategoryArticles)
                    .AsQueryable();

                if (!string.IsNullOrEmpty(keyWord))
                {
                    records = records.Where(x => x.Title.Contains(keyWord));
                }
                else
                {
                    keyWord = "";
                }

                int totalRow = await records.CountAsync();

                if (pageSize == 0)
                {
                    pageSize = 20;
                    pageIndex = 1;
                }
                var data = await records.Skip((pageIndex - 1) * pageSize).Take(pageSize).Select(x => new ArticleModel()
                {
                    ArticleID = x.ArticleID,
                    Title = x.Title,
                    Content = x.Content,
                    Description = x.Description,
                    Author = x.Author,
                    Image = x.Image,
                    CreateAt = x.CreateAt,
                    TagName = x.TagArticles.FirstOrDefault().Tags.TagName != null ? x.TagArticles.FirstOrDefault().Tags.TagName : "",
                    UserName = x.Comments.FirstOrDefault().Accounts.Username != null ? x.Comments.FirstOrDefault().Accounts.Username : "",
                    ContentComment = x.Comments.FirstOrDefault().Content != null ? x.Comments.FirstOrDefault().Content : "",
                    CategoryName = x.ArticleCategoryArticles.FirstOrDefault().CategoryArticles.CategoryName != null ?
                     x.ArticleCategoryArticles.FirstOrDefault().CategoryArticles.CategoryName : ""
                }).ToListAsync();


                var articleModel = _mapper.Map<List<ArticleModel>>(data);

                return new PagedResult<ArticleModel>(articleModel, totalRow, pageSize, pageIndex, keyWord);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new MyException("This is my exception");
            }
        }

        public override async Task<ServicesResult> UpdateRecord(ArticleModel record, int recordId)
        {
            try
            {
                var existingArticle = await _context.Articels
                    .Include(a => a.TagArticles)
                    .Include(a => a.ArticleCategoryArticles)
                    .FirstOrDefaultAsync(a => a.ArticleID == recordId);

                if (existingArticle != null)
                {
                    existingArticle.ArticleID = record.ArticleID;
                    existingArticle.Title = record.Title;
                    existingArticle.Content = record.Content;
                    existingArticle.Description = record.Description;
                    existingArticle.Image = record.Image;
                    existingArticle.Author = record.Author;
                    // Cập nhật các trường khác trong bảng Product

                    var tagArticles = record.TagName.Select(tag => new TagArticle
                    {
                        Tag = tag,
                        ProductID = recordId
                    }).ToList();
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

        public override async Task<ServicesResult> CreateRecord(ArticleModel model)
        {
            try
            {
                var newArticle = new Article
                {
                    Title = model.Title,
                    Content = model.Content,
                    // Gán các trường khác trong bảng Article
                };

                var tags = model.Tags.Select(tag => new TagArticle
                {
                    Tag = tag
                }).ToList();

                newArticle.TagArticles = tags;

                var category = await _context.Categories.FindAsync(model.CategoryId);
                var articleCategory = new ArticleCategory
                {
                    Category = category
                };

                newArticle.ArticleCategoryArticles = new List<ArticleCategory> { articleCategory };

                _context.Articles.Add(newArticle);
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

    }
}
