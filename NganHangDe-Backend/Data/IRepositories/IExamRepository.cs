using NganHangDe_Backend.Models;

namespace NganHangDe_Backend.Data.IRepositories
{
    public interface IExamRepository
    {
        public Task<List<ExamInfo>> GetExamsAsync();
        public Task<Exam> GetExamAsync(string id);
        public Task<ExamInfo> GetExamInfoAsync(string id);
        public Task<bool> CreateExamAsync(Exam subject);
        public Task UpdateExamAsync(string id, Exam subject);
        public Task DeleteExamAsync(string id);
    }
}
