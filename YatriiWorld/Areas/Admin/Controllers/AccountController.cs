using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using YatriiWorld.Interfaces;
using YatriiWorld.Models;
using YatriiWorld.Utilities.Enums;
using YatriiWorld.Utilities.Exceptions;
using YatriiWorld.Utilities.Extensions;
using YatriiWorld.ViewModels;

namespace YatriiWorld.Areas.Admin.Controllers
{
    [Area("Admin")]
    
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailService _emailService;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IMapper mapper, RoleManager<IdentityRole> roleManager,IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _roleManager = roleManager;
            _emailService = emailService;
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
            AppUser user = _mapper.Map<AppUser>(newuser);
            IdentityResult result = await _userManager.CreateAsync(user, newuser.Password);

            if (!result.Succeeded)
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View();
            }
            await _userManager.AddToRoleAsync(user, UserRole.Member.ToString());
            var token = _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = Url.Action(nameof(ConfirmEmail), "Account", new { token, Email = user.Email }, Request.Scheme);
            _emailService.SendEmail(user.Email, "Email Confirmation", confirmationLink);
            // await _signInManager.SignInAsync(user, false);

            return RedirectToAction(nameof(SuccessfullyRegistered), "Account");
        }
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            AppUser user = await _userManager.FindByEmailAsync(email);
            if (user == null) throw new NotFoundException("User not found");
            var result = await _userManager.ConfirmEmailAsync(user,token);
            if (!result.Succeeded)
            {
                throw new TokenExpireException("token has expired");
            }
            await _signInManager.SignInAsync(user, false);
            return View();
        }

        public IActionResult SuccessfullyRegistered()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Login(LoginVM loginVM, string ReturnUrl)
        {
            if (!ModelState.IsValid) return View();

            AppUser existed = await _userManager.FindByNameAsync(loginVM.UsernameOrEmail);
            if (existed == null)
            {
                existed = await _userManager.FindByEmailAsync(loginVM.UsernameOrEmail);
                if (existed == null)
                {
                    ModelState.AddModelError(String.Empty, "Username, Email or Password is not correct");
                    return View();
                }
            }

            var result = await _signInManager.PasswordSignInAsync(existed, loginVM.Password, loginVM.IsRemember, true);
            if (result.IsLockedOut)
            {
                ModelState.AddModelError(String.Empty, "Login is not enable please try again later");
                return View();
            }
            if (!existed.EmailConfirmed)
            {
                ModelState.AddModelError(String.Empty, "Please confirm your email");
                return View();
            }
            if (!result.Succeeded)
            {
                ModelState.AddModelError(String.Empty, "Username, Email or Password is not correct");
                return View();
            }

            if (ReturnUrl != null)
            {
                return Redirect(ReturnUrl);
            }
            return RedirectToAction("Index", "Home", new { area = "" });

        }

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home", new { area = "" });
        }

        public async Task<IActionResult> CreateRoles()
        {
            foreach (var role in Enum.GetValues(typeof(UserRole)))
            {
                if (!(await _roleManager.RoleExistsAsync(role.ToString())))
                {
                    await _roleManager.CreateAsync(new IdentityRole { Name = role.ToString() });
                }
            }
            return RedirectToAction("Index", "Home");
        }

    }
}
