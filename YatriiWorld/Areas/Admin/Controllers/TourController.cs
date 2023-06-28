using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using YatriiWorld.DAL;
using YatriiWorld.Models;
using YatriiWorld.Utilities.Exceptions;
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
            List<Tour> tours = await _context.Tours.Include(t=>t.User).ToListAsync();
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

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null || id < 1) throw new WrongRequestException("Id dogru deyil");
            Tour existed = await _context.Tours.FirstOrDefaultAsync(t => t.Id == id);
            if (existed == null) throw new NotFoundException("Tour is not found!");
            UpdateTourVM tourVM = _mapper.Map<UpdateTourVM>(existed);
            return View(tourVM);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int? id, UpdateTourVM tourVM)
        {
            if (id == null || id < 1) throw new WrongRequestException("Id dogru deyil");
            Tour existed = await _context.Tours.FirstOrDefaultAsync(t => t.Id == id);
            if (existed == null) throw new NotFoundException("Tour is not found!");

            if (!ModelState.IsValid)
            {
                return View();
            }
            if(tourVM.Photo != null)
             {
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
                existed.Image.DeleteFile(_env.WebRootPath, "assets/images/tour");
                tourVM.Image = await tourVM.Photo.CreateFileAsync(_env.WebRootPath, "assets/images/tour");
             }
            existed = _mapper.Map(tourVM, existed);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id < 1) throw new WrongRequestException("Id dogru deyil");
            Tour existed = await _context.Tours.FirstOrDefaultAsync(t => t.Id == id);
            if (existed == null) throw new NotFoundException("Tour is not found!");
            existed.Image.DeleteFile(_env.WebRootPath, "assets/images/tour");
            _context.Tours.Remove(existed);
            await _context.SaveChangesAsync(); 
            return RedirectToAction(nameof(Index));
        }

    } 
}
