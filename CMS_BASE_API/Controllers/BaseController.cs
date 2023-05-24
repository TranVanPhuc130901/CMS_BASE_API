using CMS_BL;
using CMS_Common;
using CMS_Common.Dtos;
using CMS_Common.Enums;
using CMS_Common.Resource;
using CMS_WT_API.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Azure.Core.HttpHeader;

namespace CMS_BASE_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "PermissionManager")]
    public abstract class BaseController<T, TResult> : ControllerBase where T : class where TResult : class
    {
        #region fileds
        private readonly IBaseBL<T, TResult> _baseBL;
        #endregion
        #region constructor
        public BaseController(IBaseBL<T, TResult> baseBL)
        {
            _baseBL = baseBL;
        }
        #endregion
        #region class
        /// <summary>
        /// Lấy tất cả bản ghi
        /// </summary>
        /// <returns>Danh sách bản ghi</returns>
        [HttpGet]
        public virtual async Task<ActionResult<List<TResult>>> GetAllRecord()
        {
            try
            {
                var records = await _baseBL.GetAllRecord();
                return Ok(records);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest();
            }
        }
        /// <summary>
        /// Lấy bản ghi theo ID
        /// </summary>
        /// <param name="recordId">Id bản ghi muốn lấy</param>
        /// <returns></returns>
        /// <exception cref="MyException"></exception>
        [HttpGet("{recordId}")]
        public async Task<ActionResult<TResult>> GetRecordById(int recordId)
        {
            try
            {
                var record = await _baseBL.GetRecordbyID(recordId);

                if (record == null)
                {
                    return NotFound();
                }

                return Ok(record);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new MyException("This is my exception");
            }
        }
        /// <summary>
        /// Thêm mới 1 bản ghi
        /// </summary>
        /// <param name="record">Thông tin bản ghi thêm mới</param>
        /// <returns>Bản ghi vừa thêm</returns>
        /// <exception cref="MyException"></exception>
        [HttpPost]
        public async Task<IActionResult> CreateRecord(TResult record)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _baseBL.CreateRecord(record);
                if (result.isSuccess == false)
                {
                    return HandleValidateFalse(result);
                }
                return Ok(record);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new MyException("This is my exception");
            }
        }
        /// <summary>
        /// Sửa bản ghi theo ID
        /// </summary>
        /// <param name="recordId">ID bản ghi muốn sửa</param>
        /// <param name="record">Thông tin muốn sửa</param>
        /// <returns>ID bản ghi vừa sửa</returns>
        [HttpPut("{recordId}")]
        public async Task<ActionResult> UpdateRecord(int recordId, TResult record)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
               var result = await _baseBL.UpdateRecord(record, recordId);
                if (result.isSuccess == false)
                {
                    return BadRequest();
                }
                return Ok(recordId);
            }
            catch (MyException ex)
            {
                return NotFound(ex.Message);
            }


        }
        /// <summary>
        /// Xóa 1 bản ghi theo ID
        /// </summary>
        /// <param name="recordId">Id bản ghi muốn xóa</param>
        /// <returns>Id bản ghi vừa xóa</returns>
        [HttpDelete("{recordId}")]
        public async Task<ActionResult> DeleteRecord(int recordId)
        {
            try
            {
                await _baseBL.DeleteRecordById(recordId);
                return Ok(recordId);
            }
            catch (MyException ex)
            {
                return NotFound(ex.Message);
            }
        }
        /// <summary>
        /// Lấy bản ghi theo phân trang và tìm kiếm
        /// </summary>
        /// <param name="pageIndex">Số bản ghi muốn hiển thị</param>
        /// <param name="pageSize">Trang bản ghi muốn hiển thị</param>
        /// <param name="keyword">Từ khóa muốn tìm kiếm</param>
        /// <returns>Danh sách bản ghi</returns>
        [HttpGet("PagingAndFilter")]
        public async Task<ActionResult<PagedResult<TResult>>> GetRecordPagingAndFilter(int pageIndex, int pageSize, string? keyword)
        {
            try
            {
                var pagedResult = await _baseBL.GetRecordPagingAndFilter(pageSize, pageIndex, keyword);

                return Ok(pagedResult);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest();
            }
        }
        /// <summary>
        /// xóa nhiều bản ghi theo ID
        /// </summary>
        /// <param name="recordIds">Danh sách ID muốn xóa</param>
        /// <returns>danh sách Id đã xóa</returns>
        /// <exception cref="MyException"></exception>
        [HttpDelete("BatchDelete")]
        public async Task<IActionResult> DeleteManyRecord(List<int> recordIds)
        {
            try
            {
                await _baseBL.DeleteManyRecord(recordIds);
                return Ok(recordIds);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new MyException("This is my exception");
            }
        }
        #endregion
        #region support
        /// <summary>
        /// các lỗi khi validate
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        protected IActionResult HandleValidateFalse(ServicesResult result)
        {

            switch (result.ErrorCode)
            {
                case ErrorCode.RequiredValueIsEmpty:
                    return StatusCode(StatusCodes.Status400BadRequest, new Error
                    {
                        ErrorCode = result.ErrorCode,
                        UserMsg = Resource.ErrorrRequiredValueEmptyUserMsg,
                        MoreInfo = result.Error,
                        TradeID = HttpContext.TraceIdentifier
                    });

                case ErrorCode.EmployeeCodeIsDuplicate:
                    return StatusCode(StatusCodes.Status400BadRequest, new Error
                    {
                        ErrorCode = result.ErrorCode,
                        UserMsg = Resource.DuplicateEmployeeCode,
                        TradeID = HttpContext.TraceIdentifier
                    });

                case ErrorCode.OutLengthEmployeeCode:
                    return StatusCode(StatusCodes.Status400BadRequest, new Error
                    {
                        ErrorCode = result.ErrorCode,
                        UserMsg = Resource.OutLengthEmployeeCode,
                        TradeID = HttpContext.TraceIdentifier
                    });

                case ErrorCode.OutLengthEmployeeName:
                    return StatusCode(StatusCodes.Status400BadRequest, new Error
                    {
                        ErrorCode = result.ErrorCode,
                        UserMsg = Resource.OutLengthEmployeeName,
                        TradeID = HttpContext.TraceIdentifier
                    });

                case ErrorCode.OutLengthIdentityIssuePlace:
                    return StatusCode(StatusCodes.Status400BadRequest, new Error
                    {
                        ErrorCode = result.ErrorCode,
                        UserMsg = Resource.OutLengthIdentityIssuePlace,
                        TradeID = HttpContext.TraceIdentifier
                    });

                case ErrorCode.OutLengthIdentityNumber:
                    return StatusCode(StatusCodes.Status400BadRequest, new Error
                    {
                        ErrorCode = result.ErrorCode,
                        UserMsg = Resource.OutLengthIdentityNumber,
                        TradeID = HttpContext.TraceIdentifier

                    });

                case ErrorCode.OutLengthPhoneNumber:
                    return StatusCode(StatusCodes.Status400BadRequest, new Error
                    {
                        ErrorCode = result.ErrorCode,
                        UserMsg = Resource.OutLengthPhoneNumber,
                        TradeID = HttpContext.TraceIdentifier

                    });

                case ErrorCode.OutLengthTelephoneNumber:
                    return StatusCode(StatusCodes.Status400BadRequest, new Error
                    {
                        ErrorCode = result.ErrorCode,
                        UserMsg = Resource.OutLengthTelephoneNumber,
                        TradeID = HttpContext.TraceIdentifier

                    });

                case ErrorCode.OutLengthEmail:
                    return StatusCode(StatusCodes.Status400BadRequest, new Error
                    {
                        ErrorCode = result.ErrorCode,
                        UserMsg = Resource.OutLengthEmail,
                        TradeID = HttpContext.TraceIdentifier

                    });

                case ErrorCode.OutLengthBankName:
                    return StatusCode(StatusCodes.Status400BadRequest, new Error
                    {
                        ErrorCode = result.ErrorCode,
                        UserMsg = Resource.OutLengthBankName,
                        TradeID = HttpContext.TraceIdentifier

                    });

                case ErrorCode.OutLengthBankBranchName:
                    return StatusCode(StatusCodes.Status400BadRequest, new Error
                    {
                        ErrorCode = result.ErrorCode,
                        UserMsg = Resource.OutLengthBankBranchName,
                        TradeID = HttpContext.TraceIdentifier

                    });

                case ErrorCode.EmailInvalid:
                    return StatusCode(StatusCodes.Status400BadRequest, new Error
                    {
                        ErrorCode = result.ErrorCode,
                        UserMsg = Resource.EmailInvalid,
                        TradeID = HttpContext.TraceIdentifier

                    });

                default:
                    return StatusCode(StatusCodes.Status400BadRequest, new Error
                    {
                        ErrorCode = result.ErrorCode,
                        UserMsg = Resource.OutLengthBankAccount,
                        TradeID = HttpContext.TraceIdentifier

                    });
            }


        }
        /// <summary>
        /// cấu trúc lỗi exception
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        protected IActionResult HandleException(Exception ex)
        {
            Console.WriteLine(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, new Error
            {
                ErrorCode = ErrorCode.Exception,
                DevMsg = Resource.ExceptionDevMsg,
                UserMsg = Resource.DefaultUserMsg,
                MoreInfo = ex.Message,
                TradeID = HttpContext.TraceIdentifier
            });
        } 
        #endregion
    }

}
