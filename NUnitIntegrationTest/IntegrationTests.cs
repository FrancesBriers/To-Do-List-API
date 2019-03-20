using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using NUnit.Framework;
using TodoApi;

namespace IntegrationTests
{
    [TestFixture]
    public class IntegrationTests
    {
        private readonly HttpClient _client;
        public IntegrationTests()
        {
            var server = new TestServer(new WebHostBuilder().UseEnvironment("Development").UseStartup<Startup>());
            _client = server.CreateClient();
        }

        private async Task<HttpResponseMessage> HttpResponseMessage(string method)
        {
            var request = new HttpRequestMessage(new HttpMethod(method), "/api/Todo");

            var response = await _client.SendAsync(request);
            return response;
        }

        [Test]
        public async Task GetMethod_return200Status()
        {
            var response = await _client.GetAsync("/api/todo");

            var responseBody = await response.Content.ReadAsStringAsync();

            response.EnsureSuccessStatusCode();
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        public async Task PostMethod_return201Status() //, string method, string name, string isComplete
        {
            var response = await _client.PostAsync("/api/todo",
                new StringContent(@"{""name"":""walk dog"",""isComplete"":true}", Encoding.UTF8, "application/json"));  // HttpResponseMessage(method);

            response.EnsureSuccessStatusCode();
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        }

        [Test]
        public async Task PutMethod_return204Status() //, string method, string name, string isComplete
        {
            var response1 = await _client.PostAsync("/api/todo",
                new StringContent(@"{""name"":""walk dog"",""isComplete"":true}", Encoding.UTF8, "application/json"));

            var response = await _client.PutAsync("/api/todo/2",
                new StringContent(@"{""id"":2,""name"":""feed fish"",""isComplete"":true}", Encoding.UTF8, "application/json"));  // HttpResponseMessage(method);

            response.EnsureSuccessStatusCode();
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Test]
        public async Task CallDeleteMethod_return204Status() //, string method, string name, string isComplete
        {
            var response1 = await _client.DeleteAsync("/api/todo/1");

            response1.EnsureSuccessStatusCode();
            Assert.AreEqual(HttpStatusCode.NoContent, response1.StatusCode);

            var response2 = await _client.DeleteAsync("/api/todo/1");

            Assert.AreEqual(HttpStatusCode.NotFound, response2.StatusCode);
        }
    }
}