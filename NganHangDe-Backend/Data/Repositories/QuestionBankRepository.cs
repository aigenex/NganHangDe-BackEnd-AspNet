using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using NganHangDe_Backend.Data.IRepositories;
using NganHangDe_Backend.Models;
using NganHangDe_Backend.ServerSettings;

namespace NganHangDe_Backend.Data.Repositories
{
    public class QuestionBankRepository : IQuestionBankRepository
    {
        private readonly IMongoCollection<QuestionBank> _qCollection;

        public QuestionBankRepository(IOptions<ExamDbSetting> options)
        {
            var client = new MongoClient(options.Value.ConnectionString);
            var database = client.GetDatabase(options.Value.DatabaseName);
            _qCollection = database.GetCollection<QuestionBank>(options.Value.QuestionBankCollectionName);
        }

        public Task<QuestionBank> Create(QuestionBank questionBank)
        {
            _qCollection.InsertOne(questionBank);
            return Task.FromResult(questionBank);
        }

        public Task<QuestionBank> Delete(string id)
        {
            _qCollection.DeleteOne(q => q.Id == id);
            return Task.FromResult<QuestionBank>(null);
        }

        public Task<IEnumerable<QuestionBank>> GetAll()
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
                //new ("$addFields", new BsonDocument
                //{
                //    {"Subject.Id", "$Subject._id" }
                //}),
                new ("$project", new BsonDocument
                {
                    {"Subject._id", 0 },
                }),
                //new ("$lookup", new BsonDocument
                //{
                //    {"from", "Questions"},
                //    {"localField", "QuestionIds"},
                //    {"foreignField", "_id"},
                //    {"as", "Questions"}
                //}),
                //new ("$project", new BsonDocument
                //{
                //    {"Questions._id", "0" }
                //})
            };

            var result = _qCollection.Aggregate<QuestionBank>(pineline).ToList();
            //return Task.FromResult(_qCollection.Find(q => true).ToEnumerable());
            return Task.FromResult(result.AsEnumerable());
        }

        public Task<QuestionBank> GetById(string id)
        {
            return Task.FromResult(_qCollection.Find(q => q.Id == id).FirstOrDefault());
        }

        public Task<QuestionBank> Update(string id, QuestionBank questionBank)
        {
            _qCollection.ReplaceOne(q => q.Id == id, questionBank);
            return Task.FromResult(questionBank);
        }
    }
}
