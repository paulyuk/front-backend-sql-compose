using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WebFront.Models;

namespace WebFront.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        //public IActionResult Index()
        //{
        //    return View();
        //}

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> Index()
        {
            //var uri = ConfigurationManager.AppSettings["BackendURI"];  //returns null, need to figure out 3.0 way to do this
            var uri = "https://webapiback/weatherforecast";    //returns certificate error
            //var uri = "https://localhost:44383/weatherforecast"; //returns 99 Cannot assign requested error, so trying Docker Compose special local URL
            //var uri = "https://host.docker.internal:44383/weatherforecast"; //returns certificate error (do we need better properties set on self signed cert for these non localhost domains?)

            var modelList = new List<WeatherForecast>();

            using (var client = new HttpClient())
            {
                using (var response = await client.GetAsync(uri))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        modelList = JsonConvert.DeserializeObject<List<WeatherForecast>>(apiResponse);

                        return View(modelList);
                    }

                    return Error();
                }
            }
        }
    }
}
