using NganHangDe_Backend.Models;

namespace NganHangDe_Backend.Data.IRepositories
{
    public interface IUserRepository
    {
        public Task<User?> GetUserByUsernameAsync(string username);
        public Task<User?> GetUserByIdAsync(string id);
        public Task CreateUserAsync(User user);
        public Task UpdateUserAsync(User user);
        public Task DeleteUserAsync(string id);
    }
}
