using Amazon.Runtime.Internal.Endpoints.StandardLibrary;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System.Net.Http.Headers;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace NganHangDe_Backend.Services
{
    public class GenerationConfig
    {
        public float Temperature { get; set; }
        public int TopK { get; set; }
        public float TopP { get; set; }
        public int MaxOutputTokens { get; set; }
        public string ResponseMimeType { get; set; }
    }

    public class Part
    {
        public string Text { get; set; }
    }

    public class ERole
    {
        public const string User = "user";
        public const string Model = "model";
    }

    public class SafetyRatingsCategory
    {
        public const string SexuallyExplicit = "HARM_CATEGORY_SEXUALLY_EXPLICIT";
        public const string HeteSpeech = "HARM_CATEGORY_HATE_SPEECH";
        public const string Harassment = "HARM_CATEGORY_HARASSMENT";
        public const string DangerousContent = "HARM_CATEGORY_DANGEROUS_CONTENT";
    }

    public class SafetyRatingsThreshold
    {
        public const string None = "BLOCK_NONE";
        public const string OnlyHigh = "BLOCK_ONLY_HIGH";
        public const string MediumAndAbove = "BLOCK_MEDIUM_AND_ABOVE";
        public const string LowAndAbove = "BLOCK_LOW_AND_ABOVE";
        public const string Unspecified = "HARM_BLOCK_THRESHOLD_UNSPECIFIED";
    }

    public class SafetyRating
    {
        public string Category { get; set; }
        public string Threshold { get; set; }
    }

    public class Content
    {
        public string Role { get; set; }
        public Part[] Parts { get; set; }
    }

    public class SendMessage
    {
        public List<Content> Contents { get; set; }
        public Content SystemInstruction { get; set; }
        public GenerationConfig GenerationConfig { get; set; }
        public SafetyRating[] SafetySettings { get; set; }
    }


    public class GeminiChatService : IGeminiChatService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _model;
        private readonly GenerationConfig _generationConfig;
        private readonly UriBuilder _url;
        private readonly Content _systemInstruction;
        private readonly SafetyRating[] _safetySettings;

        public GeminiChatService(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            _apiKey = configuration["Gemini:ApiKey"] ?? throw new ArgumentNullException("Gemini:ApiKey");
            _model = configuration["Gemini:Model"] ?? throw new ArgumentNullException("Gemini:Model");
            _generationConfig = configuration.GetSection("Gemini:GenerationConfig").Get<GenerationConfig>() ?? throw new ArgumentNullException("Gemini:GenerationConfig");
            _url = new UriBuilder
            {
                Scheme = "https",
                Host = "generativelanguage.googleapis.com",
                Path = "/v1beta/models/" + _model + ":generateContent",
                Query = "key=" + _apiKey
            };
            _systemInstruction = new Content
            {
                Role = ERole.User,
                Parts = new[] { new Part { Text = configuration["Gemini:SystemInstruction"] ?? throw new ArgumentNullException("Gemini:SystemInstruction") } }
            };
            _safetySettings = new[]
            {
                new SafetyRating { Category = SafetyRatingsCategory.SexuallyExplicit, Threshold = SafetyRatingsThreshold.None },
                new SafetyRating { Category = SafetyRatingsCategory.HeteSpeech, Threshold = SafetyRatingsThreshold.None },
                new SafetyRating { Category = SafetyRatingsCategory.Harassment, Threshold = SafetyRatingsThreshold.None },
                new SafetyRating { Category = SafetyRatingsCategory.DangerousContent, Threshold = SafetyRatingsThreshold.None }
            };
        }

        public async Task<T?> Send<T>(T obj)
        {
            //var message = new
            //{
            //    role = "user",
            //    parts = new[] { new { 
            //        text = Newtonsoft.Json.JsonConvert.SerializeObject(obj) 
            //    } },
            //};

            var contents = new List<Content>
            {
                new Content
                {
                    Role = ERole.User,
                    Parts = new[] { new Part { Text = JsonConvert.SerializeObject(obj) } }
                }
            };

            var sendData = new SendMessage
            {
                Contents = contents,
                SystemInstruction = _systemInstruction,
                GenerationConfig = _generationConfig,
                SafetySettings = _safetySettings,
            };


            // convert all first character of keys to lower case
            //var json = Newtonsoft.Json.JsonConvert.SerializeObject(sendData, new JsonSerializerSettings
            //{
            //    ContractResolver = new CamelCasePropertyNamesContractResolver()
            //});

            //var request = new HttpRequestMessage(HttpMethod.Post, _url.ToString());
            //request.Content = new StringContent(json, Encoding.UTF8);
            //request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            //var res = await _httpClient.SendAsync(request).ConfigureAwait(false);
            //var body = await res.Content.ReadAsStringAsync();
            //var data = JObject.Parse(body);
            //var str = data?["candidates"]?[0]?["content"]?["parts"]?[0]?["text"]?.ToString();

            var str = await Send(sendData);
            return str != null ? JsonConvert.DeserializeObject<T>(str) : default;
        }

        public async Task<string> Send(SendMessage message)
        {
            var json = JsonConvert.SerializeObject(message, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
            });
            var request = new HttpRequestMessage(HttpMethod.Post, _url.ToString());
            request.Content = new StringContent(json, Encoding.UTF8);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var res = await _httpClient.SendAsync(request).ConfigureAwait(false);
            var body = await res.Content.ReadAsStringAsync();
            var data = JObject.Parse(body);
            // get final data
            var str = data?["candidates"]?[0]?["content"]?["parts"]?[0]?["text"]?.ToString() ?? "";
            return str;
        }

        public async Task<T?> Send<T>(string message, T? obj)
        {
            var contents = new List<Content>
            {
                new Content
                {
                    Role = ERole.User,
                    Parts = new[] { new Part { Text =
$@"
${message}
${JsonConvert.SerializeObject(obj)}
" } }
                }
            };

            var sendData = new SendMessage
            {
                Contents = contents,
                SystemInstruction = _systemInstruction,
                GenerationConfig = _generationConfig,
                SafetySettings = _safetySettings,
            };

            var str = await Send(sendData);

            return str != null ? JsonConvert.DeserializeObject<T>(str) : default;
        }

        public async Task<JObject> SendMessageJson(string text)
        {
            throw new NotImplementedException();
        }
    }
}
