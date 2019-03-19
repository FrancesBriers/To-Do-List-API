using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using TodoApi;
using Xunit;


namespace TodoApiIntegrationTest
{
    public class IntegrationTest 
    {
        private readonly HttpClient _client;

        public IntegrationTest()
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
        
        [Fact]
        public async Task Test_Get_All()
        {
            var response = await _client.GetAsync("/api/todo");

            var responseBody = await response.Content.ReadAsStringAsync();

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    

        [Theory]
        [InlineData("GET")]
        public async Task CallGetMethod_return200Status(string method)
        {
            var response = await HttpResponseMessage(method);

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Theory]
        [InlineData("POST")]
        public async Task CallPostMethod_return201Status(string method) //, string method, string name, string isComplete
        {
            var response = await _client.PostAsync("/api/todo",
                new StringContent(@"{""name"":""walk dog"",""isComplete"":true}", Encoding.UTF8, "application/json"));  // HttpResponseMessage(method);

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Theory]
        [InlineData("PUT")]
        public async Task CallPutMethod_return204Status(string method) //, string method, string name, string isComplete
        {
            var response1 = await _client.PostAsync("/api/todo",
                new StringContent(@"{""name"":""walk dog"",""isComplete"":true}", Encoding.UTF8, "application/json"));

            var response = await _client.PutAsync("/api/todo/2",
                new StringContent(@"{""id"":2,""name"":""feed fish"",""isComplete"":true}", Encoding.UTF8, "application/json"));  // HttpResponseMessage(method);

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Theory]
        [InlineData("DELETE")]
        public async Task CallDeleteMethod_return204Status(string method) //, string method, string name, string isComplete
        {
            var response1 = await _client.DeleteAsync("/api/todo/1");

            response1.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response1.StatusCode);
        }
    }
}
