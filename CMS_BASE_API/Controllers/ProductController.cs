using CMS_BL.ProductBL;
using CMS_Common;
using CMS_Common.Dtos;
using CMS_Common.Enums;
using CMS_Common.Resource;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMS_BASE_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Policy = "PermissionManager")]
    public class ProductController : BaseController<Product, ProductModel>
    {
        #region fileds
        private IProductBL<Product, ProductModel> _productBL;
        #endregion
        #region constructor
        public ProductController(IProductBL<Product, ProductModel> productBL)
           : base(productBL)
        {
            _productBL = productBL;
        }
        #endregion
        #region class
        [HttpGet("GetNewProductCode")]
        public async Task<IActionResult> GetNewProductCode()
        {
            try
            {
                var newCode = await _productBL.GetNewCode();

                if (newCode != null)
                {
                    return StatusCode(StatusCodes.Status200OK, newCode);
                }

                return StatusCode(StatusCodes.Status500InternalServerError, new Error
                {
                    ErrorCode = ErrorCode.RequiredValueIsEmpty,
                    DevMsg = Resource.DBErrDevMsg,
                    UserMsg = Resource.DefaultUserMsg,
                    TradeID = HttpContext.TraceIdentifier
                });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
        /// <summary>
        /// Lấy tất cả ảnh theo ID sản phẩm
        /// </summary>
        /// <param name="recordId">Id sản phẩm cần lấy</param>
        /// <returns>Danh sách ảnh theo Id</returns>
        [HttpGet("GetImage")]
        public IActionResult GetImageByProductId(int recordId)
        {
            try
            {
                var images = _productBL.GetImageByProductID(recordId);
                if (images != null)
                {
                    return Ok(images);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
        /// <summary>
        /// Lấy tất cả sản phẩm có cùng danh mục
        /// </summary>
        /// <param name="recordId">Id danh mục muốn lấy</param>
        /// <returns>Danh mục sản phẩm cần lấy</returns>
        [HttpGet("GetProductByCategorry")]
        public async Task<IActionResult> GetProductByCategoryId(int categoryId)
        {
            try
            {
                var product = await _productBL.GetAllProductByCategoryId(categoryId);
                if(product == null)
                {
                    return NotFound();
                }
                return Ok(product);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message); 
                return BadRequest();
            }
        }
        #endregion
    }

}
