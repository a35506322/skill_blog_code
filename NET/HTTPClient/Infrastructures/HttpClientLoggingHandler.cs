using Microsoft.AspNetCore.Http;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HTTPClient.Infrastructures;

public class HttpClientLoggingHandler : DelegatingHandler
{
    private ILogger<HttpClientLoggingHandler> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpClientLoggingHandler(ILogger<HttpClientLoggingHandler> logger,
        IHttpContextAccessor httpContextAccessor) : base(new HttpClientHandler())
    {
        this._logger = logger;
        this._httpContextAccessor = httpContextAccessor;
    }

    public HttpClientLoggingHandler(HttpMessageHandler handler,
        ILogger<HttpClientLoggingHandler> logger,
        IHttpContextAccessor httpContextAccessor) : base(handler)
    {
        this._logger = logger;
        this._httpContextAccessor = httpContextAccessor;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // Log from
        _logger.LogInformation("From: {0}", this._httpContextAccessor.HttpContext.Request.Host);

        // Log request method and URL
        _logger.LogInformation("Request: {0} {1} {2}", request.Method, request.RequestUri, request.RequestUri?.Query);

        // Log headers
        var defaultHeaders = request.Headers
        .Concat(request.Headers ?? Enumerable.Empty<KeyValuePair<string, IEnumerable<string>>>())
        .ToDictionary(h => h.Key, h => h.Value);


        _logger.LogInformation("Default Headers: {0}", JsonSerializer.Serialize(defaultHeaders));

        if (request.Content is not null)
        {
            var headers = request.Content.Headers
                .Concat(request.Content?.Headers ?? Enumerable.Empty<KeyValuePair<string, IEnumerable<string>>>())
                .ToDictionary(h => h.Key, h => h.Value);

            _logger.LogInformation("Headers: {0}", JsonSerializer.Serialize(headers));
        }

        // Show request payload
        if (request.Content != null)
        {
            string payload = await request.Content.ReadAsStringAsync();
            _logger.LogInformation("Payload: {0}", payload);
        }

        // Send the request and get the response
        HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

        // Log response status
        _logger.LogInformation("Response: {0} ({1})", response.StatusCode, ((int)response.StatusCode));

        // Log response headers
        var repHeaders = response.Headers
                .Concat(response.Content?.Headers ?? Enumerable.Empty<KeyValuePair<string, IEnumerable<string>>>())
                .ToDictionary(h => h.Key, h => h.Value);

        _logger.LogInformation("Response Headers: {0}", JsonSerializer.Serialize(repHeaders));

        // Log response
        string jsonText = await response.Content.ReadAsStringAsync();
        _logger.LogInformation("Response Body:\n{0}", jsonText.Substring(0, 100));

        return response;
    }
}
