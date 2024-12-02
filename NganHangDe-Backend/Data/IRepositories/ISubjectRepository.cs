using NganHangDe_Backend.Models;

namespace NganHangDe_Backend.Data.IRepositories
{
    public interface ISubjectRepository
    {
        public Task<List<Subject>> GetSubjectsAsync();
        public Task<Subject> GetSubjectByIdAsync(string id);
        public Task<bool> CreateSubjectAsync(Subject subject);
        public Task UpdateSubjectAsync(string id, Subject subject);
        public Task DeleteSubjectAsync(string id);


    }
}
