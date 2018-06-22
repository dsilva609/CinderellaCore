using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CinderellaCore.Web.Views.Account
{
    public class LoginModel : PageModel
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public void OnGet()
        {
        }
    }
}