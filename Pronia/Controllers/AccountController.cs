using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pronia.Models;
using Pronia.ViewModels;
using System.Threading.Tasks;

namespace Pronia.Controllers
{

    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager,RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Register(RegisterVM register)
        {
            if(!ModelState.IsValid) return View();
            if (!register.Terms)
            {
                ModelState.AddModelError("Terms", "Accept Terms right now!");
                return View();
            }
            AppUser user = new AppUser()
            {
                firstName = register.firstName,
                lastName = register.lastName,
                UserName = register.username,
                Email = register.email
            };
            
            IdentityResult identityResult = await _userManager.CreateAsync(user,register.Password);

            if (!identityResult.Succeeded)
            {
                foreach (IdentityError item in identityResult.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View();
            }
            await _userManager.AddToRoleAsync(user, "Member");
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Login()
        {
            return View();
        }

        

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Login(LoginVM login)
        {
            if (!ModelState.IsValid) return View();
            AppUser user = await _userManager.FindByNameAsync(login.userName);
            if (user is null) return View();
            Microsoft.AspNetCore.Identity.SignInResult sign = await _signInManager.PasswordSignInAsync(user, login.password, login.rememberMe, true);
            if (!sign.Succeeded)
            {
                if (sign.IsLockedOut)
                {
                    ModelState.AddModelError("", "Wait 10 minutes");
                    return View();
                }
                ModelState.AddModelError("", "Password or username is incorrect ");
                return View();
            }
            HttpContext.Response.Cookies.Delete("Cart");

            return RedirectToAction("Register", "Account");
        }
         

        public async Task<IActionResult> Logout()
        {
            //await HttpContext.SignOutAsync();
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        public JsonResult Authentication()
        {
            return Json(User.Identity.IsAuthenticated);
        }

        //public async Task Roles()
        //{
        //    await _roleManager.CreateAsync(new IdentityRole("Member"));
        //    await _roleManager.CreateAsync(new IdentityRole("Mod"));
        //    await _roleManager.CreateAsync(new IdentityRole("Admin"));
        //}
    }
}
