using CinderellaCore.Model.Models;
using CinderellaCore.Web.Views.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CinderellaCore.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
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
    }
}