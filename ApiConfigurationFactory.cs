using _3CX_API_20.Models;
using _3CX_API_20.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3CX_API_20
{
    public class ApiConfigurationFactory
    {
        private readonly string _basePath;
        private readonly string _username;
        private readonly string _password;
        private TokenResponse _accessTokenResponse;
        private long _expires;
        private readonly SemaphoreSlim _semaphore = new(1, 1);


        public ApiConfigurationFactory(string basePath, string username, string password)
        {
            _basePath = basePath;
            _username = username;
            _password = password;
        }

        public ApiConfiguration CreateXAPIConfiguration()
        {
            return new ApiConfiguration
            {
                BasePath = $"{_basePath}/xapi/v1/",
                AccessToken = GetAccessTokenAsync
            };
        }

        private async Task<string> GetAccessTokenAsync()
        {
            await _semaphore.WaitAsync();
            try
            {
                var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                if (_accessTokenResponse == null || now > _expires)
                {
                    _accessTokenResponse = await TokenService.GetAccessTokenAsync(_basePath, _username, _password);
                    _expires = now + (_accessTokenResponse.ExpiresIn * 60 * 1000); 
                }
                return _accessTokenResponse.AccessToken;
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}
