using CMS_BL;
using CMS_Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CMS_BASE_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductMetaDataController : BaseController<ProductMetaData, ProductMetaDataModel>
    {
        public ProductMetaDataController(IBaseBL<ProductMetaData, ProductMetaDataModel> baseBL) : base(baseBL)
        {
        }
    }
}
