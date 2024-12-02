using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NganHangDe_Backend.Services
{
    public interface IGeminiChatService
    {
        public Task<JObject> SendMessageJson(string message);

        public Task<T?> Send<T>(T obj);

        public Task<T?> Send<T>(string message, T? obj);
    }
}
