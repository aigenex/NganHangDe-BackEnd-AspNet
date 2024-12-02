using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NganHangDe_Backend.Data.IRepositories;
using NganHangDe_Backend.Models;
using NganHangDe_Backend.ServerModels;
using NganHangDe_Backend.ServerSettings;
using Swashbuckle.AspNetCore.Annotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NganHangDe_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IOptions<JwtSetting> _jwtSettings;

        public AuthController(IUserRepository userRepository, IOptions<JwtSetting> options)
        {
            _userRepository = userRepository;
            _jwtSettings = options;
        }

        // dont need register

        // allow anonymous
        [HttpPost("login")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Login", Description = "Login to get token")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginModel model)
        {
            var user = await _userRepository.GetUserByUsernameAsync(model.Username);

            //if (user?.Password != model.Password)
            //{
            //    return Unauthorized("Username or Password are wrong");
            //}
            try
            {
                var passwordHash = BCrypt.Net.BCrypt.HashPassword(user.Password);
                if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, passwordHash))
                {
                    return Unauthorized("Username or Password are wrong");
                }
            } catch (Exception ex)
            {
                var x = ex.Message;
            }

            

            // jwt

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id ?? throw new Exception("UserId is missing")),
                new Claim(ClaimTypes.Name, user.Username),
                new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            foreach (var role in user.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var serectKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Value.Secret));

            var token = new JwtSecurityToken(
                _jwtSettings.Value.Issuer,
                _jwtSettings.Value.Audience,
                claims,
                expires: DateTime.Now.AddSeconds(_jwtSettings.Value.AccessTokenExpiration),
                signingCredentials: new SigningCredentials(serectKey, SecurityAlgorithms.HmacSha256)
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);  

            return Ok(new { token = tokenString });
        }



        [Authorize]
        [HttpGet("role")]
        [SwaggerOperation(Summary = "Get logined user role", Description = "Require jwt token")]
        public IActionResult Me()
        {
            var username = User.Identity?.Name; // get username from token
            var roles = User.Claims.Where(claim => claim.Type == ClaimTypes.Role).Select(claim => claim.Value);
            return Ok(new { username, roles });
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet("admin")]
        [SwaggerOperation(Summary = "Check your are admin", Description = "Require jwt token")]
        public IActionResult Admin()
        {
            return Ok("Admin");
        }

        [Authorize]
        [HttpGet("check-token")]
        [SwaggerOperation(Summary = "Check token", Description = "Require jwt token")]
        public IActionResult CheckToken()
        {
            return Ok("Token is valid");
        }

        [Authorize]
        [HttpPatch("change-password")]
        [SwaggerOperation(Summary = "Change password", Description = "Require jwt token")]
        public async Task<IActionResult> ChangePasswordAsync([FromBody] ChangePasswordModel model)
        {
            var username = User.Identity?.Name;

            #pragma warning disable CS8604 // Possible null reference argument.
            var user = await _userRepository.GetUserByUsernameAsync(username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(model.OldPassword, user.Password))
            {
                return Unauthorized("Old password is wrong");
            }

            user.Password = BCrypt.Net.BCrypt.HashPassword(model.NewPassword, 13);
            await _userRepository.UpdateUserAsync(user);

            return Ok("Password changed successfully");
        }


    }
}
