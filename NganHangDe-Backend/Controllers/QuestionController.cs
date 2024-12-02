using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NganHangDe_Backend.Models;
using NganHangDe_Backend.ServerModels;
using MongoDB.Driver;
using MongoDB.Bson;
using Microsoft.Extensions.Options;
using NganHangDe_Backend.ServerSettings;
using NganHangDe_Backend.StaticModels;
using Newtonsoft.Json.Linq;
using NganHangDe_Backend.Data.IRepositories;
using NganHangDe_Backend.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace NganHangDe_Backend.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly ISubjectRepository _subjectRepository;
        private readonly IGeminiChatService _geminiChatRepository;
        private readonly IOptions<ExamDbSetting> _settings;

        public QuestionController(IQuestionRepository questionRepository, IOptions<ExamDbSetting> settings, ISubjectRepository subjectRepository, IGeminiChatService geminiChatRepository)
        {
            _questionRepository = questionRepository;
            _settings = settings;
            _subjectRepository = subjectRepository;
            _geminiChatRepository = geminiChatRepository;
        }



        [HttpGet]
        public async Task<IActionResult> GetQuestions()
        {
            var ql = await _questionRepository.GetAllInfoAsync(true);
            return Ok(ql);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetQuestionById(string id)
        {
            var question = await _questionRepository.GetInfoAsync(id);
            if (question == null)
            {
                return NotFound();
            }
            return Ok(question);
        }

        [HttpPost]
        public async Task<IActionResult> CreateQuestion([FromBody] Question input)
        {
            // get userId from token
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            input.CreatedBy = userId;

            if (!ObjectId.TryParse(input.SubjectId, out ObjectId id))
            {
                return BadRequest();
            }

            if (await _subjectRepository.GetSubjectByIdAsync(id.ToString()) == null)
            {
                return NotFound();
            }

            //var question = Question.CreateQuestionModel(input);

            //var q = await _questionRepository.CreateQuestionAsync(question);
            var q = await _questionRepository.CreateAsync(input);
            return CreatedAtAction(nameof(GetQuestionById), new { id = q.Id }, q);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateQuestion(string id, [FromBody] Question input)
        {
            if (!ObjectId.TryParse(input.SubjectId, out ObjectId sid))
            {
                return BadRequest();
            }

            if (await _subjectRepository.GetSubjectByIdAsync(sid.ToString()) == null)
            {
                return NotFound();
            }

            //var quesion = Question.CreateQuestionModel(input);
            await _questionRepository.UpdateAsync(id, input);

            //await _questionRepository.UpdateQuestionAsync(id, quesion);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuestion(string id)
        {
            await _questionRepository.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("{id}/similar")]
        public async Task<IActionResult> GenerateSimilarQuestion(string id)
        {
            var question = await _questionRepository.GetAsync(id);

            if (question == null)
            {
                return NotFound();
            }

            var result = await _geminiChatRepository.Send(question);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }

        [HttpPost("similar")]
        public async Task<IActionResult> GenerateSimilarQuestion([FromBody] InputSimilarQuestion data)
        {

            var result = await _geminiChatRepository.Send(data.Message, data.Input);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }
    }
}
