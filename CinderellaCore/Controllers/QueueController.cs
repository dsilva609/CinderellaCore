using CinderellaCore.Model.Enums;
using CinderellaCore.Model.Models;
using CinderellaCore.Services.Features.Album;
using CinderellaCore.Services.Features.Book;
using CinderellaCore.Services.Features.Game;
using CinderellaCore.Services.Features.Movie;
using CinderellaCore.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace CinderellaCore.Web.Controllers
{
    public class QueueController : CinderellaCoreBaseController
    {
        private readonly IAlbumService _albumService;
        private readonly IBookService _bookService;
        private readonly IGameService _gameService;
        private readonly IMovieService _movieService;

        public QueueController(UserManager<ApplicationUser> userManager, IAlbumService albumService, IBookService bookService, IGameService gameService,
            IMovieService movieService) : base(userManager)
        {
            _albumService = albumService;
            _bookService = bookService;
            _gameService = gameService;
            _movieService = movieService;
        }

        [Authorize]
        [HttpGet]
        public IActionResult Index()
        {
            var model = new QueueViewModel
            {
                Albums = _albumService.GetAll(_user.Id)
                    .Where(x => x.IsQueued)
                    ?.OrderBy(y => y.QueueRank)
                    .Select(
                        z =>
                            new QueueItemViewModel
                            {
                                ID = z.ID,
                                Title = z.Title,
                                ImageUrl = z.ImageUrl,
                                QueueRank = z.QueueRank,
                                ItemType = ItemType.Album
                            })
                    .ToList(),
                Books = _bookService.GetAll(_user.Id)
                    .Where(x => x.IsQueued)
                    .OrderBy(y => y.QueueRank)
                    .Select(
                        z =>
                            new QueueItemViewModel
                            {
                                ID = z.ID,
                                Title = z.Title,
                                ImageUrl = z.ImageUrl,
                                QueueRank = z.QueueRank,
                                ItemType = ItemType.Book
                            })
                    .ToList(),
                Games = _gameService.GetAll(_user.Id)
                    .Where(x => x.IsQueued)
                    .OrderBy(y => y.QueueRank)
                    .Select(
                        z =>
                            new QueueItemViewModel
                            {
                                ID = z.ID,
                                Title = z.Title,
                                ImageUrl = z.ImageUrl,
                                QueueRank = z.QueueRank,
                                ItemType = ItemType.Game
                            })
                    .ToList(),
                Movies = _movieService.GetAll(_user.Id)
                    .Where(x => x.IsQueued)
                    .OrderBy(y => y.QueueRank)
                    .Select(
                        z =>
                            new QueueItemViewModel
                            {
                                ID = z.ID,
                                Title = z.Title,
                                ImageUrl = z.ImageUrl,
                                QueueRank = z.QueueRank,
                                ItemType = ItemType.Movie
                            })
                    .ToList()
            };

            return View(model);
        }
    }
}