using CMS_Common;
using CMS_Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_BL.ArticleBL
{
    public interface IArticleBL<T, TResult> : IBaseBL<Article, ArticleModel>
    {
    }
}
