using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
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
                .QueryAsync<WeatherForecast>(city, QueryOperator.Between,
                    new object[] { DateTime.Now.AddDays(-30), DateTime.Now })
                .GetRemainingAsync();
        }

        [HttpPost]
        public async Task Post(string city)
        {
            // var data = GenerateDummyWeatherForecast(city);
            // foreach (var item in data)
            // {
            //     await _dynamoDbContext.SaveAsync(item);
            // }

            var specificItem = await _dynamoDbContext.LoadAsync<WeatherForecast>(city, DateTime.Now.Date.AddDays(1));
            specificItem.Summary = "Warm";
            await _dynamoDbContext.SaveAsync(specificItem);
        }

        [HttpDelete]
        public async Task Delete(string city)
        {
            var specificItem =await _dynamoDbContext.LoadAsync<WeatherForecast>(city, DateTime.Now.Date.AddDays(1));
            // await _dynamoDbContext.DeleteAsync<WeatherForecast>(city, DateTime.Now.Date.AddDays(1));
            await _dynamoDbContext.DeleteAsync(specificItem);
        }

        private static IEnumerable<WeatherForecast> GenerateDummyWeatherForecast(string city)
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    City = city,
                    Date = DateTime.Now.Date.AddDays(index),
                    TemperatureC = rng.Next(-20, 55),
                    Summary = Summaries[rng.Next(Summaries.Length)]
                })
                .ToArray();
        }
    }
}
