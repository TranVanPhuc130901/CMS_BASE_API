using CMS_BL;
using CMS_Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CMS_BASE_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductPriceController : BaseController<ProductPrice, ProductPriceModel>
    {
        public ProductPriceController(IBaseBL<ProductPrice, ProductPriceModel> baseBL) : base(baseBL)
        {
        }
    }
}
