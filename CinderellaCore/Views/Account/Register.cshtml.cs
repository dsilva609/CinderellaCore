using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel;

namespace CinderellaCore.Web.Views.Account
{
    public class RegisterModel : PageModel
    {
        public string Email { get; set; }
        public string Password { get; set; }

        [DisplayName("Confirm Password")]
        public string PasswordConfirm { get; set; }

        [DisplayName("Display Name")]
        public string DisplayName { get; set; }

        public void OnGet()
        {
        }
    }
}