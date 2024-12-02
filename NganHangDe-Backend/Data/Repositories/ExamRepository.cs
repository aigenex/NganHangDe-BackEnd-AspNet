using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using NganHangDe_Backend.Data.IRepositories;
using NganHangDe_Backend.Models;
using NganHangDe_Backend.ServerSettings;

namespace NganHangDe_Backend.Data.Repositories
{
    public class ExamRepository : IExamRepository
    {
        private readonly IMongoCollection<Exam> _exams;

        public ExamRepository(IOptions<ExamDbSetting> options)
        {
            var client = new MongoClient(options.Value.ConnectionString);
            var database = client.GetDatabase(options.Value.DatabaseName);
            _exams = database.GetCollection<Exam>(options.Value.ExamCollectionName);
        }

        public Task<bool> CreateExamAsync(Exam subject)
        {
            try
            {
                _exams.InsertOne(subject);
                return Task.FromResult(true);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Task.FromResult(false);
            }
        }

        public Task DeleteExamAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Exam> GetExamAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<ExamInfo> GetExamInfoAsync(string id)
        {
            var pineline = new BsonDocument[]
            {
                new ("$lookup", new BsonDocument
                {
                    {"from", "Subjects"},
                    {"localField", "SubjectId"},
                    {"foreignField", "_id"},
                    {"as", "Subject"}
                }),
                new ("$unwind", "$Subject"),
                new ("$lookup", new BsonDocument
                {
                    {"from", "Questions"},
                    {"localField", "QuestionIds"},
                    {"foreignField", "_id"},
                    {"as", "Questions"}
                }),
                new ("$match", new BsonDocument(
                    new BsonElement("_id", ObjectId.Parse(id))
                ))
            };

            var exam = _exams.Aggregate<ExamInfo>(pineline).FirstOrDefault();

            return Task.FromResult(exam);
        }

        public async Task<List<ExamInfo>> GetExamsAsync()
        {
            var pineline = new BsonDocument[]
            {
                new ("$lookup", new BsonDocument
                {
                    {"from", "Subjects"},
                    {"localField", "SubjectId"},
                    {"foreignField", "_id"},
                    {"as", "Subject"}
                }),
                new ("$unwind", "$Subject")
            };

            var exam = await _exams.Aggregate<ExamInfo>(pineline).ToListAsync();

            return exam;
        }

        public Task UpdateExamAsync(string id, Exam subject)
        {
            throw new NotImplementedException();
        }
    }
}
