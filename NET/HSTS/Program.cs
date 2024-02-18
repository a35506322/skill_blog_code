using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using System.Net;
using System.Security.Cryptography.X509Certificates;

static X509Certificate2? SelelctCertificate(ConnectionContext? context, string? domain)
    => domain?.ToLowerInvariant() switch
    {
        "artech.com" => CertificateLoader.LoadFromStoreCert("artech.com", "My", StoreLocation.CurrentUser, true),
        _ => throw new InvalidOperationException($"Invalid domain '{domain}'.")
    };

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 設定HSTS
builder.Services.AddHsts(options =>
{
    options.Preload = true;
    // 包含子網域
    options.IncludeSubDomains = true;
    // 滑動時間為30秒
    options.MaxAge = TimeSpan.FromSeconds(30);
});

// 設定當轉址時port 為443，因launchSettings 設定為7051
// builder.Services.AddHttpsRedirection(options => options.HttpsPort = 443);

// 使用Kestrel為伺服器
builder.WebHost.UseKestrel(kestrel =>
{
    // 監聽80 port
    kestrel.Listen(IPAddress.Any, 80);
    // 監聽443 port => 當使用者domain輸入artech.com並且讀取自簽憑證
    kestrel.Listen(IPAddress.Any, 443, listener => listener.UseHttps(
       https => https.ServerCertificateSelector = SelelctCertificate));
});

var app = builder.Build();

// 利用 pipeline 觀察 request 以及 response
app.Use(async (context, next) =>
{
    Console.WriteLine($"request-  request url : {context.Response.HttpContext.Request.GetDisplayUrl()}");
    await next.Invoke();
    Console.WriteLine($"response- response location : {context.Response.Headers.Location} | status code {context.Response.StatusCode} | strict-transport-security {context.Response.Headers.StrictTransportSecurity}");
});

app.UseHsts();
// app.UseHttpsRedirection();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

