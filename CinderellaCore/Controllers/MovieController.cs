using CinderellaCore.Model.Models;
using CinderellaCore.Services.Services.Interfaces;
using CinderellaCore.Web.Enums;
using CinderellaCore.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using X.PagedList;
using CompletionStatus = CinderellaCore.Model.CompletionStatus;

namespace CinderellaCore.Web.Controllers
{
    public class MovieController : CinderellaCoreBaseController
    {
        private readonly IMovieService _service;
        private readonly ITMDBService _tmdbService;
        private readonly IWishService _wishService;
        private const int NUM_MOVIES_TO_GET = 25;

        public MovieController(UserManager<ApplicationUser> userManager, IMovieService service, ITMDBService tmdbService, IWishService wishService) : base(userManager)
        {
            _service = service;
            _tmdbService = tmdbService;
            _wishService = wishService;
        }

        [HttpGet]
        public IActionResult Index(string movieQuery, string filter, int? page)
        {
            if (string.IsNullOrWhiteSpace(movieQuery) && SessionValueExists("movie-query"))
            {
                movieQuery = GetFromSession<string>("movie-query");
                RemoveFromSession("movie-query");
            }

            ViewBag.Filter = (string.IsNullOrWhiteSpace(movieQuery) ? filter : movieQuery)?.Trim();

            var movies = _service.GetAll(string.Empty, ViewBag.Filter) as List<Movie>;
            var viewModel = new MovieViewModel
            {
                ViewTitle = "Movies/TV",
                Movies = movies?.ToPagedList(page ?? 1, NUM_MOVIES_TO_GET),
                PageSize = NUM_MOVIES_TO_GET,
            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var movie = _service.GetByID(id, _user?.Id);
            ViewBag.Title = $"Details - {movie.Title}";

            return View(movie);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Create()
        {
            var model = SessionValueExists("movieResult") ? GetFromSession<Movie>("movieResult") : new Movie { UserID = _user.Id, UserNum = _user.UserNum };
            ViewBag.Title = "Create";
            RemoveFromSession("movieResult");

            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Movie movie)
        {
            if (!ModelState.IsValid) return View(movie);

            try
            {
                if (movie.CompletionStatus == CompletionStatus.Completed && movie.TimesCompleted == 0)
                    movie.TimesCompleted = 1;
                movie.DateAdded = DateTime.UtcNow;
                SetTimeStamps(movie);

                this._service.Add(movie);
            }
            catch (Exception e)
            {
                ShowStatusMessage(MessageTypeEnum.error, e.Message, "Duplicate Movie");

                return View(movie);
            }
            RemoveFromSession("movie-query");

            if (SessionValueExists("wish"))
            {
                _wishService.Delete(Convert.ToInt32(GetFromSession<string>("wishID")), _user.Id);

                RemoveFromSession("wish");
                RemoveFromSession("wishID");
                ShowStatusMessage(MessageTypeEnum.info, "Wish list has been updated", "Wish list");
            }
            ShowStatusMessage(MessageTypeEnum.success, "New Movie Added Successfully.", "Add Successful");

            return RedirectToAction("Index", "Movie");
        }

        [Authorize]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var model = _service.GetByID(id, _user.Id);
            ViewBag.Title = $"Edit - {model.Title}";
            if (model.UserID != _user.Id) return RedirectToAction("Details", "Movie", model.ID);

            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Movie movie)
        {
            if (!ModelState.IsValid) return View(movie);

            var existingMovies = _service.GetAll(_user.Id);

            if (existingMovies.Count > 0 && existingMovies.Any(x => x.ID != movie.ID && x.Title == movie.Title && x.Type == movie.Type))
            {
                ShowStatusMessage(MessageTypeEnum.error, $"A Game of Title: {movie.Title}, Media Type: {movie.Type} already exists.",
                    "Duplicate Movie");

                return View(movie);
            }

            if (movie.CompletionStatus == CompletionStatus.Completed && movie.TimesCompleted == 0)
                movie.TimesCompleted = 1;
            if (movie.TimesCompleted > 0) movie.CompletionStatus = CompletionStatus.Completed;
            SetTimeStamps(movie);
            //TODO: make sure user id is the same so as not to change other users data
            var previous = _service.GetByID(movie.ID, _user.Id);
            if (movie.TimesCompleted > previous.TimesCompleted)
                movie.LastCompleted = movie.DateCompleted;
            movie.DateUpdated = DateTime.UtcNow;
            _service.Edit(movie);

            ShowStatusMessage(MessageTypeEnum.success, $"Movie of Title {movie.Title}, Media Type: {movie.Type} updated.", "Update Successful");

            return RedirectToAction("Index", "Movie");
        }

        [Authorize]
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var model = _service.GetByID(id, _user.Id);
            if (model.UserID != _user.Id)
            {
                ShowStatusMessage(MessageTypeEnum.error, "This game cannot be deleted by another user", "Delete Failure");

                return RedirectToAction("Index", "Movie");
            }

            _service.Delete(id, _user.Id);

            ShowStatusMessage(MessageTypeEnum.success, "", "Movie Deleted Successfully");

            return RedirectToAction("Index", "Movie");
        }

