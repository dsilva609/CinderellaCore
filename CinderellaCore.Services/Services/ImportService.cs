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

        public ImportService(UserManager<ApplicationUser> userManager, IAlbumService albumService)
        {
            _userManager = userManager;
            _albumService = albumService;
        }

        public async Task<ImportResponse> ImportAlbumAsync(AlbumImportRequest request)
        {
            ImportResponse response = await CheckRequest(request);
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
                }
            }

            response.Successful = true;

            return response;
        }

        private async Task<ImportResponse> CheckRequest(AlbumImportRequest request)
        {
            var response = new ImportResponse
            {
                NumRequested = request.Albums.Count
            };
            if (string.IsNullOrWhiteSpace(request.UserID))
            {
                response.Failed = request.Albums.Count;
                response.Message = "User ID Missing";

                return response;
            }

            var user = await _userManager.FindByIdAsync(request.UserID);
            if (!user.EnableImport)
            {
                response.Failed = request.Albums.Count;
                response.Message = "Import is not enabled for this user";

                return response;
            }

            response.Successful = true;
            return response;
        }
    }
}