using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using YatriiWorld.Models;

namespace YatriiWorld.Controllers
{
    public class DestinationController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public DestinationController(UserManager<AppUser> userManager)
        {
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
        public async Task<IActionResult> Booking( string stripeEmail,string stripeToken)
        {
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            decimal total = 0;
            //traveller count * tour price
            var optionCust = new CustomerCreateOptions
            {
                Email = stripeEmail,
                Name = user.Name + " " + user.Surname,
                Phone = "+994 50 66"
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
            return View();
        }
    }
}
