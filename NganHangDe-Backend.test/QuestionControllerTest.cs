using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NganHangDe_Backend.Controllers;
using NganHangDe_Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace NganHangDe_Backend.test
{
    public class QuestionControllerTest
    {
        private readonly WebApplicationFactory<Program> _factory = new();
        private readonly ITestOutputHelper _output;

        public QuestionControllerTest(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task GetQuestionsAsync()
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync("/api/question");

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            // log output
            _output.WriteLine(responseString);
            Assert.True(true);

        }

        [Fact]
        public async Task GetQuestionByIdAsync()
        {
            var client = _factory.CreateClient();

            // a string obj id, please replace with a real one
            var objId = "66c2e9f4a36b1021e0c16a89";

            var response = await client.GetAsync($"/api/question/{objId}");

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            // log output
            _output.WriteLine(responseString);
            Assert.True(true);

        }

        [Fact]
        public async Task CreateQuestionAsync()
        {
            var client = _factory.CreateClient();

            var question = new Question
            {
                Type = "multiple_choice",
                Class = 10,
                Title = "What is the capital of Vietnam?",
                SubjectId = "66b30d25e020e2783a05aeff",
                Difficulty = "easy",
                KnowledgeScope = new string[] { "geography" },
                Answers = new Answer[]
                {
                    new Answer
                    {
                        Content = "Hanoi",
                        IsCorrect = true
                    },
                    new Answer
                    {
                        Content = "Ho Chi Minh City",
                        IsCorrect = false
                    },
                    new Answer
                    {
                        Content = "Da Nang",
                        IsCorrect = false
                    },
                    new Answer
                    {
                        Content = "Hue",
                        IsCorrect = false
                    }
                },
                Explanation = "Hanoi is the capital of Vietnam",
                CreatedAt = new DateTime(2021, 10, 10),
            };

            var response = await client.PostAsJsonAsync("/api/question", question);

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            // log output
            _output.WriteLine(responseString);

        }

        [Fact]
        public async Task DeleteQuestionAsync()
        {
            var client = _factory.CreateClient();

            // a string obj id, please replace with a real one
            var objId = "66d7242b9727be1584033fab";


            var response = await client.DeleteAsync($"/api/question/{objId}");

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            // log output
            _output.WriteLine(responseString);
        }

        [Fact]
        // error 
        public async Task CreateSimilarAsync()
        {
            var obj = new InputSimilarQuestion
            {
                Message = "",
                Input = new QuestionInfo
                {
                    Title = "Hiện tượng nào sau đây là do sự nở ra vì nhiệt của chất rắn?",
                    Answers = new Answer[]
                    {
                        new() {
                            Content = "Đường ray xe lửa bị cong lên khi trời nóng",
                            IsCorrect = true
                        },
                        new() {
                            Content = "Mực nước trong nhiệt kế dâng lên khi tiếp xúc với nhiệt",
                            IsCorrect = false
                        },
                        new() {
                            Content = "Bóng đèn dây tóc nóng sáng",
                            IsCorrect = false
                        },
                        new() {
                            Content = "Khí trong bóng bay phồng lên khi trời nắng",
                            IsCorrect = false
                        }
                    },
                    Difficulty = "easy",
                    Class = 6,
                    KnowledgeScope = new string[] { "Sự dãn nở" },
                    Type = "multiple_choice",
                    Subject = new Subject
                    {
                        Name = "Vật lý",
                        Id = "66d67f9748475567f571bca9",
                        Description = "Môn vật lý"
                    },
                    SubjectId = "66d67f9748475567f571bca9",
                }
            };

            var json = JsonConvert.SerializeObject(obj, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });

            var client = _factory.CreateClient();

            var response = await client.PostAsJsonAsync("/api/question/similar", json);

           _output.WriteLine(json);

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            // log output
            _output.WriteLine(responseString);
        }
    }
}
