using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace _3CX_API_20.Services
{
    public class CustomPromptsServices
    {
        private readonly ApiServices _apiService;
        private readonly HttpClient _httpClient;
        public CustomPromptsServices(ApiServices apiService, HttpClient httpClient)
        {
            _apiService = apiService;
            _httpClient = httpClient;
        }

        public async Task<string> UploadCustomPromptAsync(string filePath)
        {
            var endpoint = "customPrompts";

            try
            {
                if (!File.Exists(filePath))
                {
                    throw new FileNotFoundException($"File not exist: {filePath}");
                }

                var request = await _apiService.CreateRequestAsync(HttpMethod.Post, endpoint);

                using (var multipartFormContent = new MultipartFormDataContent())
                {
                    using (var fileStream = File.OpenRead(filePath))
                    {
                        var fileContent = new StreamContent(fileStream);
                        fileContent.Headers.ContentType = new MediaTypeHeaderValue("audio/wav");

                        multipartFormContent.Add(fileContent, "file", Path.GetFileName(filePath));

                        request.Content = multipartFormContent;

                        Console.WriteLine($"Request URL: {_httpClient.BaseAddress}{endpoint}");

                        HttpResponseMessage response = await _httpClient.SendAsync(request);

                        if (response.IsSuccessStatusCode)
                        {
                          Console.WriteLine($"StatusCode {response.StatusCode.ToString()}");

                            return "Done";
                        }
                        else
                        {
                            Console.WriteLine($"Error StatusCode {response.StatusCode.ToString()}");
                            return "Error";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while uploading file");
                Console.WriteLine(ex.Message);
                throw;
            }
        }




    }
}
