
using Microsoft.AspNetCore.Mvc;
using MovieTR.Data;
using MovieTR.Models;
using System.Diagnostics;

namespace MovieTR.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        
        public IActionResult Index()
        {
            

            return View();
        }
        

        public IActionResult HomeView()
        { 
            
            return View("HomeView");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}