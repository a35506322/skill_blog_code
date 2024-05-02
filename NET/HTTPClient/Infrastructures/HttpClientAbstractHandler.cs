namespace HTTPClient.Infrastructures;

public class HttpClientAbstractHandler : DelegatingHandler
{
    private ILogger<HttpClientAbstractHandler> _logger;

    public HttpClientAbstractHandler(ILogger<HttpClientAbstractHandler> logger) : base(new HttpClientHandler())
    {
        this._logger = logger;
    }

    public HttpClientAbstractHandler(HttpMessageHandler handler, ILogger<HttpClientAbstractHandler> logger) : base(handler)
    {
        this._logger = logger;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // Log request method and URL
        _logger.LogInformation("Request: {0} {1}", request.Method, request.RequestUri);

        // Log headers
        foreach (var header in request.Headers)
        {
            Console.WriteLine("Default Headers: {0}: {1}", header.Key, string.Join(", ", header.Value));
        }

        if (request.Content is not null)
        {
            foreach (var header in request.Content.Headers)
            {
                Console.WriteLine("Content Headers: {0}: {1}", header.Key, string.Join(", ", header.Value));
            }
        }

        // Show request payload
        if (request.Content != null)
        {
            string payload = await request.Content.ReadAsStringAsync();
            _logger.LogInformation("Payload: {0}", payload);
        }

        //_logger.LogInformation();

        // Send the request and get the response
        HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

        // Log response status
        _logger.LogInformation("Response: {0} ({1})", response.StatusCode, ((int)response.StatusCode));

        // Log headers
        foreach (var header in response.Headers)
        {
            _logger.LogInformation("Headers: {0}: {1}", header.Key, string.Join(", ", header.Value));
        }

        string jsonText = await response.Content.ReadAsStringAsync();
        _logger.LogInformation("Response Body:\n{0}", jsonText);

        return response;
    }
}
