using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ApiIntegration.Core
{
    public interface IApiService
    {
        Task<TResponse?> SendRequestAsync<TResponse>(
            HttpMethod method,
            string url,
            object? requestBody = null,
            Dictionary<string, string>? headers = null,
            CancellationToken cancellationToken = default);
    }


    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ApiService> _logger;
        private readonly JsonSerializerOptions _jsonOptions;

        public ApiService(HttpClient httpClient,ILogger<ApiService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;

            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

           
        }

        public async Task<TResponse?> SendRequestAsync<TResponse>(
            HttpMethod method,
            string url,
            object? requestBody = null,
            Dictionary<string, string>? headers = null,
            CancellationToken cancellationToken = default)
        {
            try
            {
                using var request = new HttpRequestMessage(method, url);

                // Add JSON content if needed
                if (requestBody != null && method != HttpMethod.Get && method != HttpMethod.Delete)
                {
                    request.Content = new StringContent(
                        JsonSerializer.Serialize(requestBody, _jsonOptions),
                        Encoding.UTF8,
                        "application/json"
                    );
                }

                // Add custom headers
                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        request.Headers.TryAddWithoutValidation(header.Key, header.Value);
                    }
                }

                using var response = await _httpClient.SendAsync(request, cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync(cancellationToken);
                    _logger.LogError($"Request failed: {response.StatusCode}, {error}");
                    return default;
                }

                if (response.Content.Headers.ContentLength == 0)
                    return default;

                var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
                return await JsonSerializer.DeserializeAsync<TResponse>(responseStream, _jsonOptions, cancellationToken);
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }
    }






}
