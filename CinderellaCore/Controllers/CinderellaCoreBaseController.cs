using CinderellaCore.Model.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CinderellaCore.Web.Controllers
{
    public class CinderellaCoreBaseController : Controller
    {
        protected ApplicationUser _user => GetCurrentUser();
        protected readonly UserManager<ApplicationUser> _userManager;

        public CinderellaCoreBaseController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public ApplicationUser GetCurrentUser()
        {
            return HttpContext != null ? _userManager.GetUserAsync(HttpContext.User).Result : null;
        }
    }
}