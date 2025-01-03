using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace _3CX_API_20
{
    public class User
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public int Id { get; set; }
    }
    public class UsersResponse
    {
        public List<User> Value { get; set; }
    }
    public class UserApi
    {
        private readonly ApiConfiguration _config;
        private readonly HttpClient _httpClient;

        public UserApi(ApiConfiguration config)
        {
            _config = config;
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(_config.BasePath)
            };
        }
        private async Task<HttpRequestMessage> CreateRequestAsync(HttpMethod method, string endpoint)
        {
            var request = new HttpRequestMessage(method, endpoint);
            var token = await _config.AccessToken();
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
           
            Console.WriteLine($"Request URL: {_httpClient.BaseAddress}{endpoint}");
            Console.WriteLine($"Authorization Header: Bearer {token}");
            return request;
        }

        public async Task<UsersResponse> ListUserAsync(
            int? top = null,
            int? skip = null,
            string search = null,
            string filter = null,
            bool? count = null,
            string orderby = null,
            HashSet<string> select = null,
            string expand = null)
        {
            var queryParameters = new List<string>();

            if (top.HasValue)
                queryParameters.Add($"$top={top.Value}");
            if (skip.HasValue)
                queryParameters.Add($"$skip={skip.Value}");
            if (!string.IsNullOrEmpty(search))
                queryParameters.Add($"$search={Uri.EscapeDataString(search)}");
            if (!string.IsNullOrEmpty(filter))
                queryParameters.Add($"$filter={Uri.EscapeDataString(filter)}");
            if (count.HasValue)
                queryParameters.Add($"$count={count.Value.ToString().ToLower()}");
            if (!string.IsNullOrEmpty(orderby))
                queryParameters.Add($"$orderby={Uri.EscapeDataString(orderby)}");
            if (select != null && select.Count > 0)
                queryParameters.Add($"$select={Uri.EscapeDataString(string.Join(",", select))}");
            if (!string.IsNullOrEmpty(expand))
                queryParameters.Add($"$expand={Uri.EscapeDataString(expand)}");

            var queryString = string.Join("&", queryParameters);
            var endpoint = $"Users?{queryString}";

            var request = await CreateRequestAsync(HttpMethod.Get, endpoint);

            var response = await _httpClient.SendAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"404 Not Found. Response Content: {errorContent}");
            }
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var usersResponse = JsonSerializer.Deserialize<UsersResponse>(jsonResponse, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (usersResponse == null)
            {
                throw new Exception("Failed to deserialize users response.");
            }

            return usersResponse;
        }

        public async Task SendWelcomeEmailAsync(int id)
        {
            var endpoint = $"Users({id})/Pbx.SendWelcomeEmail";
            var request = await CreateRequestAsync(HttpMethod.Post, endpoint);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }
        }
    }