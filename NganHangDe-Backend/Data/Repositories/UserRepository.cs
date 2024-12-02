using Microsoft.Extensions.Options;
using MongoDB.Driver;
using NganHangDe_Backend.Data.IRepositories;
using NganHangDe_Backend.Models;
using NganHangDe_Backend.ServerSettings;

namespace NganHangDe_Backend.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _collection;

        public UserRepository(IOptions<ExamDbSetting> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _collection = database.GetCollection<User>(settings.Value.UserCollectionName);

            var usernameIndexName = "UsernameIndex";

            var usernameindexExists = _collection.Indexes.List().ToList().Exists(index => index["name"] == usernameIndexName);

            if (!usernameindexExists)
            {
                var indexModel = new CreateIndexModel<User>(
                    new IndexKeysDefinitionBuilder<User>().Ascending(x => x.Username),
                    new CreateIndexOptions
                    {
                        Unique = true,
                        Name = usernameIndexName
                    }
                );

                _collection.Indexes.CreateOne(indexModel);
            }


        }

        public async Task CreateUserAsync(User user)
        {
            await _collection.InsertOneAsync(user);
        }

        public async Task DeleteUserAsync(string id)
        {
            await _collection.DeleteOneAsync(user => user.Id == id);
        }

        public async Task<User?> GetUserByIdAsync(string id)
        {
            var user = await _collection.Find(user => user.Id == id).FirstOrDefaultAsync();
            return user;
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            var user = await _collection.Find(user => user.Username == username).FirstOrDefaultAsync();
            return user;
        }

        public async Task UpdateUserAsync(User user)
        {
            await _collection.ReplaceOneAsync(u => u.Id == user.Id, user);
        }
    }
}
