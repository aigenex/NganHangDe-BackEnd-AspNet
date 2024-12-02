using NganHangDe_Backend.Models;

namespace NganHangDe_Backend.Data.IRepositories
{
    public interface IQuestionBankRepository
    {
        public Task<IEnumerable<QuestionBank>> GetAll();

        public Task<QuestionBank> GetById(string id);

        public Task<QuestionBank> Create(QuestionBank questionBank);

        public Task<QuestionBank> Update(string id, QuestionBank questionBank);

        public Task<QuestionBank> Delete(string id);
    }
}
