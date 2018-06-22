using CinderellaCore.Model.Models;
using CinderellaCore.Services.Services.Interfaces;
using CinderellaCore.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CinderellaCore.Web.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationUser _user;
        private readonly ITestService _testService;

        public HomeController(ApplicationUser user, ITestService testService)
        {
            _user = user;
            var us = User?.Identity?.Name;
            _testService = testService;
        }

        public IActionResult Index()
        {
            //   _user = _userManager.FindByNameAsync(User.Identity.Name).Result;
            //   _user.EmailConfirmed = true;
            //   _userManager.UpdateAsync(_user);
            var things = _testService.GetAll();
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