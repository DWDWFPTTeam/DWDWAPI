using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DWDW_API.Core.Constants;
using DWDW_API.Providers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DWDW_API.Controllers
{
    [Route("[controller]")]
    public class TestController : BaseController
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public TestController(ExtensionSettings extensionSettings) : base(extensionSettings)
        {
        }

        [HttpGet]
        [Authorize(Roles = Constant.ADMIN)]
        public IEnumerable<WeatherForecast> Get(int number1, int number2)
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
