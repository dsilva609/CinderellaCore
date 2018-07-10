using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel;

namespace CinderellaCore.Web.Views.Account
{
    public class UpdateAccountModel : PageModel
    {
        [DisplayName("Display Name")]
        public string DisplayName { get; set; }

        [DisplayName("Enable Import")]
        public bool EnableImport { get; set; }

        public void OnGet()
        {
        }
    }
}