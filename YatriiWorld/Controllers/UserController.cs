using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YatriiWorld.DAL;
using YatriiWorld.Models;
using YatriiWorld.Utilities.Extensions;
using YatriiWorld.ViewModels;

namespace YatriiWorld.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public UserController(UserManager<AppUser> userManager,IMapper mapper,AppDbContext context,IWebHostEnvironment env)
        {
            _userManager = userManager;
            _mapper = mapper;
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Profile()
        {
            AppUser user=await _userManager.GetUserAsync(HttpContext.User);
            UserVM userVM= new UserVM
            {
                Name= user.Name,
                Surname= user.Surname,
                Username= user.UserName,
                Email= user.Email,
                Gender= user.Gender,
                Phone= user.Phone,
            };
            return View(userVM);
        }
        [HttpPost]
        public async Task<IActionResult> Profile(UserVM userVM)
        {
           AppUser user = await _userManager.GetUserAsync(HttpContext.User);
            user.Name = userVM.Name;
            user.Surname = userVM.Surname;
            user.Email = userVM.Email;
            user.Gender = userVM.Gender;
            user.Phone = userVM.Phone;
            user.UserName= userVM.Username;
            
            if (userVM.Photo != null)
            {
                if (!userVM.Photo.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Photo", "File type is not correct!");
                    return View();
                }
                if (!userVM.Photo.CheckFileSize(2048))
                {
                    ModelState.AddModelError("Photo", "File size must be less than 2Mb!");
                    return View();
                }
                if(user.Image!=null)
                {
                    user.Image.DeleteFile(_env.WebRootPath, "assets/images/user/");
                }
                user.Image = await userVM.Photo.CreateFileAsync(_env.WebRootPath, "assets/images/user/");
            }
            await _context.SaveChangesAsync();
            return View();
        }

        public async Task<IActionResult> History()
        {
            AppUser user = await _userManager.GetUserAsync(HttpContext.User);
            List<BookedTour> bookedTours = await _context.BookedTours.Where(b => b.UserId == user.Id).Include(b=>b.Tour).ToListAsync();
            UserModel userModel= new UserModel
            {
                User=user,
                BookedTours=bookedTours,
            };
            return View(userModel);
        }
    }
}
