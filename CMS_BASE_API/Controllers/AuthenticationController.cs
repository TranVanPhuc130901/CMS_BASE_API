using CMS_BL;
using CMS_Common;
using CMS_Common.Database;
using Microsoft.AspNetCore.Mvc;

namespace CMS_BASE_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserAuth _userServices;
        private readonly MyDbContext _context;


        public AuthenticationController(IConfiguration configuration, IUserAuth userServices, MyDbContext context)
        {
            _configuration = configuration;
            _userServices = userServices;
            _context = context;
        }
        /// <summary>
        /// Đăng nhập bằng username và password
        /// </summary>
        /// <param name="request">Thông tin đăng nhập</param>
        /// <returns>Trạng thái đăng nhập , Token , RefreshToken</returns>
        [HttpPost("Login")]
        public async Task<ActionResult<AuthMessageDto>> Login([FromBody] LoginModel request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.UserName) || string.IsNullOrEmpty(request.Password))
                {
                    return BadRequest("Vui lòng nhập đầy đủ thông tin đăng nhập.");
                }

                var user = await _userServices.Login(new Account { Username = request.UserName }, request);
                if (user.Success)
                {
                    var authResponse = new AuthMessageDto
                    {
                        Success = user.Success,
                        Message = "Đăng nhập thành công",
                        Token = user.Token,
                        RefreshToken = user.RefreshToken,
                        TokenExpires = user.TokenExpires,
                        FullName = user.FullName,
                    };

                    return Ok(authResponse);
                }

                return BadRequest(user.Message);
            }
            catch
            {
                return NotFound();
            }
        }
        /// <summary>
        /// Cấp lại Token(Khi Token còn thời gian sử dụng)
        /// </summary>
        /// <returns></returns>
        [HttpPost("refresh-token")]
        public async Task<ActionResult<string>> RefreshToken()
        {
            try
            {
                var token = await _userServices.ResetToken();

                return Ok(token);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest("Error refreshing token");
            }
        }
    }
}
