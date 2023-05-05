using AutoMapper;
using CMS_Common;
using CMS_Common.Database;
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
                //var records = await _context.Articels
                // .Join(_context.TagsArticles, a => a.ArticleID, ta => ta.ArticleID, (a, ta) => new { Article = a, TagArticle = ta })
                // .Join(_context.Tags, x => x.TagArticle.TagID, t => t.TagID, (x, t) => new { x.Article, Tag = t })
                // .Join(_context.Comments, x => x.Article.ArticleID, c => c.ArticleID, (x, c) => new { x.Article, x.Tag, Comment = c })
                // .Join(_context.Users, x => x.Comment.UserID, u => u.UserID, (x, u) => new { x.Article, x.Tag, x.Comment, User = u })
                // .Join(_context.ArticleCategorieArticles, x => x.Article.ArticleID, ca => ca.ArticleID, (x, ca) => new { x.Article, x.Tag, x.Comment, x.User, ArticleCategoryArticle = ca })
                // .Join(_context.CategoryArticles, x => x.ArticleCategoryArticle.CategoryArticleID, ca => ca.CategoryActicleID, (x, ca) => new ArticleModel
                // {
                //     ArticleID = x.Article.ArticleID,
                //     Title = x.Article.Title,
                //     Content = x.Article.Content,
                //     Description = x.Article.Description,
                //     Author = x.Article.Author,
                //     Image = x.Article.Image,
                //     CreateAt = x.Article.CreateAt,
                //     TagName = x.Tag.TagName,
                //     UserName = x.User.Username,
                //     ContentComment = x.Comment.Content,
                //     CategoryName = ca.CategoryName
                // })
                // .ToListAsync();
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
    }
}
