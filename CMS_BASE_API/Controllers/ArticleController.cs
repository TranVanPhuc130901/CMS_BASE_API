using CMS_BL;
using CMS_BL.ArticleBL;
using CMS_Common;
using CMS_Common.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CMS_BASE_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleController : BaseController<Article, ArticleModel>
    {
        public ArticleController(IArticleBL<Article, ArticleModel> articleBL) : base(articleBL)
        {
        }
    }
}
