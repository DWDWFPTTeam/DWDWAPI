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

        private readonly IUserService userService;
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public TestController(ExtensionSettings extensionSettings, IUserService userService) : base(extensionSettings)
        {
            this.userService = userService;
        }

        [HttpGet]
        [Authorize(Roles = Constant.ADMIN)]
        public IEnumerable<WeatherForecast> Get(int number1, int number2)
        {
            userService.DeactiveOverdue();
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

            string deviceToken = "dMr1J7MRSZWVtHmM4B3Fbb:APA91bH9UeX9X9ExRyBvi3clNiUlfO6lTGG8L8oS2o6zXHWpMDXFj8AmKx95Sc_hFiPejsVKx_i4sXkHIAAgXeZxWn61EWqbEu-QdL0ZjDli1gDPppTnUjo4Vt1ySDw7iQE0Vv6Imve3";
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
