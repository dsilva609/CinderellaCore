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

		public ImportService(UserManager<ApplicationUser> userManager, IAlbumService albumService, IBookService bookService)
		{
			_userManager = userManager;
			_albumService = albumService;
			_bookService = bookService;
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