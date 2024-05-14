using HTTPClient.Entites;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HTTPClient.Infrastructures;

public class HttpClientLoggingHandler : DelegatingHandler
{
    private ILogger<HttpClientLoggingHandler> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly TodoContext _todoContext;

    public HttpClientLoggingHandler(ILogger<HttpClientLoggingHandler> logger,
        IHttpContextAccessor httpContextAccessor,
        TodoContext todoContext) : base(new HttpClientHandler())
    {
        this._logger = logger;
        this._httpContextAccessor = httpContextAccessor;
        this._todoContext = todoContext;
    }

    public HttpClientLoggingHandler(HttpMessageHandler handler,
        ILogger<HttpClientLoggingHandler> logger,
        IHttpContextAccessor httpContextAccessor,
        TodoContext todoContext) : base(handler)
    {
        this._logger = logger;
        this._httpContextAccessor = httpContextAccessor;
        this._todoContext = todoContext;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // Log from
        _logger.LogInformation("From: {0}", this._httpContextAccessor.HttpContext.Request.Host);

        // Log request method and URL
        _logger.LogInformation("Request: {0} {1} {2}", request.Method, request.RequestUri, request.RequestUri?.Query);

        // Log headers
        var defaultHeadersDic = request.Headers
        .Concat(request.Headers ?? Enumerable.Empty<KeyValuePair<string, IEnumerable<string>>>())
        .ToDictionary(h => h.Key, h => h.Value);

        string defaultHeaders = JsonSerializer.Serialize(defaultHeadersDic);

        _logger.LogInformation("Default Headers: {0}", JsonSerializer.Serialize(defaultHeaders));


        string? headers = null;
        if (request.Content is not null)
        {
            var headersDic = request.Content.Headers
                .Concat(request.Content?.Headers ?? Enumerable.Empty<KeyValuePair<string, IEnumerable<string>>>())
                .ToDictionary(h => h.Key, h => h.Value);

            headers = JsonSerializer.Serialize(headersDic);
            _logger.LogInformation("Headers: {0}", headers);
        }

        // Show request payload
        string? payload = null;
        if (request.Content != null)
        {
            payload = await request.Content.ReadAsStringAsync();
            _logger.LogInformation("Payload: {0}", payload);
        }

        // Send the request and get the response
        HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

        // Log response status
        _logger.LogInformation("Response: {0} ({1})", response.StatusCode, ((int)response.StatusCode));

        // Log response headers
        var repHeadersDic = response.Headers
                .Concat(response.Content?.Headers ?? Enumerable.Empty<KeyValuePair<string, IEnumerable<string>>>())
                .ToDictionary(h => h.Key, h => h.Value);
        string repHeaders = JsonSerializer.Serialize(repHeadersDic);
        _logger.LogInformation("Response Headers: {0}", repHeaders);

        // Log response
        string jsonText = await response.Content.ReadAsStringAsync();
        _logger.LogInformation("Response Body:\n{0}", jsonText.Substring(0, 100));

        // save db
        HttpClientLog httpClientLog = new HttpClientLog()
        {
            FromRequestUri = this._httpContextAccessor.HttpContext.Request.GetDisplayUrl(),
            HttpMethod = request.Method.ToString(),
            Query = request.RequestUri?.Query,
            ReuqestUri = request.RequestUri!.ToString(),
            RequestDefaultHeaders = defaultHeaders,
            RequestHeaders = headers,
            RequestBody = payload,
            ResponseStatus = ((int)response.StatusCode).ToString(),
            ResponseHeaders = repHeaders,
            ResponseBody = jsonText,
            Time = DateTime.Now
        };

        await this._todoContext.HttpClientLog.AddAsync(httpClientLog);
        await this._todoContext.SaveChangesAsync();

        return response;
    }
}
