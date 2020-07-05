using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DWDW_API.Core.Constants;
using DWDW_API.Providers;
using DWDW_Service.Repositories;
using DWDW_Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

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

        [HttpGet]
        [Route("TestNotify")]
        public IActionResult TestSendNotify(string room)
        {
            IActionResult result = Ok();

            string deviceToken = "cAF8JeveS9av5pIdQtge0-:APA91bGvzkAno7ycM_fIzqwEjhIUTBy-la9u71_" +
                "vYocHFhnnuGIO0PyfAMU2ph0cae6YuRGpYTAnbw9KtcKgN-aENmED3Bz4KLHnjrpU9HgfRHhBcTBP_" +
                "gbd41-tcsMD4kC9Vl0dnHC2";
            string now = DateTime.Now.ToString("H:mm");

            string titleText = "Detect drowsiness!";
            string bodyText = "There was a drowsiness in " + room  + " at " + now;
            var data = new
            {
                notification = new
                {
                    title = titleText,
                    body = bodyText
                },
                to = deviceToken
            };
            string xc = JsonConvert.SerializeObject(data);
            byte[] byteArray = Encoding.UTF8.GetBytes(xc);
            TestService testService = new TestService();
            testService.SendNotification(byteArray);
            return result;
        }

    }
}
