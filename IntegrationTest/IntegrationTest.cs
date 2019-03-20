using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Xunit;



namespace TodoApi.APITests
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

        [Theory]
        [InlineData("GET")]
        public async Task CallGetMethod_return200Status(string method)
        {
            var response = await HttpResponseMessage(method);

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Theory]
        [InlineData("GET", 1)]
        public async Task CallGetMethodWithID_Return200Status(string method, int? id = null)
        {
            var response = await HttpResponseMessage(method);

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Theory]
        [InlineData("POST")]
        public async Task CallPostMethod_return201Status(string method) //, string method, string name, string isComplete
        {
            var response = await HttpResponseMessage(method);

            response.EnsureSuccessStatusCode(); 
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
