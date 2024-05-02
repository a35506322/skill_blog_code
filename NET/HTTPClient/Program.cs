using HTTPClient.Infrastructures;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.TryAddScoped<HttpClientAbstractHandler>();
builder.Services.AddHttpClient("youbike").ConfigurePrimaryHttpMessageHandler<HttpClientAbstractHandler>();
builder.Services.AddScoped<YouBikeHttpClientHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/GetYouBikeInfo", async ([FromServices] YouBikeHttpClientHandler youBikeHttp) =>
TypedResults.Ok(await youBikeHttp.GetYouBikeAsync()))
.WithName("GetYouBikeInfo")
.WithOpenApi();

app.Run();