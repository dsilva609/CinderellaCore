using CinderellaCore.Model.Models.Import;
using System.Threading.Tasks;

namespace CinderellaCore.Services.Services.Interfaces
{
	public interface IImportService
	{
		Task<ImportResponse> ImportAlbumsAsync(AlbumImportRequest request);

		Task<ImportResponse> ImportBooksAsync(BookImportRequest request);
	}
}