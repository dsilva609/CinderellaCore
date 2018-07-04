using CinderellaCore.Model.Models;
using CinderellaCore.Web.Enums;
using CinderellaCore.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CinderellaCore.Web.Controllers
{
    public class HomeController : CinderellaCoreBaseController
    {
        public HomeController(UserManager<ApplicationUser> userManager) : base(userManager)
        {
            var us = GetCurrentUser();
        }

        public IActionResult Index()
        {
            //var email = _user.Email;

            ShowStatusMessage(MessageTypeEnum.info, "test", "hgeader");
            ShowStatusMessage(MessageTypeEnum.error, "test error", "err");

            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}