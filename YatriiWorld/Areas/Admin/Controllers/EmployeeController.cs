using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YatriiWorld.DAL;
using YatriiWorld.Models;
using YatriiWorld.Utilities.Extensions;
using YatriiWorld.ViewModels;

namespace YatriiWorld.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AutoValidateAntiforgeryToken]
    [Authorize(Roles = $"Admin")]
    public class EmployeeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public EmployeeController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {
            List<Employee> employees = _context.Employees.ToList();
           
            return View(employees);
        }

        public IActionResult Create()
        {
            ViewBag.Positions = _context.Positions.ToList();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateEmployeeVM employeeVM)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Positions = _context.Positions.ToList();
                return View();
            }
            if (!employeeVM.Photo.CheckFileType("image/"))
            {
                ModelState.AddModelError("Photo", "File tipi dogru deyil");
                ViewBag.Positions = _context.Positions.ToList();
                return View();
            }
            if (!employeeVM.Photo.CheckFileSize(2048))
            {
                ModelState.AddModelError("Photo", "File olchusu 2mbdan chox olmamalidir");
                ViewBag.Positions = _context.Positions.ToList();
                return View();
            }
            Employee employee = new Employee
            {
                Name = employeeVM.Name,
                Surname = employeeVM.Surname,
                PositionId = employeeVM.PositionId,
                Image = await employeeVM.Photo.CreateFileAsync(_env.WebRootPath, "assets/images/team")
            };

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public IActionResult Update(int? id)
        {
            if (id == null || id < 1) return BadRequest();
            Employee existed = _context.Employees.FirstOrDefault(e => e.Id == id);
            if (existed == null) return NotFound();

            UpdateEmployeeVM employeeVM = new UpdateEmployeeVM
            {
                Name = existed.Name,
                Surname = existed.Surname,
                Image = existed.Image,
                PositionId = existed.PositionId,
            };
            ViewBag.Positions = _context.Positions.ToList();
            return View(employeeVM);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int? id, UpdateEmployeeVM employeeVM)
        {
            if (id == null || id < 1) return BadRequest();
            Employee existed = _context.Employees.FirstOrDefault(e => e.Id == id);
            if (existed == null) return NotFound();

            if (!ModelState.IsValid)
            {
                ViewBag.Positions = _context.Positions.ToList();
                return View();
            }
            if (employeeVM.Photo != null)
            {
                if (!employeeVM.Photo.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Photo", "File tipi dogru deyil");
                    ViewBag.Positions = _context.Positions.ToList();
                    return View();
                }
                if (!employeeVM.Photo.CheckFileSize(2048))
                {
                    ModelState.AddModelError("Photo", "File olchusu 2mbdan chox olmamalidir");
                    ViewBag.Positions = _context.Positions.ToList();
                    return View();
                }

                existed.Image.DeleteFile(_env.WebRootPath, "assets/img/team/");
                existed.Image = await employeeVM.Photo.CreateFileAsync(_env.WebRootPath, "assets/images/team");

            }
            existed.Name = employeeVM.Name;
            existed.Surname = employeeVM.Surname;
            existed.PositionId = employeeVM.PositionId;

            _context.Employees.Update(existed);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id < 1) return BadRequest();
            Employee existed = _context.Employees.FirstOrDefault(e => e.Id == id);
            if (existed == null) return NotFound();

            existed.Image.DeleteFile(_env.WebRootPath, "assets/images/team");
            _context.Employees.Remove(existed);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
