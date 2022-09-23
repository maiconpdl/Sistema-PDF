using Microsoft.AspNetCore.Mvc;
using SimplePDF.Models;
using System.Diagnostics;
using System.IO;


namespace SimplePDF.Controllers
{
    public class HomeController : Controller
    {
        
        private readonly ILogger<HomeController> _logger;
        public IWebHostEnvironment hostEnv;
        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment env)
        {
            _logger = logger;
            hostEnv = env;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult JuntarPDF()
        {
            return View();
        }
        public IActionResult DividirPDF()
        {
            Dividir.nomeDownload = "";
            return View();
        }
       

    }
}