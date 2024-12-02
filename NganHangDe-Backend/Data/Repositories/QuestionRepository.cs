using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using NganHangDe_Backend.Data.IRepositories;
using NganHangDe_Backend.Models;
using NganHangDe_Backend.ServerModels;
using NganHangDe_Backend.ServerSettings;

namespace NganHangDe_Backend.Data.Repositories
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly IMongoCollection<Question> _qCollection;
        private readonly IMongoCollection<Subject> _sCollection;

        public QuestionRepository(IOptions<ExamDbSetting> options)
        {
            var client = new MongoClient(options.Value.ConnectionString);
            var database = client.GetDatabase(options.Value.DatabaseName);
            _qCollection = database.GetCollection<Question>(options.Value.QuestionCollectionName);
            _sCollection = database.GetCollection<Subject>(options.Value.SubjectCollectionName);

        }

        public async Task<Question> CreateAsync(Question question)
        {
            await _qCollection.InsertOneAsync(question);
            return question;
        }

        public async Task DeleteAsync(string id)
        {
            await _qCollection.DeleteOneAsync(q => q.Id == id);
        }

        public Task<Question> GetAsync(string id)
        {
            return _qCollection.Find(q => q.Id == id).FirstOrDefaultAsync();
        }

        public Task<List<Question>> GetAllAsync()
        {
            return _qCollection.Find(q => true).ToListAsync();
        }

        public async Task<List<QuestionInfo>> GetAllInfoAsync(bool alone = false)
        {
            //db.Questions.aggregate([
            //             {
            //             $lookup:
            //    {
            //    from: "Subjects",
            //             	localField: "SubjectId",
            //             	foreignField: "_id",

            //                 as: "Subject"
            //             }
            //},
            //{ $unwind: "$Subject" },
            //{ $match: { GroupId: null } }
            //             ])
            var pineline = new BsonDocument[]
            {
                new("$lookup", new BsonDocument
                {
                    {"from", "Subjects"},
                    {"localField", "SubjectId"},
                    {"foreignField", "_id"},
                    {"as", "Subject"}
                }),
                new("$unwind", "$Subject"),
                //new BsonDocument("$match", new BsonDocument("GroupId", BsonNull.Value))
            };

            //if (alone)
            //{
            //    pineline
            //}


            var result = await _qCollection.Aggregate<QuestionInfo>(pineline).ToListAsync();


            return result;

        }

        public async Task<QuestionInfo> GetInfoAsync(string id)
        {
            var lookupPipeline = await _qCollection
                .Aggregate()
                .Lookup<Question, Subject, QuestionInfo>(
                    foreignCollection: _sCollection,
                    localField: q => q.SubjectId,
                    foreignField: s => s.Id,
                    @as: q => q.Subject
                )
                .Unwind<QuestionInfo, QuestionInfo>(q => q.Subject)
                .Match(q => q.Id == id)
                .FirstOrDefaultAsync();
            return lookupPipeline;
        }

        public Task UpdateAsync(string id, Question question)
        {
            question.Id = id;
            return _qCollection.ReplaceOneAsync(q => q.Id == id, question);
        }
    }
}
