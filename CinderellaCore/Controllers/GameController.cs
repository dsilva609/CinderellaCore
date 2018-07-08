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
    public class GameController : CinderellaCoreBaseController
    {
        private readonly IGameService _service;
        private readonly IGiantBombService _giantBombService;
        private readonly IBGGService _bggService;
        private readonly IWishService _wishService;
        private const int NUM_GAMES_TO_GET = 25;

        public GameController(UserManager<ApplicationUser> userManager, IGameService service, IGiantBombService giantBombService, IBGGService bggService,
            IWishService wishService) : base(userManager)

        {
            _service = service;
            _giantBombService = giantBombService;
            _bggService = bggService;
            _wishService = wishService;
        }

        [HttpGet]
        public IActionResult Index(string gameQuery, string filter, int? page = 1)
        {
            if (string.IsNullOrWhiteSpace(gameQuery) && SessionValueExists("game-query"))
            {
                gameQuery = GetFromSession<string>("game-query");
                RemoveFromSession("game-query");
            }

            ViewBag.Filter = (string.IsNullOrWhiteSpace(gameQuery) ? filter : gameQuery)?.Trim();

            var games = _service.GetAll(string.Empty, ViewBag.Filter) as List<Game>;

            var viewModel = new GameViewModel
            {
                ViewTitle = "Games",
                Games = games?.ToPagedList(page ?? 1, NUM_GAMES_TO_GET),
                PageSize = NUM_GAMES_TO_GET
            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var model = _service.GetByID(id, _user.Id);
            ViewBag.Title = $"Details - {model.Title}";

            return View(model);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Title = "Create";
            var model = SessionValueExists("gameResult") ? GetFromSession<Game>("gameResult") : new Game { UserID = _user.Id, UserNum = _user.UserNum };
            RemoveFromSession("gameResult");

            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Game game)
        {
            if (!ModelState.IsValid) return View(game);
            try
            {
                if (game.CompletionStatus == CompletionStatus.Completed && game.TimesCompleted == 0)
                    game.TimesCompleted = 1;
                game.DateAdded = DateTime.UtcNow;
                SetTimeStamps(game);
                this._service.Add(game);
            }
            catch (Exception e)
            {
                ShowStatusMessage(MessageTypeEnum.error, e.Message, "Duplicate Game");

                return View(game);
            }
            RemoveFromSession("game-query");

            if (SessionValueExists("wish"))
            {
                _wishService.Delete(Convert.ToInt32(GetFromSession<string>("wishID")), _user.Id);
                RemoveFromSession("wish");
                RemoveFromSession("wishID");

                ShowStatusMessage(MessageTypeEnum.info, "Wish list has been updated", "Wish list");
            }
            ShowStatusMessage(MessageTypeEnum.success, "New Game Added Successfully.", "Add Successful");

            return RedirectToAction("Index", "Game");
        }

        [Authorize]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var model = _service.GetByID(id, _user.Id);
            ViewBag.Title = $"Edit - {model.Title}";
            if (model.UserID != _user.Id) return RedirectToAction("Details", "Game", model.ID);

            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Game game)
        {
            if (!ModelState.IsValid) return View(game);
            var existingGames = _service.GetAll(_user.Id);
            if (existingGames.Count > 0 &&
                existingGames.Any(x => x.ID != game.ID && x.Title == game.Title && x.Developer == game.Developer))
            {
                ShowStatusMessage(MessageTypeEnum.error,
                    $"A Game of Title: {game.Title}, Developer: {game.Developer} already exists.", "Duplicate Game");

                return View(game);
            }

            if (game.CompletionStatus == CompletionStatus.Completed && game.TimesCompleted == 0)
                game.TimesCompleted = 1;
            if (game.TimesCompleted > 0) game.CompletionStatus = CompletionStatus.Completed;
            SetTimeStamps(game);

            //TODO: make sure user id is the same so as not to change other users data
            var previous = _service.GetByID(game.ID, _user.Id);
            if (game.TimesCompleted > previous.TimesCompleted)
                game.LastCompleted = game.DateCompleted;
            game.DateUpdated = DateTime.UtcNow;
            _service.Edit(game);

            ShowStatusMessage(MessageTypeEnum.success,
                $"Game of Title {game.Title}, Developer: {game.Developer} updated.", "Update Successful");

            return RedirectToAction("Index", "Game");
        }

        [Authorize]
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var model = _service.GetByID(id, _user.Id);
            if (model.UserID != _user.Id)
            {
                ShowStatusMessage(MessageTypeEnum.error, "This game cannot be deleted by another user", "Delete Failure");

                return RedirectToAction("Index", "Game");
            }

            _service.Delete(id, _user.Id);

            ShowStatusMessage(MessageTypeEnum.success, "", "Game Deleted Successfully");

            return RedirectToAction("Index", "Game");
        }

        //TODO: add tests and validation
        [Authorize]
        [HttpGet]
        public IActionResult Search(GameSearchModel searchModel)
        {
            if (!string.IsNullOrWhiteSpace(searchModel.Title)) searchModel.Title = searchModel.Title.Trim();
            if (SessionValueExists("game-query")) searchModel.Title = GetFromSession<string>("game-query");
            if (SessionValueExists("wish")) searchModel.Title = GetFromSession<string>("wish");

            if (Request.Headers["Referrer"].Contains("/Game/Search") && string.IsNullOrWhiteSpace(searchModel.Title))
            {
                ShowStatusMessage(MessageTypeEnum.error, "Please enter search terms.", "Search Error");

                return View(searchModel);
            }

            if (!string.IsNullOrWhiteSpace(searchModel.Title))
            {
                searchModel.GiantBombResult = _giantBombService.Search(searchModel.Title);
                searchModel.BGGResult = _bggService.Search(searchModel.Title);
            }

            ViewBag.Title = "Game Search";

            return View(searchModel);
        }

        [Authorize]
        [HttpGet]
        public IActionResult CreateFromSearchModel(int id, bool isBGG)
        {
            ViewBag.Title = "Create";
            var game = isBGG ? _bggService.SearchByID(id) : _giantBombService.SearchByID(id);

            game.UserID = _user.Id;
            game.UserNum = _user.UserNum;
            SetSessionString("gameResult", game);

            return RedirectToAction("Create", "Game");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult AddToShowcase(int id)
        {
            var game = _service.GetByID(id, _user.Id);
            game.IsShowcased = true;
            game.DateUpdated = DateTime.UtcNow;
            _service.Edit(game);

            ShowStatusMessage(MessageTypeEnum.info, "Game added to showcase", "Showcase");

            return RedirectToAction("Index", "Showcase", _user.UserNum);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult RemoveFromShowcase(int id)
        {
            var game = _service.GetByID(id, _user.Id);
            game.IsShowcased = false;
            game.DateUpdated = DateTime.UtcNow;
            _service.Edit(game);

            ShowStatusMessage(MessageTypeEnum.info, "Game removed from showcase", "Showcase");

            return RedirectToAction("Index", "Showcase", _user.UserNum);
        }

        [Authorize]
        [HttpGet]
        public IActionResult IncreaseCompletionCount(int id)
        {
            var game = _service.GetByID(id, _user.Id);

            if (game.UserID != _user.Id)
            {
                ShowStatusMessage(MessageTypeEnum.warning, "This game cannot be edited by another user.", "Edit Failure");

                return RedirectToAction("Index", "Game");
            }

            game.TimesCompleted += 1;
            game.CompletionStatus = CompletionStatus.Completed;
            game.LastCompleted = DateTime.UtcNow;
            game.DateUpdated = DateTime.UtcNow;
            _service.Edit(game);

            ShowStatusMessage(MessageTypeEnum.info, "Game was updated.", "Update");

            return RedirectToAction("Index", "Game");
        }

        [Authorize]
        [HttpGet]
        public IActionResult DecreaseCompletionCount(int id)
        {
            var game = _service.GetByID(id, _user.Id);

            if (game.UserID != _user.Id)
            {
                ShowStatusMessage(MessageTypeEnum.warning, "This game cannot be edited by another user.", "Edit Failure");

                return RedirectToAction("Index", "Game");
            }

            if (game.TimesCompleted > 0) game.TimesCompleted -= 1;
            if (game.TimesCompleted == 0) game.CompletionStatus = CompletionStatus.NotStarted;
            game.DateUpdated = DateTime.UtcNow;
            _service.Edit(game);

            ShowStatusMessage(MessageTypeEnum.info, "Game was updated.", "Update");

            return RedirectToAction("Index", "Game");
        }

        [Authorize]
        [HttpGet]
        public IActionResult AddToQueue(int id)
        {
            var game = _service.GetByID(id, _user.Id);

            if (game.UserID != _user.Id)
            {
                ShowStatusMessage(MessageTypeEnum.error, "This game cannot be edited by another user.", "Edit Failure");

                return RedirectToAction("Index", "Game");
            }

            if (game.IsQueued)
            {
                ShowStatusMessage(MessageTypeEnum.warning, "This game is already queued.", "Edit Failure");

                return RedirectToAction("Index", "Game");
            }

            game.IsQueued = true;
            var currentHighestRank = _service.GetHighestQueueRank(_user.Id);
            game.QueueRank = currentHighestRank + 1;

            _service.Edit(game);

            ShowStatusMessage(MessageTypeEnum.info, "Game added to queue", "Queue");

            return RedirectToAction("Index", "Queue");
        }

        [Authorize]
        [HttpGet]
        public IActionResult RemoveFromQueue(int id)
        {
            var game = _service.GetByID(id, _user.Id);

            if (game.UserID != _user.Id)
            {
                ShowStatusMessage(MessageTypeEnum.error, "This game cannot be edited by another user.", "Edit Failure");

                return RedirectToAction("Index", "Game");
            }

            game.IsQueued = false;
            game.QueueRank = 0;

            _service.Edit(game);

            ShowStatusMessage(MessageTypeEnum.info, "Game removed from queue,", "Queue");

            return RedirectToAction("Index", "Queue");
        }
    }
}