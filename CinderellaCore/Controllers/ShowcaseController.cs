using CinderellaCore.Model.Enums;
using CinderellaCore.Model.Models;
using CinderellaCore.Services.Features.Album;
using CinderellaCore.Services.Features.Book;
using CinderellaCore.Services.Features.Game;
using CinderellaCore.Services.Features.Movie;
using CinderellaCore.Services.Features.Pop;
using CinderellaCore.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace CinderellaCore.Web.Controllers
{
    public class ShowcaseController : CinderellaCoreBaseController
    {
        private readonly IAlbumService _albumService;
        private readonly IBookService _bookService;
        private readonly IGameService _gameService;
        private readonly IMovieService _movieService;
        private readonly IPopService _popService;

        public ShowcaseController(UserManager<ApplicationUser> userManager, IAlbumService albumService, IBookService bookService, IGameService gameService,
            IMovieService movieService,
            IPopService popService) : base(userManager)
        {
            _albumService = albumService;
            _bookService = bookService;
            _gameService = gameService;
            _movieService = movieService;
            _popService = popService;
        }

        [HttpGet]
        public IActionResult Index(int id)
        {
            var model = new ShowcaseViewModel
            {
                ViewTitle = "Showcase",
                Albums = _albumService.GetAll().Where(x => x.IsShowcased && x.UserNum == id).ToList(),
                Books = _bookService.GetAll().Where(x => x.IsShowcased && x.UserNum == id).ToList(),
                Games = _gameService.GetAll().Where(x => x.IsShowcased && x.UserNum == id).ToList(),
                Movies = _movieService.GetAll().Where(x => x.IsShowcased && x.UserNum == id).ToList(),
                Pops = _popService.GetAll().Where(x => x.IsShowcased && x.UserNum == id).ToList()
            };

            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Add()
        {
            ViewBag.Title = "Add";

            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult SearchItems(string query, ItemType type)
        {
            SetSessionString($"{type.ToString().ToLower()}-query", query.Trim());

            return RedirectToAction("Index", type.ToString());
        }
    }
}