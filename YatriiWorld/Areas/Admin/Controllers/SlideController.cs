using AutoMapper;
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
    public class SlideController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;

        public SlideController( AppDbContext context, IMapper mapper, IWebHostEnvironment env)
        {
            _context = context;
            _mapper = mapper;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<Slide> slides = await _context.Slides.ToListAsync();
            return View(slides);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateSlideVM slideVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (!slideVM.Photo.CheckFileType("image/"))
            {
                ModelState.AddModelError("Photo", "File type is not correct!");
                return View();
            }
            if (!slideVM.Photo.CheckFileSize(2048))
            {
                ModelState.AddModelError("Photo", "File size must be less than 2Mb!");
                return View();
            }

            Slide slide= _mapper.Map<Slide>(slideVM);
            slide.Image =await slideVM.Photo.CreateFileAsync(_env.WebRootPath, "assets/images/slider/");
            await _context.Slides.AddAsync(slide);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return BadRequest();
            Slide slide= await _context.Slides.FirstOrDefaultAsync(s=>s.Id == id);
            if (slide == null) return NotFound();
            UpdateSlideVM slideVM = _mapper.Map<UpdateSlideVM>(slide);
            return View(slideVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id, UpdateSlideVM slideVM)
        {
            if (id == null) return BadRequest();
            Slide slide = await _context.Slides.FirstOrDefaultAsync(s => s.Id == id);
            if (slide == null) return NotFound();
            if (!ModelState.IsValid)
            {
                return View();
            }
            if(slideVM.Photo!=null)
            {
                if (!slideVM.Photo.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Photo", "File type is not correct!");
                    return View();
                }
                if (!slideVM.Photo.CheckFileSize(2048))
                {
                    ModelState.AddModelError("Photo", "File size must be less than 2Mb!");
                    return View();
                }
                slideVM.Image.DeleteFile(_env.WebRootPath, "assets/images/slider/");
                slideVM.Image =await slideVM.Photo.CreateFileAsync(_env.WebRootPath, "assets/images/slider/");
            }
            slide = _mapper.Map(slideVM,slide);
            _context.Slides.Update(slide);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");

        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();
            Slide slide = await _context.Slides.FirstOrDefaultAsync(s => s.Id == id);
            if (slide == null) return NotFound();

            slide.Image.DeleteFile(_env.WebRootPath, "assets/images/slider/");
            _context.Slides.Remove(slide);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

    }
}
