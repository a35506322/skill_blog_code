using Microsoft.AspNetCore.Mvc;
using SemaphoreSlimDemo.Models;

namespace SemaphoreSlimDemo.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly TodoContext _context;
    private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

    public WeatherForecastController(ILogger<WeatherForecastController> logger, TodoContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }

    [HttpGet]
    public async Task<IActionResult> SemaphoreTest()
    {
        try
        {
            await _semaphore.WaitAsync();

            var lastSemaphore = _context.SemaphoreSlimTest.OrderByDescending(s => s.SeqNo).FirstOrDefault();

            SemaphoreSlimTest semaphoreSlimTest = new SemaphoreSlimTest()
            {
                SeqNo = lastSemaphore == null ? 1 : lastSemaphore.SeqNo + 1,
                CreateTime = DateTime.Now
            };

            _context.SemaphoreSlimTest.Add(semaphoreSlimTest);

            await _context.SaveChangesAsync();

            return Ok(semaphoreSlimTest.SeqNo);
        }
        finally
        {
            _semaphore.Release();
        }
    }
}