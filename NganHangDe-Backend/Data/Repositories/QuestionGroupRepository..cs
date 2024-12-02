using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using NganHangDe_Backend.Data.IRepositories;
using NganHangDe_Backend.Models;
using NganHangDe_Backend.ServerSettings;

namespace NganHangDe_Backend.Data.Repositories
{
    public class QuestionGroupRepository : IQuestionGroupRepository
    {
        private readonly IMongoCollection<QuestionGroup> _gCollection;
        private readonly IMongoCollection<Question> _qCollection;
        private readonly IMongoCollection<Subject> _sCollection;

        public QuestionGroupRepository(IOptions<ExamDbSetting> options)
        {
            var client = new MongoClient(options.Value.ConnectionString);
            var database = client.GetDatabase(options.Value.DatabaseName);
            _gCollection = database.GetCollection<QuestionGroup>(options.Value.QuestionGroupCollectionName);
            _qCollection = database.GetCollection<Question>(options.Value.QuestionCollectionName);
            _sCollection = database.GetCollection<Subject>(options.Value.SubjectCollectionName);
        }



        public Task<QuestionGroup> GetAsync(string id)
        {
            var group = _gCollection.Find(i => i.Id == id).FirstOrDefault();
            return Task.FromResult(group);
        }

        public async Task<List<QuestionGroup>> GetAllAsync()
        {
            return await _gCollection.Find(i => true).ToListAsync();
        }

        public Task<QuestionInfo> GetInfoAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<QuestionGroupInfo>> GetAllInfoAsync()
        {

            /*
             db.QuestionGroups.aggregate([
    { $lookup: { from: "Questions", localField: "_id", foreignField: "GroupId", as: "Questions" } },
    { $unwind: "$Questions" },
    { $lookup: { from: "Subjects", localField: "Questions.SubjectId", foreignField: "_id", as: "Subject" } },
    { $unwind: "$Subject" },
    {
        $group: {
            _id: "$_id",
            Subject: { $first: "$Subject" },
            Source: { $first: "$Source" },
            Method: { $first: "$Method" },
            Questions: { $push: "$Questions" },
            CreatedAt: { $first: "$CreatedAt" },
            UpdatedAt: { $first: "$UpdatedAt" },
        }
    },
    { $addFields: {
        KnowledgeScope: {
            $reduce: {
                input: "$Questions.KnowledgeScope",
                initialValue: [],
                in: { $setUnion: ["$$value", "$$this"] }
            }
        },
    } },
    { $project: { "Questions.SubjectId": 0, "Questions.KnowledgeScope": 0  } },
])   
             */

            var pineline = new[]
            {
                // get all question reference to this group
                new BsonDocument("$lookup", new BsonDocument
                {
                    { "from", "Questions" },
                    { "localField", "_id" },
                    { "foreignField", "GroupId" },
                    { "as", "Questions" }
                }),
                new BsonDocument("$lookup", new BsonDocument
                {
                    { "from", "Subjects" },
                    { "localField", "SubjectId" },
                    { "foreignField", "_id" },
                    { "as", "Subject" }
                }),
                new BsonDocument("$unwind", "$Subject"),
            };

            var result = await _gCollection.Aggregate<QuestionGroupInfo>(pineline).ToListAsync();

            return result;




        }


        public async Task<QuestionGroup> CreateAsync(QuestionGroup group)
        {
            await _gCollection.InsertOneAsync(group);
            return group;
        }

        public Task UpdateAsync(string id, QuestionGroup question)
        {
            question.Id = id;
            return _gCollection.ReplaceOneAsync(i => i.Id == id, question);
        }

        public async Task DeleteAsync(string id)
        {
            await _gCollection.DeleteOneAsync(i => i.Id == id);
        }

        Task<QuestionGroupInfo> IQuestionGroupRepository.GetInfoAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}
