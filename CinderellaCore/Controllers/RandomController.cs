using CinderellaCore.Model.Models;
using CinderellaCore.Services.Features.Album;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CinderellaCore.Web.Controllers
{
    public class RandomController : CinderellaCoreBaseController
    {
        private readonly IAlbumService _albumService;

        public RandomController(UserManager<ApplicationUser> userManager, IAlbumService albumService) : base(userManager)
        {
            _albumService = albumService;
        }

        [HttpGet]
        public IActionResult RandomizeAlbums(int count)
        {
            var albums = _albumService.GetRandomAlbums(_user.Id, count);

            return View("RandomAlbums", albums);
        }
    }
}