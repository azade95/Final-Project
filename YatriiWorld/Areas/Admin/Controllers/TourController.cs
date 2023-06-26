using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YatriiWorld.DAL;
using YatriiWorld.Models;
using YatriiWorld.Utilities.Extensions;
using YatriiWorld.ViewModels;

namespace YatriiWorld.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AutoValidateAntiforgeryToken]
    public class TourController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;
        private readonly UserManager<AppUser> _userManager;

        public TourController(AppDbContext context, IMapper mapper, IWebHostEnvironment env, UserManager<AppUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _env = env;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            List<Tour> tours = await _context.Tours.ToListAsync();
            return View(tours);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateTourVM tourVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (!tourVM.Photo.CheckFileType("image/"))
            {
                ModelState.AddModelError("Photo", "File type is not correct!");
                return View();
            }
            if (!tourVM.Photo.CheckFileSize(2048))
            {
                ModelState.AddModelError("Photo", "File size must be less than 2Mb!");
                return View();
            }
            tourVM.UserId = _userManager.GetUserId(HttpContext.User);
            

            Tour tour = _mapper.Map<Tour>(tourVM);
            tour.Image = await tourVM.Photo.CreateFileAsync(_env.WebRootPath, "assets/images/tour");
            _context.Tours.Add(tour);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    } 
}
