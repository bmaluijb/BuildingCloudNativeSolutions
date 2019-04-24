using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace NationalCookies.Controllers
{
    public class HomeController : Controller
    {

        public async Task<IActionResult> Index()
        {
            using (var client = new System.Net.Http.HttpClient())
            {
                var request = new System.Net.Http.HttpRequestMessage();
                request.RequestUri = new Uri("http://nationalcookies.api/api/values/");
                var response = await client.SendAsync(request);
                ViewData["Message"] += "The current time is: " + await response.Content.ReadAsStringAsync();
            }

            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Error()
        {          
            return View();
        }


    }
}
