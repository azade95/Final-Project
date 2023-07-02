using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;
using YatriiWorld.DAL;
using YatriiWorld.Models;
using YatriiWorld.Utilities.Exceptions;
using YatriiWorld.ViewModels;

namespace YatriiWorld.Controllers
{
    public class DestinationController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public DestinationController(AppDbContext context,UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

       public IActionResult Details()
        {
            return View();
        }
        [Authorize]
        public async Task<IActionResult> Booking(int? id)
        {
            if (id == null || id < 1) throw new WrongRequestException("id is not correct");
            Tour tour = await _context.Tours.Include(t=>t.Reviews).Include(t => t.User).FirstOrDefaultAsync(t => t.Id == id);
            if (tour == null) throw new NotFoundException("Tour is not found");
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null) throw new NotFoundException("user is not found");

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Booking(int? id,BookingVM bookingVM, string stripeEmail,string stripeToken)
        {
            if (id == null || id < 1) throw new WrongRequestException("id is not correct");
            Tour tour=await _context.Tours.FirstOrDefaultAsync(t=>t.Id==id);
            if (tour == null) throw new NotFoundException("Tour is not found");
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null) throw new NotFoundException("user is not found");
            
            BookedTour bookedTour = new BookedTour
            {
                TourId=tour.Id,
                UserId=user.Id,
                Price=(decimal)tour.Price,
            };
            decimal total = 0;
            total = bookedTour.Price * bookingVM.PeopleCount;

            
            var optionCust = new CustomerCreateOptions
            {
                Email = stripeEmail,
                Name = user.Name + " " + user.Surname,
                Phone = user.Phone,
            };
            var serviceCust = new CustomerService();
            Customer customer = serviceCust.Create(optionCust);

            total = total * 100;
            var optionsCharge = new ChargeCreateOptions
            {

                Amount = (long)total,
                Currency = "USD",
                Description = "Booking amount",
                Source = stripeToken,
                ReceiptEmail = stripeEmail


            };
            var serviceCharge = new ChargeService();
            Charge charge = serviceCharge.Create(optionsCharge);
            if (charge.Status != "succeeded")
            {
                ModelState.AddModelError(String.Empty, "Odenishde problem var");
                return View();
            }

            _context.BookedTours.Add(bookedTour);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index","Home");
        }
       
    }
}
