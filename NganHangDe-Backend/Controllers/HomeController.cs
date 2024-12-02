using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NganHangDe_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {

        [HttpGet]
        [Authorize]
        public IActionResult GetHome()
        {
            var guid = Guid.NewGuid();
            return Ok($"Phiên kết nối: {guid}");
        }
    }
}
