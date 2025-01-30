using _3CX_API_20.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace _3CX_API_20.Services
{
    public static class TokenService
    {
        private static readonly HttpClient HttpClient = new HttpClient();

        public static async Task<TokenResponse> GetAccessTokenAsync(string basePath, string username, string password)
        {
            var formParams = new Dictionary<string, string>
        {
            { "client_id", username },
            { "client_secret", password },
            { "grant_type", "client_credentials" }
        };
            using var content = new FormUrlEncodedContent(formParams);
            var requestUri = $"{basePath}/connect/token";

            
            var response = await HttpClient.PostAsync(requestUri, content);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Refresh failed with status code {response.StatusCode}");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();

            var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(jsonResponse, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (tokenResponse == null)
            {
                throw new Exception("Failed to deserialize token response.");
            }

            return tokenResponse;
        }
    }



}
