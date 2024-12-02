using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NganHangDe_Backend.Data.IRepositories;
using NganHangDe_Backend.Models;

namespace NganHangDe_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        private readonly ISubjectRepository _repository;

        public SubjectController(ISubjectRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetSubjects()
        {
            var subjects = await _repository.GetSubjectsAsync();
            return Ok(subjects);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSubjectById(string id)
        {
            var subject = await _repository.GetSubjectByIdAsync(id);
            if (subject == null)
            {
                return NotFound();
            }
            return Ok(subject);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSubject([FromBody] Subject subject)
        {
            subject.Id = null;
            await _repository.CreateSubjectAsync(subject);
            return CreatedAtAction(nameof(GetSubjectById), new { id = subject.Id }, subject);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSubject(string id, [FromBody] Subject subject)
        {
            var s = await _repository.GetSubjectByIdAsync(id);
            if (s == null)
            {
                return NotFound();
            }

            await _repository.UpdateSubjectAsync(id, subject);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubject(string id)
        {
            var subject = await _repository.GetSubjectByIdAsync(id);
            if (subject == null)
            {
                return NotFound();
            }

            await _repository.DeleteSubjectAsync(id);
            return NoContent();
        }
    }
}
