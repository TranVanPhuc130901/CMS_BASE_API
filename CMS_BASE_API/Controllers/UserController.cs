using CMS_BL;
using CMS_Common;
using CMS_Common.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMS_BASE_API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    //[Authorize(Policy = "UserPermission")]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserAuth _userServices;
        private readonly MyDbContext _context;


        public UserController(IConfiguration configuration, IUserAuth userServices, MyDbContext context)
        {
            _configuration = configuration;
            _userServices = userServices;
            _context = context;
        }

        /// <summary>
        /// Lấy ra danh sách người dùng và quyền của họ trong hệ thống
        /// </summary>
        /// <returns>Danh sách người dùng</returns>
        [HttpGet]
        public async Task<IActionResult> GetUsage()
        {
            try
            {
                var usage = await _userServices.GetUsage();
                return Ok(usage);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    ErrorCode = 500,
                    DevMsg = "Lỗi Exception",
                    UserMsg = "Lỗi Database",
                    MoreInfo = "",
                    TraceId = HttpContext.TraceIdentifier
                });
            }
        }
        /// <summary>
        /// Lấy ra các nhóm quyền
        /// </summary>
        /// <returns>Danh sách các nhóm quyền</returns>
        [HttpGet("GetRole")]
        public async Task<IActionResult> GetRole()
        {
            try
            {
                var role = await _userServices.GetRole();
                return Ok(role);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    ErrorCode = 500,
                    DevMsg = "Lỗi Exception",
                    UserMsg = "Lỗi Database",
                    MoreInfo = "",
                    TraceId = HttpContext.TraceIdentifier
                });
            }
        }

        /// <summary>
        /// Lấy tất cả cac permission(quyền) của 1 user(RoleID)
        /// </summary>
        /// <param name="RoleID"></param>
        /// <returns></returns>
        [HttpGet("GetPermissionByRoleID")]
        public async Task<IActionResult> GetPermissionByRoleID(int RoleID)
        {
            try
            {
                var per = await _userServices.GetPermissionByRoleID(RoleID);
                return Ok(per);
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        /// <summary>
        /// Đăng kí thông tin cho user quản trị
        /// </summary>
        /// <param name="request">Thông tin user đăng kí</param>
        /// <returns></returns>
        [HttpPost("Regester")]
        public async Task<ActionResult<Account>> Regester(UserModel request)
        {
            try
            {
                var user = await _userServices.Regester(request);
                return Ok(user);
            }
            catch
            {
                return BadRequest();

            }
        }
        /// <summary>
        /// Tạo nhóm quyền mới cho User
        /// </summary>
        /// <param name="role">Thông tin nhóm quyền(UserID, RoleID)</param>
        /// <returns>Thông tin nhóm quyền vừa tạo</returns>
        [HttpPost("CreateUserRole")]
        public async Task<IActionResult> AddUserRole(UserRoleModel role)
        {
            try
            {
                return Ok(await _userServices.CreateUsage(role));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest();
            }
        }
        /// <summary>
        /// Tạo quyền mới
        /// </summary>
        /// <param name="role">Thông tin nhóm quyền cần tạo(ID, ID Quyền cần thêm)</param>
        /// <returns>Thông tin nhóm quyên vừa tạo(ID nhóm quyền, ID quyền)</returns>
        [HttpPost("createRolePermission")]
        public async Task<IActionResult> AddPermission(RolePermissionModel role)
        {
            try
            {
                var newRole = await _userServices.CreateRolePermissionModel(role);
                return Ok(newRole);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest();
            }
        }
        /// <summary>
        /// Tạo nhóm quyền mới
        /// </summary>
        /// <param name="role">Thông tin nhóm quyền cần tạo(ID, tên và mô tả)</param>
        /// <returns>Thông tin nhóm quyên vừa tạo(ID, tên nhóm quyền, mô tả)</returns>
        [HttpPost("createRole")]
        public async Task<IActionResult> AddRole(RoleModel role)
        {
            try
            {
                var newRole = await _userServices.AddRole(role);
                return Ok(newRole);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest();
            }
        }
        /// <summary>
        /// Cập nhật lại quyền cho người dùng
        /// </summary>
        /// <param name="role">Thông tin người dùng(ID người dùng và ID nhóm quyền)</param>
        /// <param name="userId">ID người dùng</param>
        /// <param name="roleId">ID nhóm quyền</param>
        /// <param name="newRoleId">ID nhóm quyền mới</param>
        /// <returns>ID người dùng và ID nhóm quyền mới</returns>
        [HttpPut]
        public async Task<IActionResult> UpdateUsage([FromBody] UserRoleModel role, int userId, int roleId, int newRoleId)
        {
            try
            {
                if (userId != role.UserID)
                {
                    return BadRequest("ID không khớp với UserID trong body");
                }

                var existingUserRole = await _userServices.SetUsage(userId, roleId, newRoleId);
                if (existingUserRole == null)
                {
                    return NotFound();
                }

                return Ok(existingUserRole);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    ErrorCode = 500,
                    DevMsg = "Lỗi Exception",
                    UserMsg = "Lỗi Database",
                    MoreInfo = "",
                    TraceId = HttpContext.TraceIdentifier
                });
            }
        }
        /// <summary>
        /// Xóa 1 nhóm quyền
        /// </summary>
        /// <param name="id">ID nhóm quyền cần xóa</param>
        /// <returns>ID nhóm quyền vừa xóa</returns>
        [HttpDelete("deleteRole")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            try
            {
                await _userServices.DeleteRole(id);
                return Ok(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest();
            }
        }
        /// <summary>
        /// Xóa 1 quyền trong 1 nhóm quyền
        /// </summary>
        /// <param name="roleID">ID nhóm quyền</param>
        /// <param name="permissionID">ID Quyền</param>
        /// <returns>Trạng thái</returns>
        [HttpDelete("DeletePermissionOnRole")]
        public async Task<IActionResult> DeletePermissionOnRole(int roleID, int permissionID)
        {
            try
            {
                var delPer = await _userServices.DeleteRolePermissionModel(roleID, permissionID);
                if (delPer == 0)
                {
                    return NotFound("Không tìm thấy ID");
                }
                else if (delPer == 1)
                {
                    return Ok("Delete Success");
                }
                else
                {
                    return StatusCode(500);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest();

            }
        }
    }
}
