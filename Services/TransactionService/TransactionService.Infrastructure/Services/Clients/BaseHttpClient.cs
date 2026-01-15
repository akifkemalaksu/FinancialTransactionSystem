using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;
using TransactionService.Domain.Constants;

namespace TransactionService.Infrastructure.Services.Clients
{
    public class BaseHttpClient : HttpClient
    {
        private readonly HttpClient _httpClient;
        protected readonly ILogger<BaseHttpClient> _logger;

        public BaseHttpClient(
            HttpClient httpClient,
            ILogger<BaseHttpClient> logger
        )
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        protected void SetBaseAddress(string url)
        {
            _httpClient.BaseAddress = new Uri(url);
        }

        protected async Task<TResponse> SendAsync<TResponse>(
        HttpMethod method,
        string url,
        string contentType = ContentTypes.Json,
        Func<HttpContent, Task<TResponse>>? customDeserializer = null,
        Dictionary<string, string>? headers = null,
        CancellationToken cancellationToken = default)
        {
            try
            {
                var request = new HttpRequestMessage(method, url);

                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        request.Headers.TryAddWithoutValidation(header.Key, header.Value);
                    }
                }

                _logger.LogInformation("Sending {Method} request to {Url}", method, url);

                using var response = await _httpClient.SendAsync(request, cancellationToken);
                string responseBody = await response.Content.ReadAsStringAsync(cancellationToken) ?? string.Empty;

                _logger.LogInformation("Received HTTP response {StatusCode} from {Url}", response.StatusCode, url);
                _logger.LogDebug("HTTP response body from {Url}: {Body}", url, responseBody);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning(
                        "HTTP request failed with status code {StatusCode} to {Url}. Response body: {Body}",
                        response.StatusCode,
                        url,
                        responseBody
                    );
                    response.EnsureSuccessStatusCode();
                }

                return await DeserializeResponseAsync(responseBody, response.Content, contentType, customDeserializer);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while sending HTTP request {Method} {Url}",
                    method,
                    url
                );
                throw;
            }
        }


        protected async Task<TResponse> SendAsync<TRequest, TResponse>(
        HttpMethod method,
        string url,
        TRequest? content = default,
        string contentType = ContentTypes.Json,
        Func<HttpContent, Task<TResponse>>? customDeserializer = null,
        Dictionary<string, string>? headers = null,
        CancellationToken cancellationToken = default)
        {
            try
            {
                var request = CreateRequest(method, url, content, contentType, headers);

                _logger.LogInformation("Sending {Method} request to {Url}", method, url);

                using var response = await _httpClient.SendAsync(request, cancellationToken);
                string responseBody = await response.Content.ReadAsStringAsync(cancellationToken) ?? string.Empty;

                _logger.LogInformation("Received HTTP response {StatusCode} from {Url}", response.StatusCode, url);
                _logger.LogDebug("HTTP response body from {Url}: {Body}", url, responseBody);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning(
                        "HTTP request failed with status code {StatusCode} to {Url}. Response body: {Body}",
                        response.StatusCode,
                        url,
                        responseBody
                    );
                    response.EnsureSuccessStatusCode();
                }

                return await DeserializeResponseAsync(responseBody, response.Content, contentType, customDeserializer);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while sending HTTP request with content {Method} {Url}",
                    method,
                    url
                );
                throw;
            }
        }

        private HttpRequestMessage CreateRequest<TRequest>(
            HttpMethod method, string url, TRequest? content, string contentType,
            Dictionary<string, string>? headers)
        {
            var request = new HttpRequestMessage(method, url);

            if (headers != null)
            {
                foreach (var header in headers)
                    request.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            if (content != null)
            {
                if (contentType == ContentTypes.Json)
                {
                    string requestBodyStr = JsonSerializer.Serialize(content);
                    request.Content = new StringContent(requestBodyStr, Encoding.UTF8, contentType);
                }
                else if (content is string strContent)
                {
                    request.Content = new StringContent(strContent, Encoding.UTF8, contentType);
                }
            }

            return request;
        }

        private static async Task<TResponse> DeserializeResponseAsync<TResponse>(
            string responseBody, HttpContent responseContent, string contentType,
            Func<HttpContent, Task<TResponse>>? customDeserializer)
        {
            if (customDeserializer != null)
                return await customDeserializer(responseContent);

            if (typeof(TResponse) == typeof(string))
                return (TResponse)(object)(responseBody ?? string.Empty);

            if (string.IsNullOrEmpty(responseBody))
                return default!;

            if (contentType == ContentTypes.Json)
            {
                return JsonSerializer.Deserialize<TResponse>(
                    responseBody,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
            }

            throw new NotSupportedException("Unknown content type for deserialization.");
        }
    }
}
