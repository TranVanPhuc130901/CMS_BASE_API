using CMS_BL;
using CMS_Common;
using Microsoft.AspNetCore.Mvc;

namespace CMS_BASE_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductImageController : BaseController<ProductImage, ProductImageModel>
    {
        public ProductImageController(IBaseBL<ProductImage, ProductImageModel> baseBL) : base(baseBL)
        {
        }
    }
}
