using CinderellaCore.Model.Models.Import;
using System.Threading.Tasks;

namespace CinderellaCore.Services.Services.Interfaces
{
    public interface IImportService
    {
        Task<ImportResponse> ImportAlbumsAsync(AlbumImportRequest request);

        Task<ImportResponse> ImportBooksAsync(BookImportRequest request);

        Task<ImportResponse> ImportGamesAsync(GameImportRequest request);

        Task<ImportResponse> ImportMoviesAsync(MovieImportRequest request);

        Task<ImportResponse> ImportPopsAsync(PopImportRequest request);
    }
}