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

// �]�wHSTS
builder.Services.AddHsts(options =>
{
    options.Preload = true;
    // �]�t�l����
    options.IncludeSubDomains = true;
    // �ưʮɶ���30��
    options.MaxAge = TimeSpan.FromSeconds(30);
});

// �]�w����}��port ��443�A�]launchSettings �]�w��7051
// builder.Services.AddHttpsRedirection(options => options.HttpsPort = 443);

// �ϥ�Kestrel�����A��
builder.WebHost.UseKestrel(kestrel =>
{
    // ��ť80 port
    kestrel.Listen(IPAddress.Any, 80);
    // ��ť443 port => ��ϥΪ�domain��Jartech.com�åBŪ����ñ����
    kestrel.Listen(IPAddress.Any, 443, listener => listener.UseHttps(
       https => https.ServerCertificateSelector = SelelctCertificate));
});

var app = builder.Build();

// �Q�� pipeline �[�� request �H�� response
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

