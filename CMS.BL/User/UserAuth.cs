using AutoMapper;
using CMS_Common;
using CMS_Common.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace CMS_BL
{

    public class UserAuth : IUserAuth
    {
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly MyDbContext _dbContext;
        private readonly IConfiguration _configuration;

        public UserAuth(IHttpContextAccessor httpContextAccessor, MyDbContext dbContext, IConfiguration configuration, IMapper mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _dbContext = dbContext;
            _configuration = configuration;
            _mapper = mapper;

        }

        public async Task<Account> Regester(UserModel request)
        {
            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
            var user = new Account
            {
                Username = request.UserName,
                PasswordHash = passwordHash,
                PasswordSaft = passwordSalt,
                Email = request.Email,
                Fullname = request.Fullname
            };
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            return user;
        }
        public async Task<AuthMessageDto> Login(Account request, LoginModel usermd)
        {
            var user = await _dbContext.Users
                 .Where(u => u.Username == request.Username)
                 .FirstOrDefaultAsync();
            //byte[] passwordHash;
            //byte[] passwordSalt;
            //var pass = CreatePasswordHash(usermd.Password, out passwordHash, out passwordSalt);

            //var verify = VerifyPasswordHash(usermd.Password, user.PasswordHash, user.PasswordSaft);
            if (user == null)
            {
                return new AuthMessageDto { Message = "Tên đăng nhập không tồn tại trong hệ thống, vui lòng kiểm tra lại" };
            }
            else if (!VerifyPasswordHash(usermd.Password, user.PasswordHash, user.PasswordSaft))
            {
                return new AuthMessageDto { Message = "Mật khẩu không đúng" };
            }
            else
            {

                var refreshToken = CreateRefreshToken();
                SetRefreshToken(refreshToken, user);
                var Token = CreateToken(user);

                user.Token = Token.newToken;
                user.CreateToken = Token.CreateToken;
                user.ExpiresToken = Token.ExpiresToken;
                user.UserID = user.UserID;
                user.RefreshToken = refreshToken.Token;
                user.CreatedRefreshToken = refreshToken.Create;
                user.ExpiresRefreshToken = refreshToken.Expires;

                _dbContext.Update(user);

                await _dbContext.SaveChangesAsync();

                var authReponseDto = new AuthMessageDto
                {
                    Success = true,
                    Message = "Đăng nhập thành công",
                    Token = user.Token,
                    RefreshToken = user.RefreshToken,
                    TokenExpires = user.ExpiresToken,
                };


                return authReponseDto;
            }
        }
        public async Task<ActionResult<string>> ResetToken()
        {
            var refreshToken = _httpContextAccessor.HttpContext.Request.Cookies["refreshToken"];

            var user = _dbContext.Users.FirstOrDefault(x => x.RefreshToken == refreshToken);

            if (user == null)
            {
                return "Invalid Token";
            }
            else if (user.ExpiresToken < DateTime.Now)
            {
                return "Token Expires";
            }
            var Token = CreateToken(user);
            var newRefreshToken = CreateRefreshToken();
            SetRefreshToken(newRefreshToken, user);
            user.Token = Token.newToken;
            user.CreateToken = Token.CreateToken;
            user.ExpiresToken = Token.ExpiresToken;

            _dbContext.Update(user);
            await _dbContext.SaveChangesAsync();

            return Token.newToken;
        }

        public async Task<UserRoleModel> SetUsage(int UserID, int RoleID, int newRoleID)
        {
            try
            {
                var userRole = await _dbContext.UserRoles
                    .Where(ur => ur.UserID == UserID && ur.RoleID == RoleID)
                    .FirstOrDefaultAsync();
                if (userRole != null)
                {
                    // delete the existing UserRole
                    _dbContext.UserRoles.Remove(userRole);
                    await _dbContext.SaveChangesAsync();

                    // create a new UserRole with new RoleID and UserID
                    var newUserRole = new UserRole { UserID = UserID, RoleID = newRoleID };
                    await _dbContext.UserRoles.AddAsync(newUserRole);
                    await _dbContext.SaveChangesAsync();

                    // map entity to model and return
                    return _mapper.Map<UserRoleModel>(newUserRole);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<List<RoleModel>> GetRole()
        {
            var roles = await _dbContext.Roles.Select(r => new RoleModel
            {
                RoleID = r.RoleID,
                RoleName = r.RoleName,
                RoleDescription = r.RoleDescription
            }).ToListAsync();

            return roles;
        }

        public async Task<RoleModel> AddRole(RoleModel model)
        {
            var newRole = _mapper.Map<Role>(model);
            _dbContext.Add(newRole);
            await _dbContext.SaveChangesAsync();
            return _mapper.Map<RoleModel>(newRole);
        }
        public async Task<int> DeleteRole(int roleId)
        {
            try
            {
                var role = await _dbContext.Roles.FindAsync(roleId);
                if (role != null)
                {
                    _dbContext.Roles.Remove(role);
                    await _dbContext.SaveChangesAsync();
                }
                return roleId;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
        }

        public async Task<List<Permission>> GetPermissionByRoleID(int RoleID)
        {
            try
            {
                // Tìm kiếm Role trong bảng RolePermission sau đó join với bảng Permission để lấy ra PermissionName
                var permission = await _dbContext.RolePermissions.Where(rp => rp.RoleID == RoleID)
                    .Join(_dbContext.Permissions,
                    rp => rp.PermissionID, p => p.PermissionID, (rp, p) => new Permission { PermissionsName = p.PermissionsName, PermissionDesc = p.PermissionDesc })
                    .ToListAsync();

                if (permission == null)
                {
                    return null;
                }
                return permission;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        public async Task<List<object>> GetUsage()
        {
            var usage = await _dbContext.Users
        .GroupJoin(_dbContext.UserRoles, u => u.UserID, ur => ur.UserID, (u, ur) => new { User = u, UserRole = ur.FirstOrDefault() })
        .Select(x => new
        {
            UserName = x.User.Username,
            RoleName = x.UserRole != null ? x.UserRole.Role.RoleName : "No Role"
        })
        .ToListAsync();

            return usage.Cast<object>().ToList();
            //var usage = await _dbContext.UserRoles
            //.Include(ur => ur.User)
            //.Include(ur => ur.Role)
            ////.Where(ur => ur.UserID == userId)
            //.Select(ur => new
            //{
            //    UserName = ur.User.Username,
            //    RoleName = ur.Role.RoleName != null ? ur.Role.RoleName : "Not role"
            //})
            //.ToListAsync();

            //return usage.Cast<object>().ToList();
        }
        public async Task<int> CreateUsage(UserRoleModel role)
        {
            var newRole = _mapper.Map<UserRole>(role);
            _dbContext.Add(newRole);
            await _dbContext.SaveChangesAsync();
            return newRole.UserID;
        }

        public async Task<RolePermissionModel> CreateRolePermissionModel(RolePermissionModel model)
        {
            var newRole = _mapper.Map<RolePermission>(model);
            _dbContext.Add(newRole);
            await _dbContext.SaveChangesAsync();
            return _mapper.Map<RolePermissionModel>(newRole);
        }
        public async Task<int> DeleteRolePermissionModel(int RoleID, int PermissionID)
        {
            try
            {
                var rolePermission = await _dbContext.RolePermissions.FirstOrDefaultAsync(rp => rp.RoleID == RoleID && rp.PermissionID == PermissionID);
                if (rolePermission != null)
                {
                    _dbContext.RolePermissions.Remove(rolePermission);
                    await _dbContext.SaveChangesAsync();
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return -1;
            }
        }
        #region Function private
        private byte[] CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmc = new HMACSHA512())
            {
                passwordSalt = hmc.Key;
                return passwordHash = hmc.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalf)
        {
            using (var hmc = new HMACSHA512(passwordSalf))
            {
                var computedHash = hmc.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        private RefreshToken CreateRefreshToken()
        {
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.Now.AddDays(7),
                Create = DateTime.Now
            };
            return refreshToken;
        }

        private void SetRefreshToken(RefreshToken newToken, Account account)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = newToken.Expires
            };
            _httpContextAccessor.HttpContext.Response.Cookies.Append("refreshToken", newToken.Token, cookieOptions);
            account.RefreshToken = newToken.Token;
            account.CreatedRefreshToken = newToken.Create;
            account.ExpiresRefreshToken = newToken.Expires;
        }

        /// <summary>
        /// Trả về 1 chuỗi JWT cho người dung được ucng cấp dưới dạng 1 đối tượng user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private Token CreateToken(Account user)
        {

            // tạo 1 danh sách các claim để đại diện cho thông tin người dùng trong token JWT
            List<Claim> claims = new List<Claim>
            {
                // chỉ thêm duy nhất 1 claim đó là claimTypes.Name với giá trị là userName
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
        };

            //Truy xuất role từ bảng userRole
            var role = _dbContext.UserRoles
                .Include(ur => ur.Role)
                    .ThenInclude(r => r.RolePermission)
                    .ThenInclude(rp => rp.Permission)
                .Where(ur => ur.UserID == user.UserID)
                .FirstOrDefault();

            if (role != null)
            {
                var roleName = role.Role.RoleName;
                var permissions = role.Role.RolePermission
                    .Select(rp => rp.Permission)
                    .ToList();
                claims.Add(new Claim(ClaimTypes.Role, roleName.ToString()));
                foreach (var permission in permissions)
                {
                    claims.Add(new Claim("Permission", permission.PermissionsName));
                }
                // Làm gì đó với roleName và permissions
            }

            // tạo ra 1 khoá bí mật từ đối tượng sử dụng chuỗi trong cấu hình ứng dụng
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));
            //tạo đối tượng sử dụng key và thuật toán để ký kết token
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            //tạo đối tượng JWT
            var token = new JwtSecurityToken(
                // Claims là danh sách các claim, đại diện cho thông tin người dùng
                claims: claims,
                // expiess là thời gian hết hẹn của token, ở đâu là 1 ngày
                expires: DateTime.Now.AddMinutes(30),
                // đối tượng được tạo ở trên để ký kết token
                signingCredentials: creds
                );
            // chuyển đổi JWTSecurity thành 1 chuỗi token và trả về
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);


            var newToken = new Token
            {
                newToken = jwt,
                ExpiresToken = DateTime.Now.AddMinutes(30),
                CreateToken = DateTime.Now
            };

            return newToken;
        }
        #endregion
    }
}
