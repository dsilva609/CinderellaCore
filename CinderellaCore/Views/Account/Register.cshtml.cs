using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CinderellaCore.Web.Views.Account
{
	public class RegisterModel : PageModel
	{
		public string Email { get; set; }
		public string Password { get; set; }
		public string PasswordConfirm { get; set; }
		public string UserName { get; set; }

		public void OnGet()
		{
		}
	}
}