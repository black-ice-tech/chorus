using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chorus.DistributedLog.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Chorus.Samples.RestApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly IDistributedLog _distributedLog;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(IDistributedLog distributedLog, ILogger<WeatherForecastController> logger)
        {
            _distributedLog = distributedLog;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            _logger.LogInformation("Getting...");
            var rng = new Random();

            var summary = Summaries[rng.Next(Summaries.Length)];
            var bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new NumberAdded { Num = rng.Next(1000) }));

            await _distributedLog.AppendAsync("my-event", bytes);

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = summary
            })
            .ToArray();
        }
    }
}
