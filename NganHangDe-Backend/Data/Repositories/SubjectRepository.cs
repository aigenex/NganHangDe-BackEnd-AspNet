using Microsoft.Extensions.Options;
using MongoDB.Driver;
using NganHangDe_Backend.Data.IRepositories;
using NganHangDe_Backend.Models;
using NganHangDe_Backend.ServerSettings;

namespace NganHangDe_Backend.Data.Repositories
{
    public class SubjectRepository : ISubjectRepository
    {
        private readonly IMongoCollection<Subject> _collection;

        public SubjectRepository(IOptions<ExamDbSetting> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _collection = database.GetCollection<Subject>(settings.Value.SubjectCollectionName);
        }

        public async Task<List<Subject>> GetSubjectsAsync()
        {
            return await _collection.Find(subject => true).ToListAsync();
        }

        public async Task<Subject> GetSubjectByIdAsync(string id)
        {
            return await _collection.Find(subject => subject.Id == id).FirstOrDefaultAsync();
        }

        public async Task<bool> CreateSubjectAsync(Subject subject)
        {
            await _collection.InsertOneAsync(subject);
            return true;
        }

        public async Task UpdateSubjectAsync(string id, Subject subject)
        {
            await _collection.ReplaceOneAsync(s => s.Id == id, subject);
        }

        public async Task DeleteSubjectAsync(string id)
        {
            var r = await _collection.DeleteOneAsync(s => s.Id == id);
            if (r.DeletedCount == 0)
            {
                throw new Exception("Object cannot be deleted");
            }
        }
    }
}
