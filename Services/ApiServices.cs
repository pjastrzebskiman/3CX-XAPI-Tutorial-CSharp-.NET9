using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using _3CX_API_20.Models;

namespace _3CX_API_20.Services
{
    public class ApiServices
    {

        private readonly ApiConfiguration _config;
        private readonly HttpClient _httpClient;

        public ApiServices(ApiConfiguration config, HttpClient httpClient)
        {
            _config = config;
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(_config.BasePath);
        }

        public async Task<HttpRequestMessage> CreateRequestAsync(HttpMethod method, string endpoint)
        {
            var request = new HttpRequestMessage(method, endpoint);
            var token = await _config.AccessToken();
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            Console.WriteLine($"Request URL: {_httpClient.BaseAddress}{endpoint}");
          //  Console.WriteLine($"Authorization Header: Bearer {token}");
            return request;
        }
    }
}
