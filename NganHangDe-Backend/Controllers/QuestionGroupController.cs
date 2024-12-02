using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NganHangDe_Backend.Data.IRepositories;
using NganHangDe_Backend.Models;
using System.Net;

namespace NganHangDe_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionGroupController : ControllerBase
    {
        private readonly IQuestionGroupRepository _groupRepository;

        public QuestionGroupController(IQuestionGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _groupRepository.GetAllInfoAsync();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateGroup([FromBody] QuestionGroup group)
        {
            await _groupRepository.CreateAsync(group);
            return StatusCode((int)HttpStatusCode.Created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGroup(string id, [FromBody] QuestionGroup group)
        {
            await _groupRepository.UpdateAsync(id, group);
            return StatusCode((int)HttpStatusCode.NoContent);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGroup(string id)
        {
            await _groupRepository.DeleteAsync(id);
            return StatusCode((int)HttpStatusCode.NoContent);
        }
    }
}
