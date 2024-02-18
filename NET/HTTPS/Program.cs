using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Http.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 可搭配環境變數去做設定
if (!builder.Environment.IsDevelopment())
{
    // 設定當轉址時port 為443
    builder.Services.AddHttpsRedirection(options => options.HttpsPort = 443);
}

var app = builder.Build();

// 利用 pipeline 觀察 request 以及 response
app.Use(async (context, next) =>
{
    Console.WriteLine($"request-  request url : {context.Response.HttpContext.Request.GetDisplayUrl()}");
    await next.Invoke();
    Console.WriteLine($"response- response location : {context.Response.Headers.Location} | status code {context.Response.StatusCode}");
});

app.UseHttpsRedirection();

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

