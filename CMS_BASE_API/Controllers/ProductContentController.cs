using CMS_BL;
using CMS_Common;
using Microsoft.AspNetCore.Mvc;

namespace CMS_BASE_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductContentController : BaseController<ProductContent, ProductContentModel>
    {
        public ProductContentController(IBaseBL<ProductContent, ProductContentModel> baseBL) : base(baseBL)
        {
        }
    }
}
