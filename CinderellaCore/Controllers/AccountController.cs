using CinderellaCore.Model.Models;
using CinderellaCore.Web.Views.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CinderellaCore.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationUser _user;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(ApplicationUser user, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _user = user;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult SignIn()
        {
            return View("Login");
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(LoginModel login)
        {
            var result = await _signInManager.PasswordSignInAsync(login.Email, login.Password, true, false);
            if (result.Succeeded) return RedirectToAction("Index", "Home");

            return View("Login", login);
        }

        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult UpdateAccount() => View();

        [HttpPost]
        public async Task<IActionResult> UpdateAccount(UpdateAccountModel update)
        {
            var user = await _userManager.FindByEmailAsync(User.Identity.Name);
            user.UserNum = update.UserNum;
            await _userManager.UpdateAsync(user);

            return RedirectToAction("UpdateAccount", "Account");
        }
    }
}