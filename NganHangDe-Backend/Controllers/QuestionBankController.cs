using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NganHangDe_Backend.Data.IRepositories;
using NganHangDe_Backend.Models;

namespace NganHangDe_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionBankController : ControllerBase
    {
        
        private readonly IQuestionBankRepository _questionBankRepository;

        public QuestionBankController(IQuestionBankRepository questionBankRepository)
        {
            _questionBankRepository = questionBankRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetQuestionBankAsync()
        {
            var questionBanks = await _questionBankRepository.GetAll();
            // reduce the size of the response
            //foreach (var questionBank in questionBanks)
            //{
            //    questionBank.Items = Array.Empty<QuestionBankItem>();
            //}
            return Ok(questionBanks);
        }

        [HttpPost]
        public async Task<IActionResult> CreateQuestionBankAsync([FromBody] QuestionBank questionBank)
        {
            await _questionBankRepository.Create(questionBank);
            return Ok("Question bank created successfully");
        }


    }
}
