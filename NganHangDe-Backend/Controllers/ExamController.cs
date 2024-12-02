using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NganHangDe_Backend.Data.IRepositories;
using NganHangDe_Backend.Models;

namespace NganHangDe_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamController : ControllerBase
    {
        private readonly IExamRepository _examRepository;

        public ExamController(IExamRepository examRepository)
        {
            _examRepository = examRepository;
        }

        // post
        [HttpPost]
        public async Task<IActionResult> CreateExamAsync([FromBody]Exam exam)
        {
            var result = await _examRepository.CreateExamAsync(exam);
            if (result)
            {
                // 201 and message
                return StatusCode(StatusCodes.Status201Created, "Exam created successfully");
            }
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [HttpGet]
        public async Task<IActionResult> GetExamsAsync()
        {
            var exams = await _examRepository.GetExamsAsync();
            return Ok(exams);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetExamAsync(string id)
        {
            var exam = await _examRepository.GetExamInfoAsync(id);
            if (exam == null)
            {
                return NotFound();
            }
            return Ok(exam);
        }
    }
}
