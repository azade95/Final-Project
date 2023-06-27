using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using YatriiWorld.Models;

namespace YatriiWorld.Controllers
{
    public class HomeController : Controller
    {
        
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ErrorPage(string errorMessage="Xeta bash verdi")
        {
            return View(model: errorMessage);

        }

    }
}