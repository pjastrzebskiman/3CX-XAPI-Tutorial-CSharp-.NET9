using _3CX_API_20.Helpers;
using _3CX_API_20.Models;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace _3CX_API_20.Services
{
    public class GroupsServices
    {
        private readonly ApiServices _apiService;
        private readonly HttpClient _httpClient;

        public GroupsServices(ApiServices apiService, HttpClient httpClient)
        {
            _apiService = apiService;
            _httpClient = httpClient;
        }

        public async Task<GroupsResponse> ListGroupsAsync(
        int? top = null,
        int? skip = null,
        string search = null,
        string filter = null,
        bool? count = null,
        HashSet<string> orderby = null,
        HashSet<string> select = null,
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
            if (orderby != null && orderby.Count > 0)
                queryParameters.Add($"$orderby={Uri.EscapeDataString(string.Join(",", orderby))}");
            if (select != null && select.Count > 0)
                queryParameters.Add($"$select={Uri.EscapeDataString(string.Join(",", select))}");
            if (expand != null && expand.Count > 0)
                queryParameters.Add($"$expand={Uri.EscapeDataString(string.Join(",", expand))}");

           
            var queryString = string.Join("&", queryParameters);

           
            var endpoint = string.IsNullOrEmpty(queryString) ? "Groups" : $"Groups?{queryString}";

           
            var request = await _apiService.CreateRequestAsync(HttpMethod.Get, endpoint);

            Console.WriteLine($"Request URL: {_httpClient.BaseAddress}{endpoint}");


            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var groupsResponse = JsonSerializer.Deserialize<GroupsResponse>(
                jsonResponse,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            if (groupsResponse == null)
            {
                throw new Exception("Failed to deserialize groups response.");
            }

            return groupsResponse;
        }

        public async Task<GroupsResponse> ListGroupAsync(
           int id,
           int? top = null,
           int? skip = null,
           string search = null,
           string filter = null,
           bool? count = null,
           HashSet<string> orderby = null,
           HashSet<string> select = null,
           HashSet<string> expand = null)
        {
            var endpoint = $"Users({id})/Groups";

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

            var queryString = queryParameters.Any()
                ? "?" + string.Join("&", queryParameters)
                : string.Empty;

            var request = await _apiService.CreateRequestAsync(HttpMethod.Get, endpoint + queryString);

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"GET {endpoint} failed with status {response.StatusCode}. Response: {errorContent}");
            }

            var json = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var result = JsonSerializer.Deserialize<GroupsResponse>(json, options);

            return result;
        }
    
            public async Task<string> ListMembersAsync(
            int id,
            int? top = null,
            int? skip = null,
            string? search = null,
            string? filter = null,
            bool? count = null,
            HashSet<string>? orderby = null,
            HashSet<string>? select = null,
            HashSet<string>? expand = null)
        {
            string endpoint = $"Groups({id})/Members";

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

            var queryString = queryParameters.Any()
                ? "?" + string.Join("&", queryParameters)
                : string.Empty;


            try
            {
                var request = await _apiService.CreateRequestAsync(HttpMethod.Get, endpoint + queryString);

                var response = await _httpClient.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"GET {endpoint} failed with status {response.StatusCode}. Response: {errorContent}");
                }

                var jsonResponse = await response.Content.ReadAsStringAsync();


                // var a =  RightsHelper.GetRightsFlagsForId(jsonResponse, 22097);

                /*   var groupsResponse = JsonSerializer.Deserialize<GroupMembers>(
                       jsonResponse,
                       new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                   );

                   if (groupsResponse == null)
                   {
                       throw new Exception("Failed to deserialize groups response.");
                   }

                   return groupsResponse;*/
                return jsonResponse;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task UpdateGroupAsync(int id, string jsonBody)
        {
            var endpoint = $"Groups({id})";
            var patchMethod = new HttpMethod("PATCH");
            var request = await _apiService.CreateRequestAsync(patchMethod, endpoint);

           // string jsonBody = JsonSerializer.Serialize(group);
            request.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();
        }
        public async Task UpdateGroupAsync(int id, GroupMembersUpdate group)
        {
            var endpoint = $"Groups({id})";
            var patchMethod = new HttpMethod("PATCH");
            var request = await _apiService.CreateRequestAsync(patchMethod, endpoint);

            string jsonBody = JsonSerializer.Serialize(group);
            request.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateGroupAsync(int id, GroupMembersUpdate_deleteOne group)
        {
            var endpoint = $"Groups({id})";
            var patchMethod = new HttpMethod("PATCH");
            var request = await _apiService.CreateRequestAsync(patchMethod, endpoint);

            string jsonBody = JsonSerializer.Serialize(group);
            request.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteAllMembersFromGroup(int id)
        {
            var endpoint = $"Groups({id})";
            var patchMethod = new HttpMethod("PATCH");
            var request = await _apiService.CreateRequestAsync(patchMethod, endpoint);
            var membersDelete = new { Members = new object[] { } };


            string jsonBody = JsonSerializer.Serialize(membersDelete);
            request.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();
        }
    }
}
