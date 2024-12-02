using NganHangDe_Backend.Models;
using NganHangDe_Backend.ServerModels;

namespace NganHangDe_Backend.Data.IRepositories
{
    public interface IQuestionRepository
    {
        Task<List<Question>> GetAllAsync();
        Task<Question> GetAsync(string id);
        public Task<List<QuestionInfo>> GetAllInfoAsync(bool alone = false);
        public Task<QuestionInfo> GetInfoAsync(string id);
        Task<Question> CreateAsync(Question question);
        Task UpdateAsync(string id, Question question);
        Task DeleteAsync(string id);
    }
}
