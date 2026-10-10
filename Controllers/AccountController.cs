using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MedicaidEmploymentVerificationApplication.Models;
using Microsoft.AspNetCore.Authorization;

namespace MedicaidEmploymentVerificationApplication.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {

        private UserManager<User> userManager;
        private SignInManager<User> signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        //TODO: Register, LogIn, and Logout Methods

        [HttpGet]
        public IActionResult Login(string returnURL = "")
        {
            var model = new LoginViewModel { ReturnURL = returnURL };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if(ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.Username, model.Password, isPersistent: model.RememberMe, lockoutOnFailure: false);

                if(result.Succeeded)
                {
                    if(!string.IsNullOrWhiteSpace(model.ReturnURL))
                    {
                        return Redirect(model.ReturnURL);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            ModelState.AddModelError("", "Invalid Username or Password");
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if(ModelState.IsValid)
            {
                var user = new User { UserName = model.Username };
                var result = await userManager.CreateAsync(user, model.Password);
                if(result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Applicant");
                    //await userManager.AddToRoleAsync(user, "Employee"); // Employees must not be created this way in production
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach(var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }



        public IActionResult Index()
        {
            return View();
        }
    }
}
