using CinderellaCore.Model.Models;
using CinderellaCore.Model.Models.Import;
using CinderellaCore.Services.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace CinderellaCore.Services.Services
{
    public class ImportService : IImportService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAlbumService _albumService;
        private readonly IBookService _bookService;
        private readonly IGameService _gameService;
        private readonly IMovieService _movieService;
        private readonly IPopService _popService;

        public ImportService(UserManager<ApplicationUser> userManager, IAlbumService albumService, IBookService bookService, IGameService gameService, IMovieService movieService, IPopService popService)
        {
            _userManager = userManager;
            _albumService = albumService;
            _bookService = bookService;
            _gameService = gameService;
            _movieService = movieService;
            _popService = popService;
        }

        public async Task<ImportResponse> ImportAlbumsAsync(AlbumImportRequest request)
        {
            ImportResponse response = await CheckRequest(request.UserID, request.Albums.Count);
            if (!response.Successful) return response;

            var user = await _userManager.FindByIdAsync(request.UserID);
            foreach (var album in request.Albums)
            {
                try
                {
                    album.UserID = request.UserID;
                    album.UserNum = user.UserNum;
                    album.ID = 0;
                    _albumService.Add(album);
                    response.Imported++;
                }
                catch (Exception ex)
                {
                    response.Failed++;
                    response.Message += $"{album.Title}- {ex.Message} : {ex.InnerException?.Message} ,";
                }
            }

            response.Successful = true;

            return response;
        }

        public async Task<ImportResponse> ImportBooksAsync(BookImportRequest request)
        {
            ImportResponse response = await CheckRequest(request.UserID, request.Books.Count);
            if (!response.Successful) return response;

            var user = await _userManager.FindByIdAsync(request.UserID);
            foreach (var book in request.Books)
            {
                try
                {
                    book.UserID = request.UserID;
                    book.UserNum = user.UserNum;
                    book.ID = 0;
                    _bookService.Add(book);
                    response.Imported++;
                }
                catch (Exception ex)
                {
                    response.Failed++;
                    response.Message += $"{book.Title} - {ex.Message} : {ex.InnerException?.Message}";
                }
            }

            response.Successful = true;

            return response;
        }

        public async Task<ImportResponse> ImportGamesAsync(GameImportRequest request)
        {
            ImportResponse response = await CheckRequest(request.UserID, request.Games.Count);
            if (!response.Successful) return response;

            var user = await _userManager.FindByIdAsync(request.UserID);
            foreach (var game in request.Games)
            {
                try
                {
                    game.UserID = request.UserID;
                    game.UserNum = user.UserNum;
                    game.ID = 0;
                    _gameService.Add(game);
                    response.Imported++;
                }
                catch (Exception ex)
                {
                    response.Failed++;
                    response.Message += $"{game.Title} - {ex.Message} : {ex.InnerException?.Message}";
                }
            }

            response.Successful = true;

            return response;
        }

        public async Task<ImportResponse> ImportMoviesAsync(MovieImportRequest request)
        {
            ImportResponse response = await CheckRequest(request.UserID, request.Movies.Count);
            if (!response.Successful) return response;

            var user = await _userManager.FindByIdAsync(request.UserID);
            foreach (var movie in request.Movies)
            {
                try
                {
                    movie.UserID = request.UserID;
                    movie.UserNum = user.UserNum;
                    movie.ID = 0;
                    _movieService.Add(movie);
                    response.Imported++;
                }
                catch (Exception ex)
                {
                    response.Failed++;
                    response.Message += $"{movie.Title} - {ex.Message} : {ex.InnerException?.Message}";
                }
            }

            response.Successful = true;

            return response;
        }

        public async Task<ImportResponse> ImportPopsAsync(PopImportRequest request)
        {
            ImportResponse response = await CheckRequest(request.UserID, request.Pops.Count);
            if (!response.Successful) return response;

            var user = await _userManager.FindByIdAsync(request.UserID);
            foreach (var pop in request.Pops)
            {
                try
                {
                    pop.UserID = request.UserID;
                    pop.UserNum = user.UserNum;
                    pop.ID = 0;
                    _popService.Add(pop);
                    response.Imported++;
                }
                catch (Exception ex)
                {
                    response.Failed++;
                    response.Message += $"{pop.Title} - {ex.Message} : {ex.InnerException?.Message}";
                }
            }

            response.Successful = true;

            return response;
        }

        private async Task<ImportResponse> CheckRequest(string userID, int count)
        {
            var response = new ImportResponse
            {
                NumRequested = count
            };
            if (string.IsNullOrWhiteSpace(userID))
            {
                response.Failed = count;
                response.Message = "User ID Missing";

                return response;
            }

            var user = await _userManager.FindByIdAsync(userID);
            if (!user.EnableImport)
            {
                response.Failed = count;
                response.Message = "Import is not enabled for this user";

                return response;
            }

            response.Successful = true;
            return response;
        }
    }
}