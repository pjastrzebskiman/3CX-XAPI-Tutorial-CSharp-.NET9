using _3CX_API_20.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace _3CX_API_20.Services
{
    public class QueueServices
    {
        private readonly ApiServices _apiService;
        private readonly HttpClient _httpClient;

        public QueueServices(ApiServices apiService, HttpClient httpClient)
        {
            _apiService = apiService;
            _httpClient = httpClient;
        }

        public async Task<QueuesResponse> ListQueueAsync(
           int? top = null,
           int? skip = null,
           string search = null,
           string filter = null,
           bool? count = null,
           HashSet<string> orderby = null,
           HashSet<string> select = null,
           HashSet<string> expand = null       )
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
            if (orderby != null && orderby.Count > 0)
                queryParameters.Add($"$orderby={Uri.EscapeDataString(string.Join(",", orderby))}");
            if (select != null && select.Count > 0)
                queryParameters.Add($"$select={Uri.EscapeDataString(string.Join(",", select))}");
            if (expand != null && expand.Count > 0)
                queryParameters.Add($"$expand={Uri.EscapeDataString(string.Join(",", expand))}");

            var queryString = string.Join("&", queryParameters);

            var endpoint = string.IsNullOrEmpty(queryString) ? "Queues" : $"Queues?{queryString}";

            var request = await _apiService.CreateRequestAsync(HttpMethod.Get, endpoint);
            Console.WriteLine($"Request URL: {_httpClient.BaseAddress}{endpoint}");


            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var queuesResponse = JsonSerializer.Deserialize<QueuesResponse>(jsonResponse, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (queuesResponse == null)
            {
                throw new Exception("Failed to deserialize users response.");
            }

            return queuesResponse;

        }
        public async Task DeleteAllAgentsFromQueue(int id)
        {
            var endpoint = $"Queues({id})";
            var patchMethod = new HttpMethod("PATCH");
            var request = await _apiService.CreateRequestAsync(patchMethod, endpoint);
            var membersDelete = new { Agents = new object[] { } };


            string jsonBody = JsonSerializer.Serialize(membersDelete);
            request.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();
        }

    }
}
