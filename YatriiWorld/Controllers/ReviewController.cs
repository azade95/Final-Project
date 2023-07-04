using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using YatriiWorld.DAL;
using YatriiWorld.Models;

namespace YatriiWorld.Controllers
{
    public class ReviewController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public ReviewController(AppDbContext context,UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Review review,int? id=null)
        {
            AppUser user = await _userManager.GetUserAsync(HttpContext.User);
            review.UserId = user.Id;
            if (id != null)
            {
                review.TourId =(int) id;
            }
            _context.Reviews.Add(review);
            _context.Ratings.Add(review.Rating);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index","Home");
        }
    }
}
