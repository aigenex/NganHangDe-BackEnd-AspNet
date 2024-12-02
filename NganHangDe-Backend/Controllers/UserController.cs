using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using NganHangDe_Backend.Data.IRepositories;
using NganHangDe_Backend.Models;
using NganHangDe_Backend.StaticModels;
using Swashbuckle.AspNetCore.Annotations;
using System.Reflection;

namespace NganHangDe_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

#if DEBUG
        [HttpPost("create")]
        public async Task<IActionResult> CreateUserAsync([FromBody] User user)
        {
            // Uppercase all roles
            user.Roles = user.Roles.Select(role => role.ToUpper()).ToHashSet();

            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password, 13);

            var supportedRoles = new[]
            {
                "ADMIN", "USER"
            };

            foreach (var role in user.Roles)
            {
                if (!supportedRoles.Contains(role))
                {
                    return BadRequest("Role is not supported");
                }
            }

            try
            {
                await _userRepository.CreateUserAsync(user);
            }
            catch (MongoWriteException e) when (e.WriteError.Category == ServerErrorCategory.DuplicateKey)
            {
                return Conflict("Username already exists");
            }


            return StatusCode(StatusCodes.Status201Created, "User created successfully");
        }
#endif

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetMeAsync()
        {
            var username = User.Identity?.Name;
            #pragma warning disable CS8604 // Possible null reference argument.
            var user = await _userRepository.GetUserByUsernameAsync(username);

            if (user == null)
            {
                return NotFound("User not found");
            }


            return Ok(new
            {
                username = user.Username,
                avatar = user.Avatar,
            });
        }

        [HttpPatch("avatar")]
        [Authorize]
        [Consumes("multipart/form-data")]
        [SwaggerOperation(Summary = "Update avatar", Description = "Update avatar")]
        public async Task<IActionResult> UpdateAvatarAsync(IFormFile avatar)
        {
            var username = User.Identity?.Name;
            var user = await _userRepository.GetUserByUsernameAsync(username);

            if (user == null)
            {
                return NotFound("User not found");
            }

            using var memoryStream = new MemoryStream();
            await avatar.CopyToAsync(memoryStream);

            user.SetProfilePicture(memoryStream.ToArray());
            await _userRepository.UpdateUserAsync(user);

            return Ok("Avatar updated successfully");
        }


    }
}
