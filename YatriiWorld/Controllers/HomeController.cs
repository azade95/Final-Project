using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using YatriiWorld.DAL;
using YatriiWorld.Models;
using YatriiWorld.ViewModels.Home;

namespace YatriiWorld.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
           
            HomeVM homeVM = new HomeVM
            {
                Tours = await _context.Tours.Include(t => t.User).ToListAsync(),
                Slides= await _context.Slides.ToListAsync(),
             };
            return View(homeVM);
        }

        public IActionResult ErrorPage(string errorMessage="Xeta bash verdi")
        {
            return View(model: errorMessage);

        }

        public async Task<IActionResult> About()
        {
            List<Employee> employees = await _context.Employees.Include(e=>e.Position).ToListAsync();
            return View(employees);
        }

    }
}