using CinderellaCore.Model.Models.ComicVine;
using System.Threading.Tasks;

namespace CinderellaCore.Services.Features.Book
{
    public interface IComicVineService
    {
        Task<ComicVineResult> Search(string query);

        Task<Model.Models.Book> SearchByID(string id);
    }
}