﻿using CinderellaCore.Model.Enums;
using CinderellaCore.Model.Models;
using CinderellaCore.Services.Services.Interfaces;
using CinderellaCore.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Linq;

namespace CinderellaCore.Web.Controllers
{
    public class HomeController : CinderellaCoreBaseController
    {
        private readonly GlobalSettings _settings;
        private readonly IAlbumService _albumService;
        private readonly IBookService _bookService;
        private readonly IMovieService _movieService;
        private readonly IGameService _gameService;
        private readonly IPopService _popService;
        private const int NUM_ALBUMS_TO_GET = 10;
        private const int NUM_BOOKS_TO_GET = 10;
        private const int NUM_MOVIES_TO_GET = 10;
        private const int NUM_GAMES_TO_GET = 10;
        private const int NUM_POPS_TO_GET = 10;

        public HomeController(UserManager<ApplicationUser> userManager, GlobalSettings settings, IAlbumService albumService, IBookService bookService, IMovieService movieService,
            IGameService gameService, IPopService popService) : base(userManager)
        {
            _settings = settings;
            _albumService = albumService;
            _bookService = bookService;
            _movieService = movieService;
            _gameService = gameService;
            _popService = popService;
            var us = GetCurrentUser();
        }

        public IActionResult Index()
        {
            //TODO: needs refactor to take asc/desc
            var albums = _albumService.GetAll(string.Empty, string.Empty).OrderByDescending(x => x.DateAdded).Take(NUM_ALBUMS_TO_GET).ToList();
            var updatedAlbums =
                _albumService.GetAll(string.Empty, string.Empty).OrderByDescending(x => x.DateUpdated).Take(NUM_ALBUMS_TO_GET).ToList();

            var books = _bookService.GetAll(string.Empty, string.Empty).OrderByDescending(x => x.DateAdded).Take(NUM_BOOKS_TO_GET).ToList();
            var updatedBooks = _bookService.GetAll(string.Empty, string.Empty).OrderByDescending(x => x.DateUpdated).Take(NUM_BOOKS_TO_GET).ToList();

            var movies = _movieService.GetAll(string.Empty, string.Empty).OrderByDescending(x => x.DateAdded).Take(NUM_MOVIES_TO_GET).ToList();
            var updatedMovies =
                _movieService.GetAll(string.Empty, string.Empty).OrderByDescending(x => x.DateUpdated).Take(NUM_MOVIES_TO_GET).ToList();

            var games = _gameService.GetAll(string.Empty, string.Empty).OrderByDescending(x => x.DateAdded).Take(NUM_GAMES_TO_GET).ToList();
            var updatedGames = _gameService.GetAll(string.Empty, string.Empty).OrderByDescending(x => x.DateUpdated).Take(NUM_GAMES_TO_GET).ToList();

            var pops = _popService.GetAll(string.Empty, string.Empty).OrderByDescending(x => x.DateAdded).Take(NUM_POPS_TO_GET).ToList();
            var updatedPops = _popService.GetAll(string.Empty, string.Empty).OrderByDescending(x => x.DateUpdated).Take(NUM_POPS_TO_GET).ToList();

            var recordStoreDate = _settings.RecordStoreDayDate;
            var recordStoreDayTimer = new TimerModel { ID = "recordStoreDayTimer", Year = recordStoreDate.Year, Month = recordStoreDate.Month, Day = recordStoreDate.Day };

            var freeComicBookDayDate = _settings.FreeComicBookDayDate;
            var freeComicBookDayTimer = new TimerModel { ID = "freeComicBookDayTimer", Year = freeComicBookDayDate.Year, Month = freeComicBookDayDate.Month, Day = freeComicBookDayDate.Day };

            var model = new HomeViewModel
            {
                Albums = albums,
                UpdatedAlbums = updatedAlbums,
                Books = books,
                UpdatedBooks = updatedBooks,
                Movies = movies,
                UpdatedMovies = updatedMovies,
                Games = games,
                UpdatedGames = updatedGames,
                Pops = pops,
                UpdatedPops = updatedPops,
                RecordStoreDayTimer = recordStoreDayTimer,
                FreeComicBookDayTimer = freeComicBookDayTimer
            };

            return View(model);
        }

        [HttpGet]
        public virtual IActionResult About()
        {
            ViewBag.Message = "Project Cinderella.";

            return View();
        }

        [HttpGet]
        public virtual IActionResult Contact()
        {
            ViewBag.Message = "Contact and Heavy Metal Friday Updates";

            return View();
        }

        [HttpGet]
        public virtual ActionResult Search(string query, ItemType type, string act)
        {
            if (type == ItemType.Pop && act == "Search") act = "Index";

            HttpContext.Session.SetString($"{type.ToString().ToLower()}-query", query.Trim());
            return RedirectToAction(act, type.ToString());
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