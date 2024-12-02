using NganHangDe_Backend.Models;

namespace NganHangDe_Backend.Data.IRepositories
{
    public interface IQuestionGroupRepository
    {
        Task<List<QuestionGroup>> GetAllAsync();
        Task<List<QuestionGroupInfo>> GetAllInfoAsync();
        Task<QuestionGroup> GetAsync(string id);
        Task<QuestionGroupInfo> GetInfoAsync(string id);
        Task<QuestionGroup> CreateAsync(QuestionGroup question);
        Task UpdateAsync(string id, QuestionGroup question);
        Task DeleteAsync(string id);
    }
}
