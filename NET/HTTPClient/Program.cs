using HTTPClient.Entites;
using HTTPClient.Infrastructures;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpContextAccessor();

builder.Services.TryAddScoped<HttpClientLoggingHandler>();
builder.Services.AddHttpClient<YouBikeHttpClientHandler>()
    .RemoveAllLoggers()
    .ConfigurePrimaryHttpMessageHandler<HttpClientLoggingHandler>();

builder.Services.AddDbContext<TodoContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Todo")));

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
