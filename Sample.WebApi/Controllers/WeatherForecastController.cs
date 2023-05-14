using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Sample.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IDynamoDBContext _dynamoDbContext;

        public WeatherForecastController(
            ILogger<WeatherForecastController> logger,
            IDynamoDBContext dynamoDbContext)
        {
            _logger = logger;
            _dynamoDbContext = dynamoDbContext;
        }

        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> Get(string city = "Melbourne")
        {
            return await _dynamoDbContext
                .QueryAsync<WeatherForecast>(city)
                .GetRemainingAsync();
        }

        private static IEnumerable<WeatherForecast> GenerateDummyWeatherForecast(string city)
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    City = city,
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = rng.Next(-20, 55),
                    Summary = Summaries[rng.Next(Summaries.Length)]
                })
                .ToArray();
        }
    }
}
