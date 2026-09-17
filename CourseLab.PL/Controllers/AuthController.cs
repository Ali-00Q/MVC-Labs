using CourseLab.BLL.ModelVM.AuthVm;
using CourseLab.DAL.Database;
using CourseLab.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CourseLab.PL.Controllers
{
    public class AuthController : Controller
    {
        public readonly CourseLabDbContext db;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public AuthController(CourseLabDbContext db, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.db = db;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View("Register");
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM userVM)
        {
            if (!ModelState.IsValid)
            {
                return View("Register", userVM);
            }
            ApplicationUser user = new ApplicationUser()
            {
                Email = userVM.Email,
                UserName = userVM.Name

            };
            var identityResult = await userManager.CreateAsync(user, userVM.Password);
            if (!identityResult.Succeeded)
            {
                foreach(var error in identityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View("Register", userVM);
            }

            await signInManager.SignInAsync(user, false);
            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM userVM)
        {
            if (!ModelState.IsValid)
            {
                return View("Login", userVM);
            }

            var user = await userManager.FindByEmailAsync(userVM.Email);

            if (user != null)
            {
                var result = await signInManager.CheckPasswordSignInAsync(
                    user,
                    userVM.Password,
                    false);

                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, false);

                    return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError("", "Invalid email or password.");

            return View("Login", userVM);
        }


        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}
