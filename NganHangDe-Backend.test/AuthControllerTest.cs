using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using NganHangDe_Backend.Controllers;
using NganHangDe_Backend.ServerModels;
using System.Net.Http.Json;
using Xunit.Abstractions;

namespace NganHangDe_Backend.test
{
    public class AuthControllerTest
    {
        private readonly WebApplicationFactory<Program> _factory = new();
        private readonly ITestOutputHelper _output;

        public AuthControllerTest(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task LoginAsync()
        {
            var client = _factory.CreateClient();

            var response = await client.PostAsJsonAsync("/api/auth/login", new LoginModel
            {
                Username = "admin",
                Password = "admin"
            });

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            // log output
            _output.WriteLine(responseString);
            Assert.Contains("token", responseString);

        }
    }
}