using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CinderellaCore.Web.Views.Account
{
	public class UpdateAccountModel : PageModel
	{
		public int UserNum { get; set; }
		public bool EnableImport { get; set; }

		public void OnGet()
		{
		}
	}
}