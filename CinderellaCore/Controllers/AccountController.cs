using CinderellaCore.Model.Models;
using CinderellaCore.Web.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CinderellaCore.Web.Models;

namespace CinderellaCore.Web.Controllers
{
	public class AccountController : CinderellaCoreBaseController
	{
		private readonly SignInManager<ApplicationUser> _signInManager;

		public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager) : base(userManager)
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

		[HttpGet]
		public IActionResult Register() => View();

		[HttpPost]
		public async Task<IActionResult> Register(RegisterModel registration)
		{
			if (registration.Password != registration.PasswordConfirm)
			{
				ShowStatusMessage(MessageTypeEnum.error, "Passwords do not match", "Error registering");

				return View(registration);
			}
			var newUser = new ApplicationUser();
			newUser.Email = registration.Email;
			newUser.UserName = registration.Email;
			newUser.DisplayName = registration.DisplayName;

			var result = await _userManager.CreateAsync(newUser, registration.Password);

			return RedirectToAction("Index", "Home");
		}

		[HttpGet]
		public IActionResult UpdateAccount()
		{
			var model = new UpdateAccountModel
			{
				DisplayName = _user.DisplayName,
				EnableImport = _user.EnableImport
			};

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> UpdateAccount(UpdateAccountModel update)
		{
			var user = await _userManager.FindByEmailAsync(User.Identity.Name);
			user.DisplayName = update.DisplayName;
			user.EnableImport = update.EnableImport;
			await _userManager.UpdateAsync(user);

			ShowStatusMessage(MessageTypeEnum.info, "User profile updated", "Profile");

			return RedirectToAction("UpdateAccount", "Account");
		}
	}
}