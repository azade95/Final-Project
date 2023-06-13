using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using YatriiWorld.Models;
using YatriiWorld.Utilities.Extensions;
using YatriiWorld.ViewModels;

namespace YatriiWorld.Areas.Admin.Controllers
{
    [Area("Admin")]
    
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM newuser)
        {
            if (!ModelState.IsValid) return View();

            if (!newuser.Email.CheckEmail())
            {
                ModelState.AddModelError("Email", "Email formati dogru deyil");
                return View();
            }
            AppUser user = new AppUser
            {
                Name = newuser.Name.Capitalize(),
                Surname = newuser.Surname.Capitalize(),
                Email = newuser.Email,
                UserName = newuser.Name,
                Gender = newuser.Gender,

            };
            IdentityResult result = await _userManager.CreateAsync(user, newuser.Password);

            if (!result.Succeeded)
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View();
            }
            await _signInManager.SignInAsync(user, false);

            return RedirectToAction("Index", "Home", new { area = "" });
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid) return View();

            AppUser existed = await _userManager.FindByNameAsync(loginVM.UsernameOrEmail);
            if (existed == null)
            {
                existed = await _userManager.FindByEmailAsync(loginVM.UsernameOrEmail);
            }

            var result = await _signInManager.PasswordSignInAsync(existed, loginVM.Password, loginVM.IsRemember, true);
            if (!result.Succeeded)
            {
                return View();
            }
            return RedirectToAction("Index", "Home");

        }

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