        [Authorize]
        [HttpGet]
        public IActionResult Search(MovieSearchModel searchModel)
        {
            if (!string.IsNullOrWhiteSpace(searchModel.Title)) searchModel.Title = searchModel.Title.Trim();
            if (SessionValueExists("movie-query")) searchModel.Title = GetStringFromSession("movie-query");
            if (SessionValueExists("wish")) searchModel.Title = GetFromSession<string>("wish");

            if (Request.Headers["Referrer"].Contains("/Movie/Search") && string.IsNullOrWhiteSpace(searchModel.Title))
            {
                ShowStatusMessage(MessageTypeEnum.error, "Please enter search terms.", "Search Error");

                return View(searchModel);
            }

            if (!string.IsNullOrWhiteSpace(searchModel.Title))
            {
                searchModel.MovieResults = _tmdbService.SearchMovies(searchModel.Title);
                searchModel.TVShowResults = _tmdbService.SearchTV(searchModel.Title);
                foreach (var result in searchModel.MovieResults)
                    result.poster_path = string.Format("https://image.tmdb.org/t/p/w300{0}", result.poster_path);

                foreach (var result in searchModel.TVShowResults)
                    result.poster_path = string.Format("https://image.tmdb.org/t/p/w300{0}", result.poster_path);
            }
            ViewBag.Title = "Movie/TV Show Search";

            return View(searchModel);
        }

        [Authorize]
        [HttpGet]
        public IActionResult CreateFromSearchResult(int releaseID, bool isTvShow, int seasonNumber)
        {
            var movie = isTvShow ? _tmdbService.SearchTVShowByID(releaseID, seasonNumber) : _tmdbService.SearchMovieByID(releaseID);

            movie.UserID = _user.Id;
            movie.UserNum = _user.UserNum;
            ViewBag.Title = "Create";

            SetSessionString("movieResult", movie);

            return RedirectToAction("Create", "Movie");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult AddToShowcase(int id)
        {
            var movie = _service.GetByID(id, _user.Id);
            movie.IsShowcased = true;
            movie.DateUpdated = DateTime.UtcNow;
            _service.Edit(movie);

            ShowStatusMessage(MessageTypeEnum.info, "Title added to showcase", "Showcase");

            return RedirectToAction("Index", "Showcase", new { id = _user.UserNum });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult RemoveFromShowcase(int id)
        {
            var movie = _service.GetByID(id, _user.Id);
            movie.IsShowcased = false;
            movie.DateUpdated = DateTime.UtcNow;
            _service.Edit(movie);

            ShowStatusMessage(MessageTypeEnum.info, "Title removed from showcase", "Showcase");

            return RedirectToAction("Index", "Showcase", new { id = _user.UserNum });
        }

        [Authorize]
        [HttpGet]
        public IActionResult IncreaseCompletionCount(int id)
        {
            var movie = _service.GetByID(id, _user.Id);

            if (movie.UserID != _user.Id)
            {
                ShowStatusMessage(MessageTypeEnum.warning, "This movie cannot be edited by another user.", "Edit Failure");

                return RedirectToAction("Index", "Movie");
            }

            movie.TimesCompleted += 1;
            movie.CompletionStatus = CompletionStatus.Completed;
            movie.LastCompleted = DateTime.UtcNow;
            movie.DateUpdated = DateTime.UtcNow;
            _service.Edit(movie);

            ShowStatusMessage(MessageTypeEnum.info, "Movie was updated.", "Update");

            return RedirectToAction("Index", "Movie");
        }

        [Authorize]
        [HttpGet]
        public IActionResult DecreaseCompletionCount(int id)
        {
            var movie = _service.GetByID(id, _user.Id);

            if (movie.UserID != _user.Id)
            {
                ShowStatusMessage(MessageTypeEnum.warning, "This movie cannot be edited by another user.", "Edit Failure");

                return RedirectToAction("Index", "Movie");
            }

            if (movie.TimesCompleted > 0) movie.TimesCompleted -= 1;
            if (movie.TimesCompleted == 0) movie.CompletionStatus = CompletionStatus.NotStarted;
            movie.DateUpdated = DateTime.UtcNow;
            _service.Edit(movie);

            ShowStatusMessage(MessageTypeEnum.info, "Movie was updated.", "Update");

            return RedirectToAction("Index", "Movie");
        }

        [Authorize]
        [HttpGet]
        public IActionResult AddToQueue(int id)
        {
            var game = _service.GetByID(id, _user.Id);

            if (game.UserID != _user.Id)
            {
                ShowStatusMessage(MessageTypeEnum.error, "This movie/TV show cannot be edited by another user.", "Edit Failure");

                return RedirectToAction("Index", "Movie");
            }

            if (game.IsQueued)
            {
                ShowStatusMessage(MessageTypeEnum.warning, "This movie/TV show is already queued.", "Edit Failure");

                return RedirectToAction("Index", "Movie");
            }

            game.IsQueued = true;
            var currentHighestRank = _service.GetHighestQueueRank(_user.Id);
            game.QueueRank = currentHighestRank + 1;

            _service.Edit(game);

            ShowStatusMessage(MessageTypeEnum.info, "Movie/TV Show added to queue", "Queue");

            return RedirectToAction("Index", "Queue");
        }

        [Authorize]
        [HttpGet]
        public IActionResult RemoveFromQueue(int id)
        {
            var game = _service.GetByID(id, _user.Id);

            if (game.UserID != _user.Id)
            {
                ShowStatusMessage(MessageTypeEnum.error, "This movie/TV show cannot be edited by another user.", "Edit Failure");

                return RedirectToAction("Index", "Movie");
            }

            game.IsQueued = false;
            game.QueueRank = 0;

            _service.Edit(game);

            ShowStatusMessage(MessageTypeEnum.info, "Movie/TV Show removed from queue,", "Queue");

            return RedirectToAction("Index", "Queue");
        }
    }
}