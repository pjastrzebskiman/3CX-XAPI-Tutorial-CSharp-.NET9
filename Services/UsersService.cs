using _3CX_API_20.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace _3CX_API_20.Services
{
    public class UsersService
    {
        private readonly ApiServices _apiService;
        private readonly HttpClient _httpClient;

        public UsersService(ApiServices apiService, HttpClient httpClient)
        {
            _httpClient = httpClient;
            _apiService = apiService;
        }

        public async Task<UsersResponse> ListUserAsync(
           int? top = null,
           int? skip = null,
           string? search = null,
           string? filter = null,
           bool? count = null,
           string? orderby = null,
           HashSet<string>? select = null,
           HashSet<string> expand = null)
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
            if (expand != null && expand.Count > 0)
                queryParameters.Add($"$expand={Uri.EscapeDataString(string.Join(",", expand))}");

            var queryString = string.Join("&", queryParameters);
            var endpoint = $"Users?{queryString}";

            var request = await _apiService.CreateRequestAsync(HttpMethod.Get, endpoint);

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
               // DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
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
            var request = await _apiService.CreateRequestAsync(HttpMethod.Post, endpoint);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateUserAsync(int id, UsersUpdate pbxUser)
        {
            var options = new JsonSerializerOptions
            {
                Converters =
                    {
                        new JsonStringEnumConverter(new LowerCaseNamingPolicy())
                    },
                WriteIndented = true
            };
            var endpoint = $"Users({id})";

            var patchMethod = new HttpMethod("PATCH");
            var request = await _apiService.CreateRequestAsync(patchMethod, endpoint);

            string jsonBody = JsonSerializer.Serialize(pbxUser,options);
            
            request.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

        }
        public async Task UpdateUserAsync(int id, UserUpdatePermissions pbxUser)
        {
            var options = new JsonSerializerOptions
            {
                Converters =
                    {
                        new JsonStringEnumConverter(new LowerCaseNamingPolicy())
                    },
                WriteIndented = true
            };
            var endpoint = $"Users({id})";

            var patchMethod = new HttpMethod("PATCH");
            var request = await _apiService.CreateRequestAsync(patchMethod, endpoint);

            string jsonBody = JsonSerializer.Serialize(pbxUser, options);

            
            
            request.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

        }
        public async Task UpdateUserAsync(int id, UsersUpdateMainDepartment pbxUser)
        {
            var options = new JsonSerializerOptions
            {
                Converters =
                    {
                        new JsonStringEnumConverter(new LowerCaseNamingPolicy())
                    },
                WriteIndented = true
            };
            var endpoint = $"Users({id})";

            var patchMethod = new HttpMethod("PATCH");
            var request = await _apiService.CreateRequestAsync(patchMethod, endpoint);

            string jsonBody = JsonSerializer.Serialize(pbxUser, options);

            request.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

        }

        public async Task<Users> GetUserByIdAsync(int id, HashSet<string> select = null, HashSet<string> expand = null)
        {
            var endpoint = $"Users({id})";

            var queryParameters = new List<string>();

            if (select != null && select.Count > 0)
            {
                queryParameters.Add($"$select={Uri.EscapeDataString(string.Join(",", select))}");
            }

            if (expand != null && expand.Count > 0)
            {
                queryParameters.Add($"$expand={Uri.EscapeDataString(string.Join(",", expand))}");
            }

            string queryString = queryParameters.Any()
                ? "?" + string.Join("&", queryParameters)
                : string.Empty;

            var request = await _apiService.CreateRequestAsync(HttpMethod.Get, endpoint + queryString);

            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"GET {endpoint} failed with status {response.StatusCode}. Error: {error}");
            }


            var json = await response.Content.ReadAsStringAsync();
            var user = JsonSerializer.Deserialize<Users>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return user;
        }
    }
}
